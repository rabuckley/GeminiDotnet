using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.Safety;

/// <summary>
/// Block at and beyond a specified harm probability.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<HarmBlockThreshold>))]
public enum HarmBlockThreshold
{
    /// <summary>
    /// Threshold is unspecified.
    /// </summary>
    [JsonStringEnumMemberName("HARM_BLOCK_THRESHOLD_UNSPECIFIED")]
    Unspecified,

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
