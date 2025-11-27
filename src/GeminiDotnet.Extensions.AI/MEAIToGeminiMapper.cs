using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.CachedContents;
using GeminiDotnet.V1Beta.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json;
using MEAI = Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

internal static class MEAIToGeminiMapper
{
    public static GenerateContentRequest CreateMappedGenerateContentRequest(
        string model,
        IEnumerable<MEAI.ChatMessage> chatMessages,
        MEAI.ChatOptions? options)
    {
        List<Content> contents = chatMessages.TryGetNonEnumeratedCount(out var count)
            ? new List<Content>(count)
            : [];

        List<Part> systemInstructionParts = options?.Instructions is { } instructions
            ? [new Part { Text = instructions }]
            : [];

        foreach (var message in chatMessages)
        {
            if (message.Role == MEAI.ChatRole.System)
            {
                AppendSystemInstructionParts(message, systemInstructionParts);
                continue;
            }

            contents.Add(CreateMappedContent(message));
        }

        var systemInstruction = systemInstructionParts.Count > 0
            ? new Content { Role = null, Parts = systemInstructionParts }
            : null;

        return new GenerateContentRequest
        {
            Model = model,
            SystemInstruction = systemInstruction,
            GenerationConfiguration = CreateMappedGenerationConfiguration(options),
            CachedContent = null,
            Contents = contents,
            Tools = CreateMappedTools(options?.Tools),
            ToolConfiguration = null,
            SafetySettings = null,
        };

        static IReadOnlyList<Tool>? CreateMappedTools(IList<MEAI.AITool>? tools)
        {
            if (tools is null)
            {
                return null;
            }

            List<Tool> mappedTools = new(tools.Count);
            List<FunctionDeclaration>? functionDeclarations = null;

            foreach (var tool in tools)
            {
                if (tool is MEAI.HostedCodeInterpreterTool)
                {
                    mappedTools.Add(new Tool { CodeExecution = new CodeExecution() });
                    continue;
                }

                if (tool is MEAI.HostedWebSearchTool)
                {
                    mappedTools.Add(new Tool { GoogleSearch = new GoogleSearch() });
                    continue;
                }

                if (tool is MEAI.AIFunction function)
                {
                    functionDeclarations ??= [];

                    functionDeclarations.Add(new FunctionDeclaration
                    {
                        Name = function.Name,
                        Description = function.Description,
                        ParametersJsonSchema = function.JsonSchema,
                        ResponseJsonSchema = function.ReturnJsonSchema ?? default,
                    });

                    continue;
                }

                GeminiMappingException.Throw(
                    fromPropertyName: $"{typeof(MEAI.AITool)}",
                    toPropertyName: $"{typeof(Tool)}",
                    reason: $"Unsupported tool type: {tool.GetType()}");
            }

            if (functionDeclarations is not null)
            {
                mappedTools.Add(new Tool { FunctionDeclarations = functionDeclarations });
            }

            return mappedTools;
        }

        static GenerationConfiguration? CreateMappedGenerationConfiguration(MEAI.ChatOptions? options)
        {
            if (options is null)
            {
                return null;
            }

            ThinkingConfiguration? thinkingConfiguration = null;

            if (options.AdditionalProperties?.TryGetValue(GeminiAdditionalProperties.ThinkingConfiguration,
                    out var thinkingConfigObj) is true
                && thinkingConfigObj is ThinkingConfiguration thinkingConfig)
            {
                thinkingConfiguration = thinkingConfig;
            }

            IReadOnlyList<ResponseModality>? responseModalities = null;

            if (options.AdditionalProperties?.TryGetValue(GeminiAdditionalProperties.ResponseModalities,
                    out var responseModalitiesObj) is true
                && responseModalitiesObj is IEnumerable<ResponseModality> responseModalitiesList)
            {
                responseModalities =
                    responseModalitiesObj as IReadOnlyList<ResponseModality> ?? responseModalitiesList.ToList();
            }

            ImageConfiguration? imageConfiguration = null;

            if (options.AdditionalProperties?.TryGetValue(GeminiAdditionalProperties.ImageConfiguration,
                   out var imageConfigObj) is true
               && imageConfigObj is ImageConfiguration imageConfig)
            {
                imageConfiguration = imageConfig;
            }

            var configuration = new GenerationConfiguration
            {
                StopSequences = options.StopSequences is null ? null : [.. options.StopSequences],
                ResponseMimeType = CreateMappedResponseMimeType(options.ResponseFormat),
                ResponseJsonSchema = CreateMappedResponseSchema(options.ResponseFormat),
                ResponseModalities = responseModalities,
                ImageConfiguration = imageConfiguration,
                CandidateCount = null,
                MaxOutputTokens = options.MaxOutputTokens,
                Temperature = options.Temperature,
                TopP = options.TopP,
                TopK = options.TopK,
                Seed = (int?)options.Seed, // TODO: can we support long seeds?
                PresencePenalty = options.PresencePenalty,
                FrequencyPenalty = options.FrequencyPenalty,
                ResponseLogprobs = null,
                Logprobs = null,
                EnableEnhancedCivicAnswers = null,
                SpeechConfiguration = null,
                ThinkingConfiguration = thinkingConfiguration
            };


            return configuration;
        }

        static JsonElement CreateMappedResponseSchema(MEAI.ChatResponseFormat? responseFormat)
        {
            if (responseFormat is null or MEAI.ChatResponseFormatText)
            {
                return default;
            }

            if (responseFormat is MEAI.ChatResponseFormatJson { Schema: JsonElement schema })
            {
                return schema;
            }

            GeminiMappingException.Throw(
                fromPropertyName: $"{typeof(MEAI.ChatOptions)}.{nameof(MEAI.ChatOptions.ResponseFormat)}",
                toPropertyName: $"{typeof(GenerationConfiguration)}.{nameof(GenerationConfiguration.ResponseSchema)}",
                reason: $"Unsupported {typeof(MEAI.ChatResponseFormat)}: '{responseFormat}'");

            return default; // unreachable
        }

        static Content CreateMappedContent(MEAI.ChatMessage chatMessage)
        {
            return new Content
            {
                Role = CreateMappedRole(chatMessage.Role), Parts = CreateMappedParts(chatMessage.Contents),
            };
        }

        static string? CreateMappedRole(MEAI.ChatRole role)
        {
            if (role == MEAI.ChatRole.System)
            {
                return null;
            }

            if (role == MEAI.ChatRole.User)
            {
                return ChatRoles.User;
            }

            if (role == MEAI.ChatRole.Assistant || role == MEAI.ChatRole.Tool)
            {
                return ChatRoles.Model;
            }

            GeminiMappingException.Throw(
                fromPropertyName: $"{typeof(MEAI.ChatMessage)}.{nameof(MEAI.ChatMessage.Role)}",
                toPropertyName: $"{typeof(Content)}.{nameof(Content.Role)}",
                reason: $"Unsupported {typeof(MEAI.ChatRole)}: '{role}'");

            return null!; // unreachable
        }

        static IReadOnlyList<Part> CreateMappedParts(IList<MEAI.AIContent> contents)
        {
            List<Part> parts = new(contents.Count);

            foreach (var content in contents)
            {
                var mapped = content switch
                {
                    MEAI.TextContent textContent => CreateTextPart(textContent),
                    MEAI.TextReasoningContent textReasoningContent => CreateTextReasoningPart(textReasoningContent),
                    MEAI.DataContent dataContent => CreateInlineDataPart(dataContent),
                    MEAI.UriContent uriContent => CreateFileDataPart(uriContent),
                    MEAI.FunctionCallContent functionCall => CreateFunctionCallPart(functionCall),
                    MEAI.FunctionResultContent functionResult => CreateFunctionResponsePart(functionResult),
                    _ => ThrowUnsupportedContentException(content),
                };

                parts.Add(mapped);
            }

            return parts;

            [DoesNotReturn]
            static Part ThrowUnsupportedContentException(MEAI.AIContent content)
            {
                GeminiMappingException.Throw(
                    fromPropertyName: content.GetType().ToString(),
                    toPropertyName: $"{typeof(Part)}",
                    reason: $"Unsupported {typeof(MEAI.AIContent)} type: {content.GetType()}");

                return null!; // unreachable
            }

            static Part CreateTextPart(MEAI.TextContent textContent)
            {
                return new Part { Text = textContent.Text };
            }

            static Part CreateInlineDataPart(MEAI.DataContent dataContent)
            {
                if (dataContent.Data.IsEmpty)
                {
                    GeminiMappingException.Throw(
                        fromPropertyName: $"{typeof(MEAI.DataContent)}.{nameof(MEAI.DataContent.Data)}",
                        toPropertyName: $"{typeof(Part)}.{nameof(Part.InlineData)}",
                        reason:
                        $"{nameof(MEAI.DataContent.Data)} cannot be empty when creating an {nameof(Part.InlineData)} part.");
                }

                return new Part
                {
                    InlineData = new Blob { Data = dataContent.Data, MimeType = dataContent.MediaType },
                    ThoughtSignature = GetThoughtSignature(dataContent),
                };
            }


            static Part CreateFileDataPart(MEAI.UriContent uriContent)
            {
                return new Part
                {
                    FileData = new FileData
                    {
                        FileUri = uriContent.Uri.ToString(), MimeType = uriContent.MediaType,
                    },
                    ThoughtSignature = GetThoughtSignature(uriContent),
                };
            }

            static Part CreateFunctionCallPart(MEAI.FunctionCallContent functionCall)
            {
                JsonElement arguments = JsonSerializer.SerializeToElement(
                    functionCall.Arguments,
                    JsonContext.Default.IDictionaryStringObject);

                return new Part
                {
                    FunctionCall = new FunctionCall
                    {
                        Id = functionCall.CallId, Name = functionCall.Name, Arguments = arguments
                    },
                    ThoughtSignature = GetThoughtSignature(functionCall)
                };
            }

            static Part CreateFunctionResponsePart(MEAI.FunctionResultContent functionResult)
            {
                var response = functionResult.Exception is null
                    ? new Dictionary<string, object?> { { "result", functionResult.Result } }
                    : new Dictionary<string, object?> { { "error", functionResult.Result }, };

                return new Part
                {
                    FunctionResponse = new FunctionResponse
                    {
                        Id = functionResult.CallId,
                        Name = functionResult.CallId,
                        Response = JsonSerializer.SerializeToElement(response,
                            JsonContext.Default.IDictionaryStringObject)
                    },
                    ThoughtSignature = GetThoughtSignature(functionResult)
                };
            }
        }

        static string? CreateMappedResponseMimeType(MEAI.ChatResponseFormat? responseFormat)
        {
            return responseFormat is MEAI.ChatResponseFormatJson ? MediaTypeNames.Application.Json : null;
        }
    }

