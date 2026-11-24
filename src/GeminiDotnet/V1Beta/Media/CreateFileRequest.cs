using System.Text.Json.Serialization;
using File = GeminiDotnet.V1Beta.Files.File;

namespace GeminiDotnet.V1Beta.Media;

/// <summary>
/// Request for <c>CreateFile</c>.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record CreateFileRequest
{
    /// <summary>
    /// Optional. Metadata for the file to create.
    /// </summary>
    [JsonPropertyName("file")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public File? File { get; init; }
}

