using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// The FileSearch tool that retrieves knowledge from Semantic Retrieval corpora.
/// Files are imported to Semantic Retrieval corpora using the ImportFile API.
/// </summary>
public sealed record FileSearch
{
    /// <summary>
    /// Required. The names of the file_search_stores to retrieve from.
    /// Example: <c>fileSearchStores/my-file-search-store-123</c>
    /// </summary>
    [JsonPropertyName("fileSearchStoreNames")]
    public required IReadOnlyList<string> FileSearchStoreNames { get; init; }

    /// <summary>
    /// Optional. Metadata filter to apply to the semantic retrieval documents and chunks.
    /// </summary>
    [JsonPropertyName("metadataFilter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MetadataFilter { get; init; }

    /// <summary>
    /// Optional. The number of semantic retrieval chunks to retrieve.
    /// </summary>
    [JsonPropertyName("topK")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TopK { get; init; }
}

