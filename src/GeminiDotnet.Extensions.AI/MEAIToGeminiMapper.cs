using GeminiDotnet.ContentGeneration;
using GeminiDotnet.ContentGeneration.FunctionCalling;
using GeminiDotnet.Embeddings;
using Microsoft.Extensions.AI;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json;
using MEAI = Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

internal static class MEAIToGeminiMapper
{
    public static GenerateContentRequest CreateMappedGenerateContentRequest(
        IEnumerable<MEAI.ChatMessage> chatMessages,
        MEAI.ChatOptions? options)
    {
        List<Content> contents = chatMessages.TryGetNonEnumeratedCount(out var count)
            ? new(count)
            : new();

        Content? systemInstruction = null;

        foreach (var m in chatMessages)
        {
            if (m.Role == MEAI.ChatRole.System)
            {
                if (systemInstruction is not null)
                {
                    GeminiMappingException.Throw(
                        fromPropertyName: $"{typeof(MEAI.ChatRole)}.{nameof(MEAI.ChatRole.System)}",
                        toPropertyName:
                        $"{typeof(GenerateContentRequest)}.{nameof(GenerateContentRequest.SystemInstruction)}",
                        reason: "Cannot use multiple system instructions");

                    return null!; // unreachable
                }

                systemInstruction = CreateMappedContent(m);
                continue;
            }

            contents.Add(CreateMappedContent(m));
        }

        return new GenerateContentRequest
        {
            SystemInstruction = systemInstruction,
            GenerationConfiguration = CreateMappedGenerationConfiguration(options),
            CachedContent = null,
            Contents = contents,
            Tools = CreateMappedTools(options?.Tools),
            ToolConfiguration = null,
            SafetySettings = null,
        };

        static IEnumerable<Tool>? CreateMappedTools(IList<MEAI.AITool>? tools)
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

                if (tool is MEAI.AIFunction function)
                {
                    functionDeclarations ??= [];

                    functionDeclarations.Add(new FunctionDeclaration
                    {
                        Name = function.Name,
                        Description = function.Description,
                        Parameters = CreateMappedFunctionParameters(function.JsonSchema),
                        Response = null,
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

            var configuration = new GenerationConfiguration
            {
                StopSequences = options.StopSequences,
                ResponseMimeType = CreateMappedResponseMimeType(options.ResponseFormat),
                ResponseSchema = CreateMappedResponseSchema(options.ResponseFormat),
                ResponseModalities = null,
                CandidateCount = null,
                MaxOutputTokens = options.MaxOutputTokens,
                Temperature = options.Temperature,
                TopP = options.TopP,
                TopK = options.TopK,
                Seed = options.Seed,
                PresencePenalty = options.PresencePenalty,
                FrequencyPenalty = options.FrequencyPenalty,
                ResponseLogprobs = null,
                Logprobs = null,
                EnableEnhancedCivicAnswers = null,
                SpeechConfiguration = null,
                ThinkingConfiguration = null,
            };

            return configuration;
        }

        static Schema? CreateMappedResponseSchema(MEAI.ChatResponseFormat? responseFormat)
        {
            if (responseFormat is null)
            {
                return null;
            }

            if (responseFormat is MEAI.ChatResponseFormatJson { Schema: JsonElement schema })
            {
                return Schema.FromJsonElement(schema);
            }

            if (responseFormat is MEAI.ChatResponseFormatText)
            {
                return null;
            }

            GeminiMappingException.Throw(
                fromPropertyName: $"{typeof(MEAI.ChatOptions)}.{nameof(MEAI.ChatOptions.ResponseFormat)}",
                toPropertyName: $"{typeof(GenerationConfiguration)}.{nameof(GenerationConfiguration.ResponseSchema)}",
                reason: $"Unsupported {typeof(MEAI.ChatResponseFormat)}: '{responseFormat}'");

            return null!; // unreachable
        }

        static Content CreateMappedContent(MEAI.ChatMessage chatMessage)
        {
            return new Content
            {
                Role = CreateMappedRole(chatMessage.Role),
                Parts = chatMessage.Contents.Select(CreateMappedPart).ToList(),
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

            if (role == MEAI.ChatRole.Assistant)
            {
                return ChatRoles.Model;
            }

            if (role == MEAI.ChatRole.Tool)
            {
                return ChatRoles.Model;
            }

            GeminiMappingException.Throw(
                fromPropertyName: $"{typeof(MEAI.ChatMessage)}.{nameof(MEAI.ChatMessage.Role)}",
                toPropertyName: $"{typeof(Content)}.{nameof(Content.Role)}",
                reason: $"Unsupported {typeof(MEAI.ChatRole)}: '{role}'");

            return null!; // unreachable
        }

        static Part CreateMappedPart(MEAI.AIContent content)
        {
            return content switch
            {
                MEAI.TextContent textContent => CreateTextPart(textContent),
                MEAI.DataContent dataContent => CreateInlineDataPart(dataContent),
                MEAI.UriContent uriContent => CreateFileDataPart(uriContent),
                MEAI.FunctionCallContent functionCall => CreateFunctionCallPart(functionCall),
                MEAI.FunctionResultContent functionResult => CreateFunctionResponsePart(functionResult),
                _ => ThrowUnsupportedContentException(content),
            };

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
                    InlineData = new Blob { Data = dataContent.Data, MimeType = dataContent.MediaType }
                };
            }


            static Part CreateFileDataPart(UriContent uriContent)
            {
                return new Part
                {
                    FileData = new FileData { Uri = uriContent.Uri, MimeType = uriContent.MediaType }
                };
            }

            static Part CreateFunctionCallPart(MEAI.FunctionCallContent functionCall)
            {
                return new Part
                {
                    FunctionCall = new FunctionCall
                    {
                        Id = functionCall.CallId,
                        Name = functionCall.Name,
                        Arguments = functionCall.Arguments,
                    }
                };
            }

            static Part CreateFunctionResponsePart(MEAI.FunctionResultContent functionResult)
            {
                var jsonTypeInfo = MEAI.AIJsonUtilities.DefaultOptions.GetTypeInfo(typeof(object));

                return new Part
                {
                    FunctionResponse = new FunctionResponse
                    {
                        Id = functionResult.CallId,
                        Name = functionResult.CallId,
                        Response = new Dictionary<string, JsonElement>
                        {
                            {
                                "content",
                                JsonSerializer.SerializeToElement(functionResult.Result, jsonTypeInfo)
                            }
                        }
                    }
                };
            }
        }

        static string? CreateMappedResponseMimeType(MEAI.ChatResponseFormat? responseFormat)
        {
            return responseFormat is MEAI.ChatResponseFormatJson ? MediaTypeNames.Application.Json : null;
        }
    }

    private static ObjectSchema? CreateMappedFunctionParameters(JsonElement functionSchema)
    {
        // The element describes the whole function. For a function with no properties, this looks like
        //
        // ```json
        // {
        //     "title": "GetCurrentWeather",
        //     "description": "Gets the current weather",
        //     "type": "object",
        //     "properties": {}
        // }
        // ```
        // 
        // We want
        //
        // ```json
        // {
        //     "name": "GetCurrentWeather",
        //     "description": "Gets the current weather",
        //     "parameters": {
        //         "type": "object",
        //         "properties": {}
        //     }
        // }
        // ```

        var properties = functionSchema.GetProperty("properties");

        if (properties.ValueKind != JsonValueKind.Object)
        {
            GeminiMappingException.Throw(
                fromPropertyName: $"{functionSchema}",
                toPropertyName: $"{typeof(FunctionDeclaration)}.{nameof(FunctionDeclaration.Parameters)}",
                reason: $"Expected {JsonValueKind.Object} but got {properties.ValueKind}");
        }

        Dictionary<string, Schema>? parameters = null;

        foreach (var param in properties.EnumerateObject())
        {
            parameters ??= new Dictionary<string, Schema>();
            parameters[param.Name] = Schema.FromJsonElement(param.Value);
        }

        if (parameters is null)
        {
            // Gemini API expects all OBJECT schemas to have at least one property. For no parameters, return null.
            return null;
        }

        return new ObjectSchema
        {
            Format = null,
            Description = null,
            Nullable = null,
            Properties = parameters,
            RequiredProperties = null
        };
    }

    public static EmbedContentRequest CreateMappedEmbeddingRequest(
        IEnumerable<string> values,
        MEAI.EmbeddingGenerationOptions? options)
    {
        return new EmbedContentRequest
        {
            Content = new Content { Parts = [.. values.Select(v => new Part { Text = v })] },
            OutputDimensionality = options?.Dimensions,
        };
    }
}
