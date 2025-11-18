using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Request for <c>ImportFile</c> to import a File API file with a <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
/// LINT.IfChange(ImportFileRequest)
/// </summary>
public sealed record ImportFileRequest
{
    /// <summary>
    /// Optional. Config for telling the service how to chunk the file.
    /// If not provided, the service will use default parameters.
    /// </summary>
    [JsonPropertyName("chunkingConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ChunkingConfiguration? ChunkingConfiguration { get; init; }

    /// <summary>
    /// Custom metadata to be associated with the file.
    /// </summary>
    [JsonPropertyName("customMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<CustomMetadata>? CustomMetadata { get; init; }

    /// <summary>
    /// Required. The name of the <see cref="V1Beta.Files.File"/> to import.
    /// Example: <c>files/abc-123</c>
    /// </summary>
    [JsonPropertyName("fileName")]
    public required string FileName { get; init; }
}

