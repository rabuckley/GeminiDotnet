using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Response for <c>ImportFile</c> to import a File API file with a <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
/// </summary>
public sealed record ImportFileResponse
{
    /// <summary>
    /// Immutable. Identifier. The identifier for the <see cref="V1Beta.Document"/> imported.
    /// Example:
    /// <c>fileSearchStores/my-file-search-store-123/documents/my-awesome-doc-123a456b789c</c>
    /// </summary>
    [JsonPropertyName("documentName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DocumentName { get; init; }

    /// <summary>
    /// The name of the <see cref="V1Beta.FileSearchStores.FileSearchStore"/> containing <see cref="V1Beta.Document"/>s.
    /// Example: <c>fileSearchStores/my-file-search-store-123</c>
    /// </summary>
    [JsonPropertyName("parent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Parent { get; init; }
}

