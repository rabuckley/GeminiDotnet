using GeminiDotnet.Extensions.AI.Contents;
using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.Models;
using Microsoft.Extensions.AI;
using System.Diagnostics;
using System.Text.Json;

namespace GeminiDotnet.Extensions.AI;

internal static class GeminiToMEAIMapper
{
    public static ChatResponseUpdate CreateMappedChatResponseUpdate(
        GenerateContentResponse response,
        DateTimeOffset createdAt)
    {
        var candidate = response.Candidates is { Count: > 0 } c ? c[0] : null;

        // Map content parts
        var contents = CreateMappedContents(candidate?.Content?.Parts) ?? [];

        // Add UsageContent for streaming aggregation (consumed by ToChatResponse())
        if (CreateMappedUsageDetails(response.UsageMetadata) is { } usageDetails)
        {
            contents.Add(new UsageContent(usageDetails));
        }

        return new ChatResponseUpdate
        {
            AuthorName = null,
            // Streaming responses always come from the model, so default to
            // Assistant when the role is absent rather than falling through to
            // CreateMappedChatRole's default of System.
            Role = candidate?.Content?.Role is { } role
                ? CreateMappedChatRole(role)
                : ChatRole.Assistant,
            Contents = contents,
            RawRepresentation = response,
            AdditionalProperties = null,
            ResponseId = response.ResponseId,
            MessageId = response.ResponseId,
            ConversationId = null,
            CreatedAt = createdAt,
            FinishReason = CreateMappedChatFinishReason(candidate?.FinishReason),
            ModelId = response.ModelVersion
        };
    }

    private static ChatFinishReason? CreateMappedChatFinishReason(CandidateFinishReason? finishReason)
    {
        return finishReason switch
        {
            CandidateFinishReason.Unspecified => throw new ArgumentOutOfRangeException(
                nameof(finishReason),
                finishReason,
                "Unspecified is not a valid finish reason."),
            CandidateFinishReason.Stop => ChatFinishReason.Stop,
            CandidateFinishReason.MaxTokens => ChatFinishReason.Length,
            CandidateFinishReason.Safety => ChatFinishReason.ContentFilter,
            CandidateFinishReason.Recitation => ChatFinishReason.ContentFilter,
            CandidateFinishReason.Language => ChatFinishReason.ContentFilter,
            CandidateFinishReason.Other => ChatFinishReason.ContentFilter,
            CandidateFinishReason.Blocklist => ChatFinishReason.ContentFilter,
            CandidateFinishReason.ProhibitedContent => ChatFinishReason.ContentFilter,
            CandidateFinishReason.Spii => ChatFinishReason.ContentFilter,
            CandidateFinishReason.MalformedFunctionCall => null,
            CandidateFinishReason.ImageSafety => ChatFinishReason.ContentFilter,
            CandidateFinishReason.UnexpectedToolCall => null,
            CandidateFinishReason.TooManyToolCalls => null,
            _ => null
        };
    }

