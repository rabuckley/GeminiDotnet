using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Required. Controls the probability threshold at which harm is blocked.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<SafetySettingThreshold>))]
public enum SafetySettingThreshold
{
    /// <summary>
    /// Threshold is unspecified.
    /// </summary>
    [JsonStringEnumMemberName("HARM_BLOCK_THRESHOLD_UNSPECIFIED")]
    HarmBlockThresholdUnspecified,

    /// <summary>
    /// Content with NEGLIGIBLE will be allowed.
    /// </summary>
    [JsonStringEnumMemberName("BLOCK_LOW_AND_ABOVE")]
    BlockLowAndAbove,

    /// <summary>
    /// Content with NEGLIGIBLE and LOW will be allowed.
    /// </summary>
    [JsonStringEnumMemberName("BLOCK_MEDIUM_AND_ABOVE")]
    BlockMediumAndAbove,

    /// <summary>
    /// Content with NEGLIGIBLE, LOW, and MEDIUM will be allowed.
    /// </summary>
    [JsonStringEnumMemberName("BLOCK_ONLY_HIGH")]
    BlockOnlyHigh,

    /// <summary>
    /// All content will be allowed.
    /// </summary>
    [JsonStringEnumMemberName("BLOCK_NONE")]
    BlockNone,

    /// <summary>
    /// Turn off the safety filter.
    /// </summary>
    [JsonStringEnumMemberName("OFF")]
    Off,
}

