using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// This resource represents a long-running operation where metadata and response fields are strongly typed.
/// </summary>
public sealed record ImportFileOperation : BaseOperation
{
    /// <summary>
    /// Metadata for LongRunning ImportFile Operations.
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImportFileMetadata? Metadata { get; init; }

    /// <summary>
    /// Response for <c>ImportFile</c> to import a File API file with a <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    [JsonPropertyName("response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImportFileResponse? Response { get; init; }
}

