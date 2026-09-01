using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.Models;
using Microsoft.Extensions.AI;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

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

        // Streaming responses carry only the grounding chunks not already sent, while
        // GroundingSupport.GroundingChunkIndices index the accumulated list across every response.
        // This mapper is per-update and stateless, so it cannot resolve them; citations for a
        // streamed update therefore carry no regions.
        AppendMappedGroundingMetadata(contents, candidate?.GroundingMetadata, groundingSupports: null);

        // Add UsageContent for streaming aggregation (consumed by ToChatResponse())
        if (CreateMappedUsageDetails(response.UsageMetadata) is { } usageDetails)
        {
            contents.Add(new UsageContent(usageDetails));
        }

        return new ChatResponseUpdate
        {
            AuthorName = null,
            Role = CreateMappedCandidateRole(candidate?.Content?.Role),
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

    /// <summary>
    /// Maps each <see cref="Part"/> to exactly one <see cref="AIContent"/>, in order, so that a
    /// <see cref="Segment.PartIndex"/> also indexes the returned list.
    /// </summary>
    private static List<AIContent>? CreateMappedContents(IReadOnlyList<Part>? parts)
    {
        if (parts is null)
        {
            return null;
        }

        List<AIContent> contents = new(parts.Count);

        // Gemini emits an ExecutableCode followed by the CodeExecutionResult that answers it, and a
        // server-side ToolCall followed by its ToolResponse. Their ids are optional on the wire, and
        // Gemini can run several tools in one turn, so an id-less response is paired with the oldest
        // unanswered call rather than with whichever call came last.
        Queue<string> unansweredCodeExecutionCallIds = [];
        Queue<string> unansweredToolCallIds = [];

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
                var callId = part.ExecutableCode.Id ?? $"code_execution/{Guid.NewGuid()}";
                unansweredCodeExecutionCallIds.Enqueue(callId);
                mapped = CreateMappedCodeInterpreterToolCallContent(part, callId);
            }
            else if (part.CodeExecutionResult is not null)
            {
                unansweredCodeExecutionCallIds.TryDequeue(out var unansweredCallId);

                var callId = part.CodeExecutionResult.Id
                    ?? unansweredCallId
                    ?? $"code_execution/{Guid.NewGuid()}";

                mapped = CreateMappedCodeInterpreterToolResultContent(part, callId);
            }
            else if (part.ToolCall is not null)
            {
                var callId = part.ToolCall.Id ?? $"{part.ToolCall.ToolType}/{Guid.NewGuid()}";
                unansweredToolCallIds.Enqueue(callId);
                mapped = CreateMappedToolCallContent(part, callId);
            }
            else if (part.ToolResponse is not null)
            {
                unansweredToolCallIds.TryDequeue(out var unansweredCallId);

                var callId = part.ToolResponse.Id
                    ?? unansweredCallId
                    ?? $"{part.ToolResponse.ToolType}/{Guid.NewGuid()}";

                mapped = CreateMappedToolResultContent(part, callId);
            }
            else
            {
                mapped = ThrowUnrecognisedPart();
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

        static CodeInterpreterToolCallContent CreateMappedCodeInterpreterToolCallContent(Part part, string callId)
        {
            Debug.Assert(part.ExecutableCode is not null);

            var executableCode = part.ExecutableCode!;

            // Map language to a MIME type for the DataContent input.
            var mediaType = executableCode.Language switch
            {
                ExecutableCodeLanguage.Python => "text/x-python",
                _ => "text/plain",
            };

            var codeBytes = System.Text.Encoding.UTF8.GetBytes(executableCode.Code);

            return new CodeInterpreterToolCallContent(callId)
            {
                Inputs = [new DataContent(codeBytes, mediaType)],
                Annotations = null,
                RawRepresentation = part,
                AdditionalProperties = CreateMappedAdditionalProperties(
                [
                    new(GeminiContentProperties.Id, executableCode.Id),
                    new(GeminiContentProperties.ThoughtSignature, part.ThoughtSignature),
                ]),
            };
        }

        static CodeInterpreterToolResultContent CreateMappedCodeInterpreterToolResultContent(Part part, string callId)
        {
            Debug.Assert(part.CodeExecutionResult is not null);

            var codeExecutionResult = part.CodeExecutionResult!;

            var outputs = new List<AIContent>();
            if (codeExecutionResult.Output is { } output)
            {
                outputs.Add(new TextContent(output));
            }

            // Unspecified is the enum's default, not null, so it has to be dropped by hand for the
            // "present only when Gemini reported one" contract to hold.
            object? outcome = codeExecutionResult.Outcome is CodeExecutionResultOutcome.Unspecified
                ? null
                : codeExecutionResult.Outcome;

            return new CodeInterpreterToolResultContent(callId)
            {
                Outputs = outputs,
                Annotations = null,
                RawRepresentation = part,
                AdditionalProperties = CreateMappedAdditionalProperties(
                [
                    new(GeminiContentProperties.Id, codeExecutionResult.Id),
                    new(GeminiContentProperties.Outcome, outcome),
                    new(GeminiContentProperties.ThoughtSignature, part.ThoughtSignature),
                ]),
            };
        }

        static ToolCallContent CreateMappedToolCallContent(Part part, string callId)
        {
            Debug.Assert(part.ToolCall is not null);

            var toolCall = part.ToolCall!;

            return new ToolCallContent(callId)
            {
                Annotations = null,
                RawRepresentation = part,
                AdditionalProperties = CreateMappedAdditionalProperties(
                [
                    new(GeminiContentProperties.Id, toolCall.Id),
                    new(GeminiContentProperties.ToolType, toolCall.ToolType),
                    new(GeminiContentProperties.ToolName, toolCall.ToolName),
                    new(GeminiContentProperties.Arguments, DefinedOrNull(toolCall.Arguments)),
                    new(GeminiContentProperties.ThoughtSignature, part.ThoughtSignature),
                ]),
            };
        }

        static ToolResultContent CreateMappedToolResultContent(Part part, string callId)
        {
            Debug.Assert(part.ToolResponse is not null);

            var toolResponse = part.ToolResponse!;

            return new ToolResultContent(callId)
            {
                Annotations = null,
                RawRepresentation = part,
                AdditionalProperties = CreateMappedAdditionalProperties(
                [
                    new(GeminiContentProperties.Id, toolResponse.Id),
                    new(GeminiContentProperties.ToolType, toolResponse.ToolType),
                    new(GeminiContentProperties.Response, DefinedOrNull(toolResponse.Response)),
                    new(GeminiContentProperties.ThoughtSignature, part.ThoughtSignature),
                ]),
            };
        }

        static object? DefinedOrNull(JsonElement element)
        {
            return element.ValueKind is JsonValueKind.Undefined ? null : element;
        }

        [DoesNotReturn]
        static AIContent ThrowUnrecognisedPart()
        {
            GeminiMappingException.Throw(
                fromPropertyName: $"{typeof(Part)}",
                toPropertyName: $"{typeof(AIContent)}",
                reason: $"The {nameof(Part)} carries no field this mapper recognises.");

            return null!;
        }
    }

    /// <summary>
    /// Appends the evidence and invocation content that <paramref name="groundingMetadata"/> describes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Every recognised <see cref="GroundingChunk"/> becomes a <see cref="CitationAnnotation"/> on the text
    /// it grounds, whether it came from the web, a file search store, image search or Google Maps, so that
    /// one loop over the annotations reaches every source behind an answer.
    /// </para>
    /// <para>
    /// Gemini reports no invocation signal for file search equivalent to the
    /// <see cref="WebSearchToolCallContent"/> / <see cref="WebSearchToolResultContent"/> pair — there is no
    /// query field, and <see cref="GroundingMetadata.RetrievalMetadata"/> only carries a Google Search
    /// dynamic-retrieval score — so a file search that matched nothing is indistinguishable from no file
    /// search at all.
    /// </para>
    /// </remarks>
    /// <param name="groundingSupports">
    /// The supports locating each chunk within the candidate's parts, or <see langword="null"/> to emit
    /// region-less citations. Streaming callers pass <see langword="null"/>, because chunk indices there are
    /// cumulative across responses and this mapper sees one response at a time.
    /// </param>
    private static void AppendMappedGroundingMetadata(
        List<AIContent> contents,
        GroundingMetadata? groundingMetadata,
        IReadOnlyList<GroundingSupport>? groundingSupports)
    {
        if (groundingMetadata is null)
        {
            return;
        }

        if (groundingMetadata.GroundingChunks is { Count: > 0 } chunks)
        {
            AttachMappedCitationAnnotations(contents, chunks, groundingSupports);
        }

        if (groundingMetadata.WebSearchQueries is not { Count: > 0 } queries)
        {
            return;
        }

        var callId = $"web-search/{Guid.NewGuid()}";

        contents.Add(new WebSearchToolCallContent(callId)
        {
            Queries = [.. queries],
            RawRepresentation = groundingMetadata,
        });

        // The sources live on the citation annotations, so the result carries no outputs of its own.
        contents.Add(new WebSearchToolResultContent(callId)
        {
            Outputs = null,
            RawRepresentation = groundingMetadata,
        });
    }

    /// <summary>
    /// Attaches one <see cref="CitationAnnotation"/> per grounding chunk to the content it grounds.
    /// </summary>
    /// <remarks>
    /// A chunk cited from several spans of the same part yields a single annotation carrying several
    /// regions. A chunk that no support references still gets an annotation, without a region, because the
    /// source is real even when the span it grounds is unknown.
    /// </remarks>
    private static void AttachMappedCitationAnnotations(
        List<AIContent> contents,
        IReadOnlyList<GroundingChunk> chunks,
        IReadOnlyList<GroundingSupport>? groundingSupports)
    {
        Dictionary<(int ContentIndex, int ChunkIndex), CitationAnnotation> attached = [];
        HashSet<int> referencedChunks = [];

        foreach (var support in groundingSupports ?? [])
        {
            if (support.Segment is not { } segment)
            {
                continue;
            }

            var contentIndex = segment.PartIndex ?? 0;

            // Gemini can name a part this mapper produced no text for (a thought, a function call), and
            // nothing stops it naming one that does not exist. Neither can carry a region.
            if ((uint)contentIndex >= (uint)contents.Count || contents[contentIndex] is not TextContent text)
            {
                continue;
            }

            var span = CreateMappedTextSpan(text.Text, segment);

            foreach (var chunkIndex in support.GroundingChunkIndices.Span)
            {
                if ((uint)chunkIndex >= (uint)chunks.Count)
                {
                    continue;
                }

                referencedChunks.Add(chunkIndex);

                if (!attached.TryGetValue((contentIndex, chunkIndex), out var annotation))
                {
                    if (CreateMappedCitationAnnotation(chunks[chunkIndex]) is not { } created)
                    {
                        continue;
                    }

                    annotation = created;
                    attached[(contentIndex, chunkIndex)] = annotation;
                    (text.Annotations ??= []).Add(annotation);
                }

                if (span is { } textSpan)
                {
                    // TextSpanAnnotatedRegion is mutable, so each annotation gets its own instance
                    // rather than an edit on one co-cited annotation reaching all of them.
                    (annotation.AnnotatedRegions ??= []).Add(new TextSpanAnnotatedRegion
                    {
                        StartIndex = textSpan.StartIndex,
                        EndIndex = textSpan.EndIndex,
                    });
                }
            }
        }

        List<CitationAnnotation>? unreferenced = null;

        for (var chunkIndex = 0; chunkIndex < chunks.Count; chunkIndex++)
        {
            if (referencedChunks.Contains(chunkIndex))
            {
                continue;
            }

            if (CreateMappedCitationAnnotation(chunks[chunkIndex]) is { } annotation)
            {
                (unreferenced ??= []).Add(annotation);
            }
        }

        if (unreferenced is null)
        {
            return;
        }

        var target = contents.OfType<TextContent>().FirstOrDefault();

        if (target is null)
        {
            // A candidate can be grounded without a text part of its own. Carry the sources rather than
            // dropping them.
            target = new TextContent(string.Empty);
            contents.Add(target);
        }

        foreach (var annotation in unreferenced)
        {
            (target.Annotations ??= []).Add(annotation);
        }
    }

    /// <summary>
    /// Maps a <see cref="GroundingChunk"/> to a <see cref="CitationAnnotation"/>, or returns
    /// <see langword="null"/> when the chunk carries no variant this mapper recognises.
    /// </summary>
    private static CitationAnnotation? CreateMappedCitationAnnotation(GroundingChunk chunk)
    {
        if (chunk.Web is { } web)
        {
            return new CitationAnnotation
            {
                Title = web.Title,
                Url = CreateMappedUri(web.Uri),
                ToolName = GeminiToolNames.GoogleSearch,
                RawRepresentation = chunk,
            };
        }

        if (chunk.RetrievedContext is { } retrievedContext)
        {
            return new CitationAnnotation
            {
                Title = retrievedContext.Title,
                Url = CreateMappedUri(retrievedContext.Uri),
                // Only the media blob is a file. The store it came from is named separately, because a
                // store name in FileId would leave the consumer unable to tell the two apart.
                FileId = retrievedContext.MediaId,
                ToolName = GeminiToolNames.FileSearch,
                Snippet = retrievedContext.Text,
                RawRepresentation = chunk,
                AdditionalProperties = CreateMappedAdditionalProperties(
                [
                    new(GeminiCitationProperties.PageNumber, retrievedContext.PageNumber),
                    new(GeminiCitationProperties.FileSearchStore, retrievedContext.FileSearchStore),
                    new(
                        GeminiCitationProperties.CustomMetadata,
                        CreateMappedCustomMetadata(retrievedContext.CustomMetadata)),
                ]),
            };
        }

        if (chunk.Image is { } image)
        {
            return new CitationAnnotation
            {
                Title = image.Title,
                Url = CreateMappedUri(image.SourceUri),
                ToolName = GeminiToolNames.GoogleSearch,
                RawRepresentation = chunk,
                AdditionalProperties = CreateMappedAdditionalProperties(
                [
                    new(GeminiCitationProperties.ImageUri, image.ImageUri),
                    new(GeminiCitationProperties.Domain, image.Domain),
                ]),
            };
        }

        if (chunk.Maps is { } maps)
        {
            return new CitationAnnotation
            {
                Title = maps.Title,
                Url = CreateMappedUri(maps.Uri),
                FileId = maps.PlaceId,
                ToolName = GeminiToolNames.GoogleMaps,
                Snippet = maps.Text,
                RawRepresentation = chunk,
            };
        }

        return null;
    }

    /// <summary>
    /// Builds the dictionary carrying the fields the mapped content type has no property for, dropping
    /// the entries with no value.
    /// </summary>
    private static AdditionalPropertiesDictionary? CreateMappedAdditionalProperties(
        ReadOnlySpan<KeyValuePair<string, object?>> entries)
    {
        AdditionalPropertiesDictionary? properties = null;

        foreach (var (key, value) in entries)
        {
            if (value is null)
            {
                continue;
            }

            properties ??= [];
            properties[key] = value;
        }

        return properties;
    }

    private static AdditionalPropertiesDictionary? CreateMappedCustomMetadata(
        IReadOnlyList<GroundingChunkCustomMetadata>? customMetadata)
    {
        AdditionalPropertiesDictionary? properties = null;

        foreach (var entry in customMetadata ?? [])
        {
            if (entry.Key is not { } key || CreateMappedCustomMetadataValue(entry) is not { } value)
            {
                continue;
            }

            properties ??= [];
            properties[key] = value;
        }

        return properties;

        static object? CreateMappedCustomMetadataValue(GroundingChunkCustomMetadata entry)
        {
            if (entry.StringValue is { } stringValue)
            {
                return stringValue;
            }

            if (entry.NumericValue is { } numericValue)
            {
                return numericValue;
            }

            if (entry.StringListValue?.Values is not { } values)
            {
                return null;
            }

            var array = new JsonArray();

            foreach (var value in values)
            {
                array.Add((JsonNode?)JsonValue.Create(value));
            }

            return array;
        }
    }

    private static Uri? CreateMappedUri(string? uri)
    {
        return uri is not null && Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out var result)
            ? result
            : null;
    }

    /// <summary>
    /// Converts a <see cref="Segment"/>'s UTF-8 byte offsets into the UTF-16 character indices that
    /// <see cref="TextSpanAnnotatedRegion"/> uses, or returns <see langword="null"/> when the segment does
    /// not describe a span that lies within <paramref name="text"/>.
    /// </summary>
    private static (int StartIndex, int EndIndex)? CreateMappedTextSpan(string text, Segment segment)
    {
        if (segment.StartIndex is not { } startByte || segment.EndIndex is not { } endByte)
        {
            return null;
        }

        if (startByte < 0 || endByte < startByte || endByte > Encoding.UTF8.GetByteCount(text))
        {
            return null;
        }

        return (CountCharsInUtf8Prefix(text, startByte), CountCharsInUtf8Prefix(text, endByte));

        static int CountCharsInUtf8Prefix(string text, int byteOffset)
        {
            var bytes = 0;
            var chars = 0;

            foreach (var rune in text.EnumerateRunes())
            {
                if (bytes >= byteOffset)
                {
                    break;
                }

                bytes += rune.Utf8SequenceLength;
                chars += rune.Utf16SequenceLength;
            }

            return chars;
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
            var contents = CreateMappedContents(candidateResponse.Content?.Parts) ?? [];

            AppendMappedGroundingMetadata(
                contents,
                candidateResponse.GroundingMetadata,
                candidateResponse.GroundingMetadata?.GroundingSupports);

            return new ChatMessage
            {
                AuthorName = null,
                CreatedAt = null,
                Role = CreateMappedCandidateRole(candidateResponse.Content?.Role),
                Contents = contents,
                MessageId = null,
                RawRepresentation = candidateResponse,
                AdditionalProperties = null
            };
        }
    }

    private static ChatRole CreateMappedCandidateRole(string? role)
    {
        if (role is null)
        {
            return ChatRole.Assistant;
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
