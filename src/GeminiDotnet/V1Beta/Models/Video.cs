using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Representation of a video.
/// </summary>
public sealed record Video
{
    /// <summary>
    /// Path to another storage.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Uri { get; init; }

    /// <summary>
    /// Raw bytes.
    /// </summary>
    [JsonPropertyName("video")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? VideoValue { get; init; }
}

