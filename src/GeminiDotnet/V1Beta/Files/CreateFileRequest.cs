using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Files;

/// <summary>
/// Request for <c>CreateFile</c>.
/// </summary>
public sealed record CreateFileRequest
{
    /// <summary>
    /// Optional. Metadata for the file to create.
    /// </summary>
    [JsonPropertyName("file")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public File? File { get; init; }
}

