using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// Optional. Defines which input is included in the user's turn.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<RealtimeInputConfigTurnCoverage>))]
public enum RealtimeInputConfigTurnCoverage
{
    /// <summary>
    /// If unspecified, a default behavior is selected based on the model. E.g.,
    /// for Gemini 2.5, the default is <c>TURN_INCLUDES_ONLY_ACTIVITY</c>, while for
    /// Gemini 3.1 and onwards, it's
    /// <c>TURN_INCLUDES_AUDIO_ACTIVITY_AND_ALL_VIDEO</c>.
    /// </summary>
    [JsonStringEnumMemberName("TURN_COVERAGE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Includes activity since the last turn, excluding inactivity (e.g. silence
    /// on the audio stream).
    /// </summary>
    [JsonStringEnumMemberName("TURN_INCLUDES_ONLY_ACTIVITY")]
    TurnIncludesOnlyActivity,

    /// <summary>
    /// Includes all realtime input since the last turn, including inactivity
    /// (e.g. silence on the audio stream).
    /// </summary>
    [JsonStringEnumMemberName("TURN_INCLUDES_ALL_INPUT")]
    TurnIncludesAllInput,

    /// <summary>
    /// Includes audio activity and all video since the last turn. With automatic
    /// activity detection, audio activity means speech and excludes silence.
    /// </summary>
    [JsonStringEnumMemberName("TURN_INCLUDES_AUDIO_ACTIVITY_AND_ALL_VIDEO")]
    TurnIncludesAudioActivityAndAllVideo,
}

