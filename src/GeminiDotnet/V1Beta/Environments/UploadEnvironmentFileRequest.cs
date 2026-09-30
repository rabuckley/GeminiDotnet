using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Request for <c>UploadEnvironmentFile</c>.
/// </summary>
public sealed record UploadEnvironmentFileRequest
{
    /// <summary>
    /// Optional. If true, treats the uploaded file as a tar/tar.gz archive and
    /// unpacks it into <c>path</c>.
    /// </summary>
    [JsonPropertyName("extract")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? Extract { get; init; }

    /// <summary>
    /// Optional. Whether to overwrite the destination file if it already exists.
    /// </summary>
    [JsonPropertyName("overwrite")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? Overwrite { get; init; }
}

