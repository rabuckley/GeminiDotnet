using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Media;

/// <summary>
/// This resource represents a long-running operation where metadata and response fields are strongly typed.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
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

