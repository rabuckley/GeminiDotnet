using GeminiDotnet.ContentGeneration;
using GeminiDotnet.ContentGeneration.FunctionCalling;
using GeminiDotnet.Embeddings;
using System.Diagnostics.CodeAnalysis;
using MEAI = Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

internal static class ExtensionsAIToGeminiMapper
{
    public static GenerateContentRequest CreateMappedTextGenerationRequest(IList<MEAI.ChatMessage> chatMessages)
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

        return new GenerateContentRequest { SystemInstruction = systemInstruction, Contents = contents, };

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
    }

    public static EmbeddingRequest CreateMappedEmbeddingRequest(IEnumerable<string> values)
    {
        return new EmbeddingRequest
        {
            Content = new EmbeddingContent { Parts = [.. values.Select(v => new Part { Text = v })] }
        };
    }
}
