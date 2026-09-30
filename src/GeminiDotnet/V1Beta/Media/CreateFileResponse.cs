using System.Text.Json.Serialization;
using File = GeminiDotnet.V1Beta.Files.File;

namespace GeminiDotnet.V1Beta.Media;

/// <summary>
/// Response for <c>CreateFile</c>.
/// </summary>
public sealed record CreateFileResponse
{
    /// <summary>
    /// Metadata for the created file.
    /// </summary>
    [JsonPropertyName("file")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public File? File { get; init; }
}

