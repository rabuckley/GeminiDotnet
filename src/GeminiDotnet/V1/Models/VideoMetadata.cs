using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Models;

/// <summary>
/// Deprecated: Use <c>GenerateContentRequest.processing_options</c> instead. Metadata
/// describes the input video content.
/// </summary>
[Obsolete]
public sealed record VideoMetadata
{
    /// <summary>
    /// Optional. The end offset of the video.
    /// </summary>
    [JsonPropertyName("endOffset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? EndOffset { get; init; }

    /// <summary>
    /// Optional. The frame rate of the video sent to the model. If not specified, the
    /// default value will be 1.0.
    /// The fps range is (0.0, 24.0].
    /// </summary>
    [JsonPropertyName("fps")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double? Fps { get; init; }

    /// <summary>
    /// Optional. The start offset of the video.
    /// </summary>
    [JsonPropertyName("startOffset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? StartOffset { get; init; }
}

