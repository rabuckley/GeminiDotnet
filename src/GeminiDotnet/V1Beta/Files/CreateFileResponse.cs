using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Files;

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

