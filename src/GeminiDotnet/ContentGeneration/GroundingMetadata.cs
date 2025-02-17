using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Metadata returned to client when grounding is enabled.
/// </summary>
public sealed record GroundingMetadata
{
    /// <summary>
    /// List of supporting references retrieved from specified grounding source.
    /// </summary>
    [JsonPropertyName("groundingChunks")]
    public required IReadOnlyList<GroundingChunk> GroundingChunks { get; init; }

    /// <summary>
    /// List of grounding support.
    /// </summary>
    [JsonPropertyName("groundingSupports")]
    public required IReadOnlyList<GroundingSupport> GroundingSupports { get; init; }

    /// <summary>
    /// Web search queries for the following-up web search.
    /// </summary>
    [JsonPropertyName("webSearchQueries")]
    public required IReadOnlyList<string> WebSearchQueries { get; init; }

    /// <summary>
    /// Google search entry for the following-up web searches.
    /// </summary>
    [JsonPropertyName("searchEntryPoint")]
    public SearchEntryPoint? SearchEntryPoint { get; init; }

    /// <summary>
    /// Metadata related to retrieval in the grounding flow.
    /// </summary>
    [JsonPropertyName("retrievalMetadata")]
    public required RetrievalMetadata RetrievalMetadata { get; init; }
}
