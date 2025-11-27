using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Configuration for retrieving grounding content from a <see cref="V1Beta.Corpora.Corpus"/> or
/// <see cref="V1Beta.FileSearchStores.Document"/> created using the Semantic Retriever API.
/// </summary>
public sealed record SemanticRetrieverConfiguration
{
    /// <summary>
    /// Optional. Maximum number of relevant <c>Chunk</c>s to retrieve.
    /// </summary>
    [JsonPropertyName("maxChunksCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? MaxChunksCount { get; init; }

    /// <summary>
    /// Optional. Filters for selecting <see cref="V1Beta.FileSearchStores.Document"/>s and/or <c>Chunk</c>s from the resource.
    /// </summary>
    [JsonPropertyName("metadataFilters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<MetadataFilter>? MetadataFilters { get; init; }

    /// <summary>
    /// Optional. Minimum relevance score for retrieved relevant <c>Chunk</c>s.
    /// </summary>
    [JsonPropertyName("minimumRelevanceScore")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? MinimumRelevanceScore { get; init; }

    /// <summary>
    /// Required. Query to use for matching <c>Chunk</c>s in the given resource by similarity.
    /// </summary>
    [JsonPropertyName("query")]
    public required Content Query { get; init; }

    /// <summary>
    /// Required. Name of the resource for retrieval. Example: <c>corpora/123</c> or
    /// <c>corpora/123/documents/abc</c>.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; init; }
}