    private static string? GetThoughtSignature(MEAI.AIContent content)
    {
        return (content.RawRepresentation as Part)?.ThoughtSignature;
    }

    private static Part CreateTextReasoningPart(MEAI.TextReasoningContent content)
    {
        return new Part { Thought = true, Text = content.Text, ThoughtSignature = content.ProtectedData };
    }

    private static void AppendSystemInstructionParts(
        MEAI.ChatMessage message,
        List<Part> systemInstructionParts)
    {
        foreach (var content in message.Contents)
        {
            if (content is not MEAI.TextContent textContent)
            {
                GeminiMappingException.Throw(
                    fromPropertyName: $"{typeof(MEAI.ChatMessage)}.{nameof(MEAI.ChatMessage.Contents)}",
                    toPropertyName:
                    $"{typeof(GenerateContentRequest)}.{nameof(GenerateContentRequest.SystemInstruction)}",
                    reason:
                    $"Only {typeof(MEAI.TextContent)} is supported in system instructions because Gemini doesn't support non-text system instructions. Got {content.GetType()}");

                return; // unreachable
            }

            systemInstructionParts.Add(new Part { Text = textContent.Text });
        }
    }

    public static EmbedContentRequest CreateMappedEmbeddingRequest(
        string model,
        IEnumerable<string> values,
        MEAI.EmbeddingGenerationOptions? options)
    {
        return new EmbedContentRequest
        {
            Model = model,
            Content = new Content { Parts = [.. values.Select(v => new Part { Text = v })] },
            OutputDimensionality = options?.Dimensions,
        };
    }
}
