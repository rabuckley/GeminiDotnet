using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// Optional. Determines how likely detected speech is ended.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<AutomaticActivityDetectionEndOfSpeechSensitivity>))]
public enum AutomaticActivityDetectionEndOfSpeechSensitivity
{
    /// <summary>
    /// The default is END_SENSITIVITY_HIGH.
    /// </summary>
    [JsonStringEnumMemberName("END_SENSITIVITY_UNSPECIFIED")]
    EndSensitivityUnspecified,

    /// <summary>
    /// Automatic detection ends speech more often.
    /// </summary>
    [JsonStringEnumMemberName("END_SENSITIVITY_HIGH")]
    EndSensitivityHigh,

    /// <summary>
    /// Automatic detection ends speech less often.
    /// </summary>
    [JsonStringEnumMemberName("END_SENSITIVITY_LOW")]
    EndSensitivityLow,
}

