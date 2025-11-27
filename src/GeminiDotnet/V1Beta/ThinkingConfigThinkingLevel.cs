using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. Controls the maximum depth of the model's internal reasoning process before
/// it produces a response. If not specified, the default is HIGH. Recommended
/// for Gemini 3 or later models. Use with earlier models results in an error.
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

