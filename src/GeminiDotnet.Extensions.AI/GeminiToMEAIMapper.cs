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
    /// <param name="state">
    /// The state carried across the updates of one stream. Callers must pass the same instance for every
    /// update, so that a call and the result answering it correlate and a grounding segment resolves
    /// against the text the whole stream has produced.
    /// </param>
    public static ChatResponseUpdate CreateMappedChatResponseUpdate(
        GenerateContentResponse response,
        CandidateMappingState state,
        DateTimeOffset createdAt)
    {
        var candidate = response.Candidates is { Count: > 0 } c ? c[0] : null;

        // Map content parts
        var contents = CreateMappedContents(candidate?.Content?.Parts, state) ?? [];

        // A streamed segment's offsets index every non-thought text part of the stream, not just this
        // update's, so the text has to be accumulated before the grounding metadata that arrives with the
        // final chunk can be resolved against it. TextReasoningContent is deliberately not counted.
        foreach (var content in contents)
        {
            if (content is TextContent text)
            {
                state.Text.Append(text.Text);
            }
        }

        if (candidate?.GroundingMetadata is { } groundingMetadata)
        {
            AppendMappedGroundingMetadata(
                contents,
                groundingMetadata,
                state,
                CitationTarget.ForStream(contents, state.Text.ToString()));
        }

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
    private static List<AIContent>? CreateMappedContents(IReadOnlyList<Part>? parts, CandidateMappingState state)
    {
        if (parts is null)
        {
            return null;
        }

        List<AIContent> contents = new(parts.Count);

        // Gemini emits an ExecutableCode followed by the CodeExecutionResult that answers it, and a
        // server-side ToolCall followed by its ToolResponse. The state carries the unanswered calls
        // because streaming splits a call and its result across chunks.
        var unansweredCodeExecutionCallIds = state.UnansweredCodeExecutionCallIds;
        var unansweredToolCallIds = state.UnansweredToolCallIds;

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

                if (part.ToolCall.ToolType is ToolType.GoogleSearchWeb)
                {
                    state.HasReportedWebSearch = true;
                    mapped = CreateMappedWebSearchToolCallContent(part, callId);
                }
                else
                {
                    mapped = CreateMappedToolCallContent(part, callId);
                }
            }
            else if (part.ToolResponse is not null)
            {
                unansweredToolCallIds.TryDequeue(out var unansweredCallId);

                var callId = part.ToolResponse.Id
                    ?? unansweredCallId
                    ?? $"{part.ToolResponse.ToolType}/{Guid.NewGuid()}";

                mapped = part.ToolResponse.ToolType is ToolType.GoogleSearchWeb
                    ? CreateMappedWebSearchToolResultContent(part, callId)
                    : CreateMappedToolResultContent(part, callId);
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
                AdditionalProperties = CreateMappedAdditionalProperties(
                [
                    new(GeminiContentProperties.ThoughtSignature, part.ThoughtSignature),
                ]),
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

            return new ToolCallContent(callId)
            {
                Annotations = null,
                RawRepresentation = part,
                AdditionalProperties = CreateToolCallProperties(part.ToolCall!, part.ThoughtSignature),
            };
        }

        static ToolResultContent CreateMappedToolResultContent(Part part, string callId)
        {
            Debug.Assert(part.ToolResponse is not null);

            return new ToolResultContent(callId)
            {
                Annotations = null,
                RawRepresentation = part,
                AdditionalProperties = CreateToolResponseProperties(part.ToolResponse!, part.ThoughtSignature),
            };
        }

        static WebSearchToolCallContent CreateMappedWebSearchToolCallContent(Part part, string callId)
        {
            Debug.Assert(part.ToolCall is not null);

            var toolCall = part.ToolCall!;

            return new WebSearchToolCallContent(callId)
            {
                Queries = ReadQueries(toolCall.Arguments),
                Annotations = null,
                RawRepresentation = part,
                AdditionalProperties = CreateToolCallProperties(toolCall, part.ThoughtSignature),
            };
        }

        static WebSearchToolResultContent CreateMappedWebSearchToolResultContent(Part part, string callId)
        {
            Debug.Assert(part.ToolResponse is not null);

            return new WebSearchToolResultContent(callId)
            {
                // The sources live on the citation annotations, and the raw response is kept on the
                // AdditionalProperties, so the result carries no outputs of its own.
                Outputs = null,
                Annotations = null,
                RawRepresentation = part,
                AdditionalProperties = CreateToolResponseProperties(part.ToolResponse!, part.ThoughtSignature),
            };
        }

        static AdditionalPropertiesDictionary? CreateToolCallProperties(ToolCall toolCall, string? thoughtSignature)
        {
            return CreateMappedAdditionalProperties(
            [
                new(GeminiContentProperties.Id, toolCall.Id),
                new(GeminiContentProperties.ToolType, toolCall.ToolType),
                new(GeminiContentProperties.ToolName, toolCall.ToolName),
                new(GeminiContentProperties.Arguments, DefinedOrNull(toolCall.Arguments)),
                new(GeminiContentProperties.ThoughtSignature, thoughtSignature),
            ]);
        }

        static AdditionalPropertiesDictionary? CreateToolResponseProperties(
            ToolResponse toolResponse,
            string? thoughtSignature)
        {
            return CreateMappedAdditionalProperties(
            [
                new(GeminiContentProperties.Id, toolResponse.Id),
                new(GeminiContentProperties.ToolType, toolResponse.ToolType),
                new(GeminiContentProperties.Response, DefinedOrNull(toolResponse.Response)),
                new(GeminiContentProperties.ThoughtSignature, thoughtSignature),
            ]);
        }

        // The strings of the invocation's queries argument, or null when Gemini reported no such array of
        // strings.
        static List<string>? ReadQueries(JsonElement arguments)
        {
            if (arguments.ValueKind is not JsonValueKind.Object
                || !arguments.TryGetProperty("queries", out var queries)
                || queries.ValueKind is not JsonValueKind.Array)
            {
                return null;
            }

            var mapped = new List<string>(queries.GetArrayLength());

            foreach (var query in queries.EnumerateArray())
            {
                if (query.ValueKind is not JsonValueKind.String)
                {
                    return null;
                }

                mapped.Add(query.GetString()!);
            }

            return mapped;
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
    /// <para>
    /// A pair is synthesized from <see cref="GroundingMetadata.WebSearchQueries"/> only when no web search
    /// has been reported for the candidate yet. A <c>GOOGLE_SEARCH_WEB</c> invocation is the complete
    /// record of a search, and the queries are cumulative across every search of the turn and not in
    /// invocation order, so they cannot be attributed to one call; once any search has been reported, by
    /// an invocation or by an earlier grounding delivery, a later delivery adds nothing.
    /// </para>
    /// </remarks>
    /// <param name="target">
    /// How this candidate's segments resolve to the text they index. The whole-response and streamed forms
    /// differ, because a streamed segment spans the whole stream rather than one part.
    /// </param>
    private static void AppendMappedGroundingMetadata(
        List<AIContent> contents,
        GroundingMetadata groundingMetadata,
        CandidateMappingState state,
        CitationTarget target)
    {
        if (groundingMetadata.GroundingChunks is { Count: > 0 } chunks)
        {
            AttachMappedCitationAnnotations(chunks, groundingMetadata.GroundingSupports, target);
        }

        if (groundingMetadata.WebSearchQueries is not { Count: > 0 } queries || state.HasReportedWebSearch)
        {
            return;
        }

        state.HasReportedWebSearch = true;

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
    /// The content a <see cref="Segment"/>'s offsets index, and the content its citation is attached to.
    /// </summary>
    private readonly record struct SegmentTarget(TextContent Content, string IndexedText);

    /// <summary>
    /// How a candidate's grounding segments resolve to the text they index, and where the citations for
    /// the chunks no support references are attached.
    /// </summary>
    /// <param name="Resolve">
    /// Returns the content to annotate and the text the segment's UTF-8 byte offsets index, or
    /// <see langword="null"/> when the segment names text this mapper cannot place.
    /// </param>
    /// <param name="UncitedCarrier">
    /// Returns the content that carries the region-less citations, creating it if it does not exist yet.
    /// </param>
    private readonly record struct CitationTarget(
        Func<Segment, SegmentTarget?> Resolve,
        Func<TextContent> UncitedCarrier)
    {
        /// <summary>
        /// A whole response: <see cref="Segment.PartIndex"/> selects the content the mapper produced for
        /// that part, and the offsets index that content's own text, as the Gemini spec defines them.
        /// </summary>
        public static CitationTarget ForResponse(List<AIContent> contents)
        {
            TextContent? carrier = null;

            return new(Resolve, UncitedCarrier);

            SegmentTarget? Resolve(Segment segment)
            {
                var contentIndex = segment.PartIndex ?? 0;

                // Gemini can name a part this mapper produced no text for (a thought, a function call),
                // and nothing stops it naming one that does not exist. Neither can carry a region.
                return (uint)contentIndex < (uint)contents.Count && contents[contentIndex] is TextContent text
                    ? new SegmentTarget(text, text.Text)
                    : null;
            }

            TextContent UncitedCarrier()
            {
                return carrier ??= contents.OfType<TextContent>().FirstOrDefault() ?? AppendCarrier(contents);
            }
        }

        /// <summary>
        /// A streamed update: the offsets index every non-thought text part of the stream so far, so the
        /// annotations go on one empty carrier rather than on the fragment they happen to overlap.
        /// </summary>
        /// <remarks>
        /// A carrier rather than the update's own text, because the regions index the whole stream and an
        /// annotated fragment would also stop coalescing. A support that names a part is unresolvable:
        /// the spec defines the offsets as part-relative, so resolving them against the joined text would
        /// produce a region the caller could not tell was wrong. Proto3 JSON omits a zero-valued field, so
        /// this only catches a part index of one or more; a streamed segment has never carried either. A
        /// segment ending past the text streamed so far resolves to no region, which only a grounding
        /// delivery before the final chunk could produce.
        /// </remarks>
        public static CitationTarget ForStream(List<AIContent> contents, string streamText)
        {
            TextContent? carrier = null;

            return new(Resolve, UncitedCarrier);

            SegmentTarget? Resolve(Segment segment)
            {
                return segment.PartIndex is null ? new SegmentTarget(UncitedCarrier(), streamText) : null;
            }

            TextContent UncitedCarrier() => carrier ??= AppendCarrier(contents);
        }

        /// <summary>
        /// Appends the empty <see cref="TextContent"/> that carries citations with nowhere else to go.
        /// </summary>
        /// <remarks>
        /// A candidate can be grounded without a text part of its own, and
        /// <see cref="MEAIToGeminiMapper"/> skips an empty <see cref="TextContent"/>, so the carrier is
        /// safe to feed back as history.
        /// </remarks>
        private static TextContent AppendCarrier(List<AIContent> contents)
        {
            var carrier = new TextContent(string.Empty);
            contents.Add(carrier);
            return carrier;
        }
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
        IReadOnlyList<GroundingChunk> chunks,
        IReadOnlyList<GroundingSupport>? groundingSupports,
        CitationTarget target)
    {
        Dictionary<(TextContent Content, int ChunkIndex), CitationAnnotation> attached = [];
        HashSet<int> referencedChunks = [];

        foreach (var support in groundingSupports ?? [])
        {
            if (support.Segment is not { } segment || target.Resolve(segment) is not { } resolved)
            {
                continue;
            }

            var text = resolved.Content;
            var span = CreateMappedTextSpan(resolved.IndexedText, segment);

            foreach (var chunkIndex in support.GroundingChunkIndices.Span)
            {
                if ((uint)chunkIndex >= (uint)chunks.Count)
                {
                    continue;
                }

                referencedChunks.Add(chunkIndex);

                if (!attached.TryGetValue((text, chunkIndex), out var annotation))
                {
                    if (CreateMappedCitationAnnotation(chunks[chunkIndex]) is not { } created)
                    {
                        continue;
                    }

                    annotation = created;
                    attached[(text, chunkIndex)] = annotation;
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

        var carrier = target.UncitedCarrier();

        foreach (var annotation in unreferenced)
        {
            (carrier.Annotations ??= []).Add(annotation);
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
        // Gemini serializes proto3 JSON, which omits a zero-valued field, so a segment that starts at the
        // beginning of the text arrives without a StartIndex at all. An absent EndIndex is a zero-length
        // span, which describes nothing.
        if (segment.EndIndex is not { } endByte)
        {
            return null;
        }

        var startByte = segment.StartIndex ?? 0;

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
            var state = new CandidateMappingState();
            var contents = CreateMappedContents(candidateResponse.Content?.Parts, state) ?? [];

            if (candidateResponse.GroundingMetadata is { } groundingMetadata)
            {
                AppendMappedGroundingMetadata(
                    contents,
                    groundingMetadata,
                    state,
                    CitationTarget.ForResponse(contents));
            }

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
