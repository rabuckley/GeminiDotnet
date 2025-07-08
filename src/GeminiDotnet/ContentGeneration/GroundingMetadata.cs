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
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<GroundingChunk>? GroundingChunks { get; init; }

    /// <summary>
    /// List of grounding support.
    /// </summary>
    [JsonPropertyName("groundingSupports")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<GroundingSupport>? GroundingSupports { get; init; }

    /// <summary>
    /// Web search queries for the following-up web search.
    /// </summary>
    [JsonPropertyName("webSearchQueries")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? WebSearchQueries { get; init; }

    /// <summary>
    /// Google search entry for the following-up web searches.
    /// </summary>
    [JsonPropertyName("searchEntryPoint")]
    public SearchEntryPoint? SearchEntryPoint { get; init; }

    /// <summary>
    /// Metadata related to retrieval in the grounding flow.
    /// </summary>
    [JsonPropertyName("retrievalMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RetrievalMetadata? RetrievalMetadata { get; init; }
}
