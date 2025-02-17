using GeminiDotnet.ContentGeneration;
using GeminiDotnet.ContentGeneration.FunctionCalling;
using Microsoft.Extensions.AI;
using System.Diagnostics;

namespace GeminiDotnet.Extensions.AI;

internal static class GeminiToMEAIMapper
{
    public static ChatResponseUpdate CreateMappedChatResponseUpdate(
        GenerateContentResponse response,
        DateTimeOffset createdAt)
    {
        var candidate = response.Candidates.Single();

        return new ChatResponseUpdate
        {
            AuthorName = null,
            Role = CreateMappedChatRole(candidate.Content.Role),
            Contents = candidate.Content.Parts.Select(CreateMappedAIContent).ToList(),
            RawRepresentation = response,
            AdditionalProperties = null,
            ResponseId = null,
            ChatThreadId = null,
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
            return CreateMappedTextContent(messagePart.Text);
        }

        if (messagePart.InlineData is not null)
        {
            return CreateMappedDataContent(messagePart.InlineData);
        }

        if (messagePart.FunctionCall is not null)
        {
            return CreateMappedFunctionCallContent(messagePart.FunctionCall);
        }

        if (messagePart.FunctionResponse is not null)
        {
            return CreateMappedFunctionResultContent(messagePart.FunctionResponse);
        }

        if (messagePart.FileData is not null)
        {
            return CreateMappedFileDataContent(messagePart.FileData);
        }

        // For now, M.E.AI does not have a representation for executable code, so map it to text.
        if (messagePart.ExecutableCode is not null)
        {
            return CreateMappedTextContent(
                $"""
                 ```{messagePart.ExecutableCode.Language}
                 {messagePart.ExecutableCode.Code}
                 ```
                 """);
        }

        if (messagePart.CodeExecutionResult is not null)
        {
            return CreateMappedTextContent(
                $"""
                 ```
                 {messagePart.CodeExecutionResult.Output}
                 ```

                 {nameof(CodeExecutionResult.Outcome)}: {messagePart.CodeExecutionResult.Outcome}
                 """);
        }

        throw new UnreachableException($"All properties of {nameof(Part)} are null.");

        static DataContent CreateMappedDataContent(Blob inlineData)
        {
            return new DataContent(inlineData.Data, inlineData.MimeType)
            {
                RawRepresentation = inlineData, AdditionalProperties = null
            };
        }

        static DataContent CreateMappedFileDataContent(FileData fileData)
        {
            return new DataContent(fileData.Uri, fileData.MimeType)
            {
                RawRepresentation = fileData, AdditionalProperties = null
            };
        }

        static TextContent CreateMappedTextContent(string text)
        {
            return new TextContent(text) { RawRepresentation = text, AdditionalProperties = null };
        }

        static FunctionCallContent CreateMappedFunctionCallContent(FunctionCall functionCall)
        {
            if (functionCall.Id is null)
            {
                GeminiMappingException.Throw(
                    fromPropertyName: $"{typeof(FunctionCall)}.{nameof(FunctionCall.Id)}",
                    toPropertyName: $"{typeof(FunctionCallContent)}",
                    reason: $"'{typeof(FunctionCall)}.{nameof(FunctionCall.Id)}' must not be null.");

                return null!; // Unreachable
            }

            return new FunctionCallContent(functionCall.Id, functionCall.Name, functionCall.Arguments)
            {
                RawRepresentation = functionCall, AdditionalProperties = null
            };
        }

        static FunctionResultContent CreateMappedFunctionResultContent(FunctionResponse functionResponse)
        {
            if (functionResponse.Id is null)
            {
                GeminiMappingException.Throw(
                    fromPropertyName: $"{typeof(FunctionResponse)}.{nameof(FunctionResponse.Id)}",
                    toPropertyName: $"{typeof(FunctionResultContent)}",
                    reason: $"'{typeof(FunctionResponse)}.{nameof(FunctionResponse.Id)}' must not be null.");

                return null!; // Unreachable
            }

            return new FunctionResultContent(functionResponse.Id, functionResponse.Response)
            {
                RawRepresentation = functionResponse, AdditionalProperties = null
            };
        }
    }

    public static ChatResponse CreateMappedChatResponse(GenerateContentResponse response, DateTimeOffset createdAt)
    {
        var choices = response.Candidates.Select(CreateMappedChatMessage).ToList();

        return new ChatResponse(choices)
        {
            ResponseId = null,
            ChatThreadId = null,
            ModelId = response.ModelVersion,
            CreatedAt = createdAt,
            FinishReason = CreateMappedChatFinishReason(response.Candidates[0].FinishReason),
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
