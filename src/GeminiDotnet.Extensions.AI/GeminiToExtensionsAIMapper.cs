using GeminiDotnet.ContentGeneration;
using Microsoft.Extensions.AI;
using System.Diagnostics;

namespace GeminiDotnet.Extensions.AI;

internal static class GeminiToExtensionsAIMapper
{
    public static StreamingChatCompletionUpdate CreateMappedStreamingChatCompletionUpdate(
        GenerateContentResponse response,
        DateTimeOffset createdAt)
    {
        var candidate = response.Candidates.Single();

        return new StreamingChatCompletionUpdate
        {
            AuthorName = null,
            Role = CreateMappedChatRole(candidate.Content.Role),
            Contents = candidate.Content.Parts.Select(CreateMappedAIContent).ToList(),
            RawRepresentation = response,
            AdditionalProperties = null,
            CompletionId = null,
            CreatedAt = createdAt,
            ChoiceIndex = 0,
            FinishReason = CreateMappedChatFinishReason(candidate.FinishReason),
            ModelId = response.ModelVersion
        };
    }

    private static ChatFinishReason? CreateMappedChatFinishReason(FinishReason? finishReason)
    {
        if (finishReason is null)
        {
            return null;
        }

        if (finishReason == FinishReason.Stop)
        {
            return ChatFinishReason.Stop;
        }

        if (finishReason == FinishReason.MaxTokens)
        {
            return ChatFinishReason.Length;
        }

        if (finishReason == FinishReason.Safety)
        {
            return ChatFinishReason.ContentFilter;
        }

        return null;
    }

    private static AIContent CreateMappedAIContent(Part messagePart)
    {
        if (messagePart.Text is not null)
        {
            return CreateMappedTextContent(messagePart);
        }

        if (messagePart.InlineData is not null)
        {
            return CreateMappedDataContent(messagePart);
        }

        throw new NotSupportedException($"Unsupported {nameof(Part)} type: {messagePart.GetType()}");
    }

    private static DataContent CreateMappedDataContent(Part part)
    {
        Debug.Assert(part.InlineData is not null);

        var inlineData = part.InlineData;

        return new DataContent(inlineData.Data, inlineData.MimeType)
        {
            RawRepresentation = part.InlineData,
            AdditionalProperties = null
        };
    }

    private static TextContent CreateMappedTextContent(Part part)
    {
        Debug.Assert(part.Text is not null);
        return new TextContent(part.Text) { RawRepresentation = part.Text, AdditionalProperties = null };
    }

    public static ChatCompletion CreateMappedChatCompletion(GenerateContentResponse response, DateTimeOffset createdAt)
    {
        var choices = response.Candidates.Select(CreateMappedChatMessage).ToList();

        return new ChatCompletion(choices)
        {
            CompletionId = null,
            ModelId = response.ModelVersion,
            CreatedAt = createdAt,
            FinishReason = CreateMappedChatFinishReason(response.Candidates.First().FinishReason),
            Usage = CreateMappedUsageDetails(response.UsageMetadata),
            RawRepresentation = response,
            AdditionalProperties = null
        };
    }

    private static UsageDetails CreateMappedUsageDetails(UsageMetadata usage)
    {
        return new UsageDetails
        {
            InputTokenCount = usage.PromptTokenCount,
            OutputTokenCount = usage.CandidatesTokenCount ?? 0,
            TotalTokenCount = usage.TotalTokenCount,
            AdditionalCounts = new AdditionalPropertiesDictionary<long>
            {
                ["promptTokenCount"] = usage.PromptTokenCount,
                ["candidatesTokenCount"] = usage.CandidatesTokenCount ?? 0,
                ["totalTokenCount"] = usage.TotalTokenCount
            }
        };
    }

    private static Microsoft.Extensions.AI.ChatMessage CreateMappedChatMessage(Candidate candidateResponse)
    {
        return new Microsoft.Extensions.AI.ChatMessage
        {
            AuthorName = null,
            Role = CreateMappedChatRole(candidateResponse.Content.Role),
            Contents = candidateResponse.Content.Parts.Select(CreateMappedAIContent).ToList(),
            RawRepresentation = null,
            AdditionalProperties = null
        };
    }

    private static Microsoft.Extensions.AI.ChatRole CreateMappedChatRole(string role)
    {
        if (string.Equals(role, "user", StringComparison.OrdinalIgnoreCase))
        {
            return Microsoft.Extensions.AI.ChatRole.User;
        }

        if (string.Equals(role, "model", StringComparison.OrdinalIgnoreCase))
        {
            return Microsoft.Extensions.AI.ChatRole.Assistant;
        }

        // role == TextGeneration.ChatRole.System
        // role == TextGeneration.ChatRole.Tool

        throw new NotSupportedException($"Unsupported {nameof(ChatRole)}: {role}");
    }


    public static GeneratedEmbeddings<Embedding<float>> CreateMappedGeneratedEmbeddings(
        Embeddings.EmbeddingResponse response)
    {
        var rawEmbedding = response.Embedding.Values;

        const int embeddingSize = 768;

        if (rawEmbedding.Length % embeddingSize != 0)
        {
            throw new InvalidOperationException("The response embedding size is not a multiple of the expected size.");
        }

        var generatedEmbeddings = new GeneratedEmbeddings<Embedding<float>>(rawEmbedding.Length / embeddingSize);

        for (int i = 0; i < rawEmbedding.Length; i += embeddingSize)
        {
            var embedding = new Embedding<float>(rawEmbedding.AsMemory(i, embeddingSize));
            generatedEmbeddings.Add(embedding);
        }

        return generatedEmbeddings;
    }
}