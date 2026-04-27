using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Chunk from context retrieved by the file search tool.
/// </summary>
public sealed record RetrievedContext
{
    /// <summary>
    /// Optional. User-provided metadata about the retrieved context.
    /// </summary>
    [JsonPropertyName("customMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<GroundingChunkCustomMetadata>? CustomMetadata { get; init; }

    /// <summary>
    /// Optional. Name of the <see cref="V1Beta.FileSearchStores.FileSearchStore"/> containing the document.
    /// Example: <c>fileSearchStores/123</c>
    /// </summary>
    [JsonPropertyName("fileSearchStore")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FileSearchStore { get; init; }

    /// <summary>
    /// Optional. The media blob resource name for multimodal file search results.
    /// Format: fileSearchStores/{file_search_store_id}/media/{blob_id}
    /// </summary>
    [JsonPropertyName("mediaId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MediaId { get; init; }

    /// <summary>
    /// Optional. Page number of the retrieved context, if applicable.
    /// </summary>
    [JsonPropertyName("pageNumber")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? PageNumber { get; init; }

    /// <summary>
    /// Optional. Text of the chunk.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Text { get; init; }

    /// <summary>
    /// Optional. Title of the document.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Title { get; init; }

    /// <summary>
    /// Optional. URI reference of the semantic retrieval document.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Uri { get; init; }
}

