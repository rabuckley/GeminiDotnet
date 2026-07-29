using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Files;

/// <summary>
/// Metadata for a video <see cref="V1.Files.File"/>.
/// </summary>
public sealed record VideoFileMetadata
{
    /// <summary>
    /// Duration of the video.
    /// </summary>
    [JsonPropertyName("videoDuration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? VideoDuration { get; init; }
}

