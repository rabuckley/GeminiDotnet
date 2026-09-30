using System.Text.Json.Serialization;
using File = GeminiDotnet.V1.Files.File;

namespace GeminiDotnet.V1.Media;

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

