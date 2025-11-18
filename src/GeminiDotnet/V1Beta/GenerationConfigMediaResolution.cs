using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. If specified, the media resolution specified will be used.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<GenerationConfigMediaResolution>))]
public enum GenerationConfigMediaResolution
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