    private static List<AIContent>? CreateMappedContents(IReadOnlyList<Part>? parts)
    {
        if (parts is null)
        {
            return null;
        }

        List<AIContent> contents = new(parts.Count);

        foreach (var part in parts)
        {
            // Each Part should have exactly one property set. Using else-if makes
            // the mutual exclusivity explicit and prevents silent overwrites if a Part
            // ever has multiple properties populated.
            AIContent mapped;

            if (part.Text is not null)
            {
                mapped = CreateMappedTextContent(part);
            }
            else if (part.InlineData is not null)
            {
                mapped = CreateMappedDataContent(part);
            }
            else if (part.FunctionCall is not null)
            {
                mapped = CreateMappedFunctionCallContent(part);
            }
            else if (part.FunctionResponse is not null)
            {
                mapped = CreateMappedFunctionResultContent(part);
            }
            else if (part.FileData is not null)
            {
                mapped = CreateMappedFileDataContent(part);
            }
            else if (part.ExecutableCode is not null)
            {
                mapped = CreateMappedExecutableCodeContent(part);
            }
            else if (part.CodeExecutionResult is not null)
            {
                mapped = CreateMappedCodeExecutionResultContent(part);
            }
            else
            {
                throw new UnreachableException($"All properties of {nameof(Part)} are null.");
            }

            contents.Add(mapped);
        }

        return contents;

        static DataContent CreateMappedDataContent(Part part)
        {
            Debug.Assert(part.InlineData is not null);

            var inlineData = part.InlineData!;

            return new DataContent(inlineData.Data, inlineData.MimeType!) // Let M.E.AI throw.
            {
                RawRepresentation = part,
                AdditionalProperties = null
            };
        }

        static HostedFileContent CreateMappedFileDataContent(Part part)
        {
            Debug.Assert(part.FileData is not null);

            var fileData = part.FileData!;

            return new HostedFileContent(fileData.FileUri)
            {
                MediaType = fileData.MimeType,
                RawRepresentation = part,
                AdditionalProperties = null,
            };
        }

        static AIContent CreateMappedTextContent(Part part)
        {
            if (part.Thought is true)
            {
                return new TextReasoningContent(part.Text)
                {
                    Annotations = null,
                    RawRepresentation = part,
                    AdditionalProperties = null,
                    ProtectedData = part.ThoughtSignature,
                };
            }

            return new TextContent(part.Text)
            {
                Annotations = null,
                RawRepresentation = part,
                AdditionalProperties = null
            };
        }

        static FunctionCallContent CreateMappedFunctionCallContent(Part part)
        {
            Debug.Assert(part.FunctionCall is not null);

            var functionCall = part.FunctionCall!;

            var callId = functionCall.Id ?? $"{functionCall.Name}/{Guid.NewGuid()}";

            var args = functionCall.Arguments.Deserialize(JsonContext.Default.IDictionaryStringObject)
                ?? new Dictionary<string, object?>();

            return new FunctionCallContent(callId, functionCall.Name, args)
            {
                Annotations = null,
                RawRepresentation = part,
                AdditionalProperties = null,
                Exception = null,
                // When the part is a thought, the model is reasoning about calling a
                // function rather than requesting it. Mark it as informational only.
                InformationalOnly = part.Thought is true,
            };
        }

        static FunctionResultContent CreateMappedFunctionResultContent(Part part)
        {
            Debug.Assert(part.FunctionResponse is not null);

            var functionResponse = part.FunctionResponse!;

            var responseId = functionResponse.Id ?? $"{functionResponse.Name}/{Guid.NewGuid()}";

            var result = functionResponse.Response.Deserialize(JsonContext.Default.Object);

            return new FunctionResultContent(responseId, result)
            {
                Annotations = null,
                RawRepresentation = part,
                AdditionalProperties = null,
                Exception = null
            };
        }

        static ExecutableCodeContent CreateMappedExecutableCodeContent(Part part)
        {
            Debug.Assert(part.ExecutableCode is not null);

            var executableCode = part.ExecutableCode!;

            return new ExecutableCodeContent
            {
                Annotations = null,
                Language = executableCode.Language,
                Code = executableCode.Code,
                RawRepresentation = part,
                AdditionalProperties = null
            };
        }

        static CodeExecutionContent CreateMappedCodeExecutionResultContent(Part part)
        {
            Debug.Assert(part.CodeExecutionResult is not null);

            var codeExecutionResult = part.CodeExecutionResult!;

            return new CodeExecutionContent
            {
                Annotations = null,
                Output = codeExecutionResult.Output,
                Status = codeExecutionResult.Outcome switch
                {
                    CodeExecutionResultOutcome.Unspecified => CodeExecutionStatus.None,
                    CodeExecutionResultOutcome.Ok => CodeExecutionStatus.Success,
                    CodeExecutionResultOutcome.Failed => CodeExecutionStatus.Error,
                    CodeExecutionResultOutcome.DeadlineExceeded => CodeExecutionStatus.Timeout,
                    _ => throw new ArgumentOutOfRangeException(nameof(codeExecutionResult.Outcome),
                        codeExecutionResult.Outcome,
                        null)
                },
                RawRepresentation = part,
                AdditionalProperties = null
            };
        }
    }

