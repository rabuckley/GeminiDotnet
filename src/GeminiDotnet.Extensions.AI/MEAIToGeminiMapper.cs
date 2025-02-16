using GeminiDotnet.ContentGeneration;
using GeminiDotnet.ContentGeneration.FunctionCalling;
using GeminiDotnet.Embeddings;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json;
using MEAI = Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

internal static class MEAIToGeminiMapper
{
    public static GenerateContentRequest CreateMappedGenerateContentRequest(
        IList<MEAI.ChatMessage> chatMessages,
        MEAI.ChatOptions options)
    {
        List<Content> contents = new(chatMessages.Count);
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
            Tools = CreateMappedTools(options.Tools),
            ToolConfiguration = null,
            SafetySettings = null,
        };

        static IEnumerable<Tool>? CreateMappedTools(IList<MEAI.AITool>? tools)
        {
            // TODO-TOOLS
            return null;
        }

        static GenerationConfiguration? CreateMappedGenerationConfiguration(MEAI.ChatOptions options)
        {
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
                if (dataContent.Data is null)
                {
                    GeminiMappingException.Throw(
                        fromPropertyName: $"{typeof(MEAI.DataContent)}.{nameof(MEAI.DataContent.Data)}",
                        toPropertyName: $"{typeof(Part)}.{nameof(Part.InlineData)}",
                        reason:
                        $"{nameof(MEAI.DataContent.Data)} cannot be null when creating an {nameof(Part.InlineData)} part.");
                }

                if (dataContent.MediaType is null)
                {
                    GeminiMappingException.Throw(
                        fromPropertyName: $"{typeof(MEAI.DataContent)}.{nameof(MEAI.DataContent.MediaType)}",
                        toPropertyName: $"{typeof(Part)}.{nameof(Part.InlineData)}",
                        reason:
                        $"{nameof(MEAI.DataContent.MediaType)} cannot be null when creating an {nameof(Part.InlineData)} part.");
                }

                return new Part
                {
                    InlineData = new Blob { Data = dataContent.Data.Value, MimeType = dataContent.MediaType }
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
                return new Part
                {
                    FunctionResponse = new FunctionResponse
                    {
                        Name = functionResult.Name,
                        Response = functionResult.Result!.ToString()!
                    }
                };
            }
        }

        static string? CreateMappedResponseMimeType(MEAI.ChatResponseFormat? responseFormat)
        {
            return responseFormat is MEAI.ChatResponseFormatJson ? MediaTypeNames.Application.Json : null;
        }
    }

    public static EmbeddingRequest CreateMappedEmbeddingRequest(IEnumerable<string> values)
    {
        return new EmbeddingRequest
        {
            Content = new EmbeddingContent { Parts = [.. values.Select(v => new Part { Text = v })] }
        };
    }
}
