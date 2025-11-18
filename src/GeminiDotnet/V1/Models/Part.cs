using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Models;

/// <summary>
/// A datatype containing media that is part of a multi-part <see cref="V1.Models.Content"/> message.
/// A <see cref="V1.Models.Part"/> consists of data which has an associated datatype. A <see cref="V1.Models.Part"/> can only
/// contain one of the accepted types in <c>Part.data</c>.
/// A <see cref="V1.Models.Part"/> must have a fixed IANA MIME type identifying the type and subtype
/// of the media if the <c>inline_data</c> field is filled with raw bytes.
/// </summary>
public sealed record Part
{
    /// <summary>
    /// Inline media bytes.
    /// </summary>
    [JsonPropertyName("inlineData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Blob? InlineData { get; init; }

    /// <summary>
    /// Inline text.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Text { get; init; }

    /// <summary>
    /// Optional. Video metadata. The metadata should only be specified while the video
    /// data is presented in inline_data or file_data.
    /// </summary>
    [JsonPropertyName("videoMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public VideoMetadata? VideoMetadata { get; init; }
}