    public static ChatResponse CreateMappedChatResponse(GenerateContentResponse response, DateTimeOffset createdAt)
    {
        var choices = response.Candidates?.Select(CreateMappedChatMessage).ToList();

        return new ChatResponse(choices)
        {
            ResponseId = response.ResponseId,
            ConversationId = null,
            ModelId = response.ModelVersion,
            CreatedAt = createdAt,
            FinishReason = CreateMappedChatFinishReason(
                response.Candidates is { Count: > 0 } candidates ? candidates[0].FinishReason : null),
            Usage = CreateMappedUsageDetails(response.UsageMetadata),
            RawRepresentation = response,
            AdditionalProperties = null
        };

        static ChatMessage CreateMappedChatMessage(Candidate candidateResponse)
        {
            return new ChatMessage
            {
                AuthorName = null,
                CreatedAt = null,
                Role = CreateMappedChatRole(candidateResponse.Content?.Role),
                Contents = CreateMappedContents(candidateResponse.Content?.Parts) ?? [],
                MessageId = null,
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

    private static UsageDetails? CreateMappedUsageDetails(UsageMetadata? usage)
    {
        if (usage is null)
        {
            return null;
        }

        // Per M.E.AI convention (UsageDetails.cs remarks), ReasoningTokenCount should be
        // counted as part of OutputTokenCount. We include it in both places so that:
        // 1. OutputTokenCount reflects the total billable output tokens (for telemetry/cost)
        // 2. ReasoningTokenCount remains available for detailed breakdown reporting
        return new UsageDetails
        {
            InputTokenCount = usage.PromptTokenCount,
            OutputTokenCount = usage.CandidatesTokenCount is not null || usage.ThoughtsTokenCount is not null
                ? (usage.CandidatesTokenCount ?? 0) + (usage.ThoughtsTokenCount ?? 0)
                : null,
            TotalTokenCount = usage.TotalTokenCount,
            CachedInputTokenCount = usage.CachedContentTokenCount,
            ReasoningTokenCount = usage.ThoughtsTokenCount,
            AdditionalCounts = usage.ToolUsePromptTokenCount is { } toolTokens
                ? new() { [GeminiAdditionalCounts.ToolUsePromptTokenCount] = toolTokens }
                : null,
        };
    }

    public static GeneratedEmbeddings<Embedding<float>> CreateMappedGeneratedEmbeddings(
        EmbedContentResponse response,
        string modelId,
        DateTimeOffset createdAt)
    {
        GeneratedEmbeddings<Embedding<float>> result = [];

        if (response.Embedding?.Values is { } embeddingValues)
        {
            var embedding = new Embedding<float>(embeddingValues)
            {
                CreatedAt = createdAt,
                ModelId = modelId,
                AdditionalProperties = null,
            };

            result.Add(embedding);
        }

        return result;
    }

    /// <summary>
    /// Maps a <see cref="BatchEmbedContentsResponse"/> to <see cref="GeneratedEmbeddings{TEmbedding}"/>,
    /// preserving one embedding per input string in the original order.
    /// </summary>
    /// <param name="response">The batch embedding response from the Gemini API.</param>
    /// <param name="modelId">The model identifier.</param>
    /// <param name="createdAt">The timestamp for the embeddings.</param>
    /// <returns>A collection of embeddings matching the input order.</returns>
    public static GeneratedEmbeddings<Embedding<float>> CreateMappedGeneratedEmbeddings(
        BatchEmbedContentsResponse response,
        string modelId,
        DateTimeOffset createdAt)
    {
        GeneratedEmbeddings<Embedding<float>> result = [];

        if (response.Embeddings is { } embeddings)
        {
            foreach (var contentEmbedding in embeddings)
            {
                // Always add an embedding per batch entry to preserve 1:1 correspondence
                // with input strings. When Values is default (empty), this produces a
                // zero-length embedding rather than skipping the entry, so that result[i]
                // always maps to input[i].
                var embedding = new Embedding<float>(contentEmbedding.Values)
                {
                    CreatedAt = createdAt,
                    ModelId = modelId,
                    AdditionalProperties = null,
                };

                result.Add(embedding);
            }
        }

        return result;
    }
}
