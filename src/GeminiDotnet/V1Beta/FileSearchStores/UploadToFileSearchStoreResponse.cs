using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Response from UploadToFileSearchStore.
/// </summary>
public sealed record UploadToFileSearchStoreResponse
{
    /// <summary>
    /// Immutable. Identifier. The identifier for the <see cref="V1Beta.FileSearchStores.Document"/> imported.
    /// Example: <c>fileSearchStores/my-file-search-store-123a456b789c</c>
    /// </summary>
    [JsonPropertyName("documentName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DocumentName { get; init; }

    /// <summary>
    /// MIME type of the file.
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }

    /// <summary>
    /// The name of the <see cref="V1Beta.FileSearchStores.FileSearchStore"/> containing <see cref="V1Beta.FileSearchStores.Document"/>s.
    /// Example: <c>fileSearchStores/my-file-search-store-123</c>
    /// </summary>
    [JsonPropertyName("parent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Parent { get; init; }

    /// <summary>
    /// Size of the file in bytes.
    /// </summary>
    [JsonPropertyName("sizeBytes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? SizeBytes { get; init; }
}

