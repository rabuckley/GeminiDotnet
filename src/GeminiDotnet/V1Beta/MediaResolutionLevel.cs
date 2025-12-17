using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

[JsonConverter(typeof(JsonStringEnumConverter<MediaResolutionLevel>))]
public enum MediaResolutionLevel
{
    /// <summary>
    /// Media resolution has not been set.
    /// </summary>
    [JsonStringEnumMemberName("MEDIA_RESOLUTION_UNSPECIFIED")]
    MediaResolutionUnspecified,

    /// <summary>
    /// Media resolution set to low.
    /// </summary>
    [JsonStringEnumMemberName("MEDIA_RESOLUTION_LOW")]
    MediaResolutionLow,

    /// <summary>
    /// Media resolution set to medium.
    /// </summary>
    [JsonStringEnumMemberName("MEDIA_RESOLUTION_MEDIUM")]
    MediaResolutionMedium,

    /// <summary>
    /// Media resolution set to high.
    /// </summary>
    [JsonStringEnumMemberName("MEDIA_RESOLUTION_HIGH")]
    MediaResolutionHigh,

    /// <summary>
    /// Media resolution set to ultra high.
    /// </summary>
    [JsonStringEnumMemberName("MEDIA_RESOLUTION_ULTRA_HIGH")]
    MediaResolutionUltraHigh,
}

