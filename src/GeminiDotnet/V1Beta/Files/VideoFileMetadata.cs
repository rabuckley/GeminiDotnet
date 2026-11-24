using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Files;

/// <summary>
/// Metadata for a video <see cref="V1Beta.Files.File"/>.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record VideoFileMetadata
{
    /// <summary>
    /// Duration of the video.
    /// </summary>
    [JsonPropertyName("videoDuration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? VideoDuration { get; init; }
}

