using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Media resolution for the input media.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<MediaResolution>))]
public enum MediaResolution
{
    /// <summary>
    /// Media resolution has not been set.
    /// </summary>
    [JsonStringEnumMemberName("MEDIA_RESOLUTION_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Media resolution set to low (64 tokens).
    /// </summary>
    [JsonStringEnumMemberName("MEDIA_RESOLUTION_LOW")]
    Low,

    /// <summary>
    /// Media resolution set to medium (256 tokens).
    /// </summary>
    [JsonStringEnumMemberName("MEDIA_RESOLUTION_MEDIUM")]
    Medium,

    /// <summary>
    /// Media resolution set to high (zoomed reframing with 256 tokens).
    /// </summary>
    [JsonStringEnumMemberName("MEDIA_RESOLUTION_HIGH")]
    High,
}
