using GeminiDotnet.ContentGeneration;
using Microsoft.Extensions.AI;
using System.Diagnostics;

namespace GeminiDotnet.Extensions.AI;

internal static class GeminiToMEAIMapper
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
        return finishReason switch
        {
            FinishReason.Stop => ChatFinishReason.Stop,
            FinishReason.MaxTokens => ChatFinishReason.Length,
            FinishReason.Safety => ChatFinishReason.ContentFilter,
            _ => null
        };
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

        string type;

        if (messagePart.FunctionCall is not null)
        {
            type = nameof(Part.FunctionCall);
        }
        else if (messagePart.FunctionResponse is not null)
        {
            type = nameof(Part.FunctionResponse);
        }
        else if (messagePart.FileData is not null)
        {
            type = nameof(Part.FileData);
        }
        else if (messagePart.ExecutableCode is not null)
        {
            type = nameof(Part.ExecutableCode);
        }
        else if (messagePart.CodeExecutionResult is not null)
        {
            type = nameof(Part.CodeExecutionResult);
        }
        else
        {
            throw new UnreachableException($"All properties of {nameof(Part)} are null.");
        }

        GeminiMappingException.Throw(
            fromPropertyName: $"{typeof(Part)}",
            toPropertyName: $"{typeof(AIContent)}",
            reason: $"Unsupported {nameof(Part)} type: {type}");

        return null!; // Unreachable

        static DataContent CreateMappedDataContent(Part part)
        {
            Debug.Assert(part.InlineData is not null);
            var inlineData = part.InlineData;

            return new DataContent(inlineData.Data, inlineData.MimeType)
            {
                RawRepresentation = part.InlineData, AdditionalProperties = null
            };
        }

        static TextContent CreateMappedTextContent(Part part)
        {
            Debug.Assert(part.Text is not null);
            return new TextContent(part.Text) { RawRepresentation = part.Text, AdditionalProperties = null };
        }
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

        static UsageDetails CreateMappedUsageDetails(UsageMetadata usage)
        {
            return new UsageDetails
            {
                InputTokenCount = usage.PromptTokenCount,
                OutputTokenCount = usage.CandidatesTokenCount ?? 0,
                TotalTokenCount = usage.TotalTokenCount,
                AdditionalCounts = null,
            };
        }

        static ChatMessage CreateMappedChatMessage(Candidate candidateResponse)
        {
            return new ChatMessage
            {
                AuthorName = null,
                Role = CreateMappedChatRole(candidateResponse.Content.Role),
                Contents = candidateResponse.Content.Parts.Select(CreateMappedAIContent).ToList(),
                RawRepresentation = candidateResponse,
                AdditionalProperties = null
            };
        }
    }

    private static ChatRole CreateMappedChatRole(string? role)
    {
        if (role is null)
        {
            return ChatRole.System;
        }

        if (string.Equals(role, ChatRoles.User, StringComparison.OrdinalIgnoreCase))
        {
            return ChatRole.User;
        }

        if (string.Equals(role, ChatRoles.Model, StringComparison.OrdinalIgnoreCase))
        {
            return ChatRole.Assistant;
        }

        GeminiMappingException.Throw(
            fromPropertyName: $"{typeof(Candidate)}.{nameof(Content.Role)}",
            toPropertyName: $"{typeof(ChatRole)}",
            reason: $"Unsupported role: {role}");

        return default; // Unreachable
    }


    public static GeneratedEmbeddings<Embedding<float>> CreateMappedGeneratedEmbeddings(
        Embeddings.EmbeddingResponse response)
    {
        var rawEmbedding = response.Embedding.Values;

        // Currently all models return 768-dimensional embeddings.
        // https://ai.google.dev/gemini-api/docs/models/gemini?#text-embedding
        const int embeddingSize = 768;

        if (rawEmbedding.Length % embeddingSize != 0)
        {
            throw new InvalidDataException("The response embedding size is not a multiple of the expected size.");
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
