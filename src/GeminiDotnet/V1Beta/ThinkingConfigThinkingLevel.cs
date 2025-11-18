using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. The level of thoughts tokens that the model should generate.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ThinkingConfigThinkingLevel>))]
public enum ThinkingConfigThinkingLevel
{
    /// <summary>
    /// Default value.
    /// </summary>
    [JsonStringEnumMemberName("THINKING_LEVEL_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Low thinking level.
    /// </summary>
    [JsonStringEnumMemberName("LOW")]
    Low,

    /// <summary>
    /// High thinking level.
    /// </summary>
    [JsonStringEnumMemberName("HIGH")]
    High,
}

