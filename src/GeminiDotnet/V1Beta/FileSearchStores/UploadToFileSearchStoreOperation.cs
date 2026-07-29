using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// This resource represents a long-running operation where metadata and response fields are strongly typed.
/// </summary>
public sealed record UploadToFileSearchStoreOperation : BaseOperation
{
    /// <summary>
    /// Metadata for LongRunning UploadToFileSearchStore Operations.
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public UploadToFileSearchStoreMetadata? Metadata { get; init; }

    /// <summary>
    /// Response from UploadToFileSearchStore.
    /// </summary>
    [JsonPropertyName("response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public UploadToFileSearchStoreResponse? Response { get; init; }
}

