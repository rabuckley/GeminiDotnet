using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Request for <c>UploadToFileSearchStore</c>.
/// </summary>
public sealed record UploadToFileSearchStoreRequest
{
    /// <summary>
    /// Optional. Config for telling the service how to chunk the data.
    /// If not provided, the service will use default parameters.
    /// </summary>
    [JsonPropertyName("chunkingConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ChunkingConfiguration? ChunkingConfiguration { get; init; }

    /// <summary>
    /// Custom metadata to be associated with the data.
    /// </summary>
    [JsonPropertyName("customMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<CustomMetadata>? CustomMetadata { get; init; }

    /// <summary>
    /// Optional. Display name of the created document.
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Optional. MIME type of the data. If not provided, it will be inferred from the
    /// uploaded content.
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }
}

