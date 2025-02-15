using GeminiDotnet.ContentGeneration;
using GeminiDotnet.ContentGeneration.FunctionCalling;
using GeminiDotnet.Embeddings;
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
                    throw new InvalidOperationException("Cannot use multiple system prompts.");
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

            throw new NotSupportedException($"Unsupported {nameof(MEAI.ChatRole)}: {role}");
        }

        static Part CreateMappedPart(MEAI.AIContent content)
        {
            return content switch
            {
                MEAI.TextContent textContent => CreateTextPart(textContent),
                MEAI.DataContent dataContent => CreateInlineDataPart(dataContent),
                MEAI.FunctionCallContent functionCall => CreateFunctionCallPart(functionCall),
                MEAI.FunctionResultContent functionResult => CreateFunctionResponsePart(functionResult),
                _ => throw new NotSupportedException($"Unsupported {nameof(MEAI.AIContent)} type: {content.GetType()}")
            };
        }

        static Part CreateTextPart(MEAI.TextContent textContent)
        {
            return new Part { Text = textContent.Text };
        }

        static Part CreateInlineDataPart(MEAI.DataContent dataContent)
        {
            if (dataContent.Data is null)
            {
                throw new InvalidOperationException(
                    $"{nameof(MEAI.DataContent.Data)} cannot be null when creating an {nameof(Blob)}");
            }

            if (dataContent.MediaType is null)
            {
                throw new InvalidOperationException(
                    $"{nameof(MEAI.DataContent.MediaType)} cannot be null when creating an {nameof(Blob)}");
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
                    Id = functionCall.CallId, Name = functionCall.Name, Arguments = functionCall.Arguments,
                }
            };
        }

        static Part CreateFunctionResponsePart(MEAI.FunctionResultContent functionResult)
        {
            return new Part
            {
                FunctionResponse = new FunctionResponse
                {
                    Name = functionResult.Name, Response = functionResult.Result!.ToString()!
                }
            };
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