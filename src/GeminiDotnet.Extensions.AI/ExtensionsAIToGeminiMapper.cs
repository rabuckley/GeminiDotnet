using GeminiDotnet.ContentGeneration;
using GeminiDotnet.Embeddings;
using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

internal static class ExtensionsAIToGeminiMapper
{
    public static GenerateContentRequest CreateMappedTextGenerationRequest(
        IList<Microsoft.Extensions.AI.ChatMessage> chatMessages)
    {
        return new GenerateContentRequest { Contents = chatMessages.Select(CreateGeminiChatMessage).ToList() };

        static Content CreateGeminiChatMessage(Microsoft.Extensions.AI.ChatMessage chatMessage)
        {
            return new Content
            {
                Role = CreateGeminiChatRole(chatMessage.Role),
                Parts = chatMessage.Contents.Select(CreateGeminiChatMessagePart).ToList(),
            };
        }

        static string CreateGeminiChatRole(Microsoft.Extensions.AI.ChatRole role)
        {
            if (role == Microsoft.Extensions.AI.ChatRole.User)
            {
                return ChatRoles.User;
            }

            if (role == Microsoft.Extensions.AI.ChatRole.Assistant)
            {
                return ChatRoles.Model;
            }

            throw new NotSupportedException($"Unsupported {nameof(Microsoft.Extensions.AI.ChatRole)}: {role}");
        }
    }

    private static Part CreateGeminiChatMessagePart(AIContent content)
    {
        return content switch
        {
            TextContent textContent => CreateGeminiTextChatMessagePart(textContent),
            DataContent dataContent => CreateGeminiInlineDataChatMessagePart(dataContent),
            _ => throw new NotSupportedException($"Unsupported {nameof(AIContent)} type: {content.GetType()}")
        };
    }

    private static Part CreateGeminiTextChatMessagePart(TextContent textContent)
    {
        return new Part { Text = textContent.Text };
    }

    private static Part CreateGeminiInlineDataChatMessagePart(DataContent dataContent)
    {
        if (dataContent.Data is null)
        {
            throw new InvalidOperationException(
                $"{nameof(DataContent.Data)} cannot be null when creating an {nameof(Blob)}");
        }

        if (dataContent.MediaType is null)
        {
            throw new InvalidOperationException(
                $"{nameof(DataContent.MediaType)} cannot be null when creating an {nameof(Blob)}");
        }

        return new Part { InlineData = new Blob { Data = dataContent.Data.Value, MimeType = dataContent.MediaType } };
    }

    public static EmbeddingRequest CreateMappedEmbeddingRequest(IEnumerable<string> values)
    {
        return new EmbeddingRequest
        {
            Content = new EmbeddingContent { Parts = [.. values.Select(v => new Part { Text = v })] }
        };
    }
}