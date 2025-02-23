using GeminiDotnet.ContentGeneration;
using GeminiDotnet.ContentGeneration.FunctionCalling;
using GeminiDotnet.Embeddings;
using GeminiDotnet.Extensions.AI.Contents;
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

        if (messagePart.ExecutableCode is not null)
        {
            return CreateMappedExecutableCodeContent(messagePart.ExecutableCode);
        }

        if (messagePart.CodeExecutionResult is not null)
        {
            return CreateMappedCodeExecutionResultContent(messagePart.CodeExecutionResult);
        }

        throw new UnreachableException($"All properties of {nameof(Part)} are null.");

        static DataContent CreateMappedDataContent(Blob inlineData)
        {
            return new DataContent(inlineData.Data, inlineData.MimeType)
            {
                RawRepresentation = inlineData,
                AdditionalProperties = null
            };
        }

        static DataContent CreateMappedFileDataContent(FileData fileData)
        {
            return new DataContent(fileData.Uri, fileData.MimeType)
            {
                RawRepresentation = fileData,
                AdditionalProperties = null
            };
        }

        static TextContent CreateMappedTextContent(string text)
        {
            return new TextContent(text) { RawRepresentation = text, AdditionalProperties = null };
        }

        static FunctionCallContent CreateMappedFunctionCallContent(FunctionCall functionCall)
        {
            var callId = functionCall.Id ?? $"{functionCall.Name}/{Guid.NewGuid()}";

            return new FunctionCallContent(callId, functionCall.Name, functionCall.Arguments)
            {
                RawRepresentation = functionCall,
                AdditionalProperties = null
            };
        }

        static FunctionResultContent CreateMappedFunctionResultContent(FunctionResponse functionResponse)
        {
            var responseId = functionResponse.Id ?? $"{functionResponse.Name}/{Guid.NewGuid()}";

            return new FunctionResultContent(responseId, functionResponse.Response)
            {
                RawRepresentation = functionResponse,
                AdditionalProperties = null
            };
        }

        static ExecutableCodeContent CreateMappedExecutableCodeContent(ExecutableCode executableCode)
        {
            return new ExecutableCodeContent
            {
                Language = executableCode.Language,
                Code = executableCode.Code,
                RawRepresentation = executableCode,
                AdditionalProperties = null
            };
        }

        static CodeExecutionContent CreateMappedCodeExecutionResultContent(
            CodeExecutionResult codeExecutionResult)
        {
            return new CodeExecutionContent
            {
                Output = codeExecutionResult.Output,
                Status = codeExecutionResult.Outcome switch
                {
                    CodeExecutionOutcome.Unspecified => CodeExecutionStatus.None,
                    CodeExecutionOutcome.Ok => CodeExecutionStatus.Success,
                    CodeExecutionOutcome.Failed => CodeExecutionStatus.Error,
                    CodeExecutionOutcome.DeadlineExceeded => CodeExecutionStatus.Timeout,
                    _ => throw new ArgumentOutOfRangeException(nameof(codeExecutionResult.Outcome), codeExecutionResult.Outcome, null)
                },
                RawRepresentation = codeExecutionResult,
                AdditionalProperties = null
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
        EmbedContentResponse response,
        EmbeddingGenerationOptions? options)
    {
        // Currently all models return 768-dimensional embeddings.
        // https://ai.google.dev/gemini-api/docs/models/gemini?#text-embedding
        const int geminiEmbeddingSize = 768;
        var embeddingSize = options?.Dimensions ?? geminiEmbeddingSize;
        var vector = response.Embedding.Values;

        if (vector.Length % embeddingSize != 0)
        {
            throw new InvalidOperationException(
                $"The returned embedding vector's size is not a multiple of the expected dimensions '{embeddingSize}'.");
        }

        var generatedEmbeddings = new GeneratedEmbeddings<Embedding<float>>(vector.Length / embeddingSize);

        for (int i = 0; i < vector.Length; i += embeddingSize)
        {
            var embedding = new Embedding<float>(vector.Slice(i, embeddingSize));
            generatedEmbeddings.Add(embedding);
        }

        return generatedEmbeddings;
    }
}
