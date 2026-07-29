using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// Optional. Determines how likely speech is to be detected.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<AutomaticActivityDetectionStartOfSpeechSensitivity>))]
public enum AutomaticActivityDetectionStartOfSpeechSensitivity
{
    /// <summary>
    /// The default is START_SENSITIVITY_HIGH.
    /// </summary>
    [JsonStringEnumMemberName("START_SENSITIVITY_UNSPECIFIED")]
    StartSensitivityUnspecified,

    /// <summary>
    /// Automatic detection will detect the start of speech more often.
    /// </summary>
    [JsonStringEnumMemberName("START_SENSITIVITY_HIGH")]
    StartSensitivityHigh,

    /// <summary>
    /// Automatic detection will detect the start of speech less often.
    /// </summary>
    [JsonStringEnumMemberName("START_SENSITIVITY_LOW")]
    StartSensitivityLow,
}

