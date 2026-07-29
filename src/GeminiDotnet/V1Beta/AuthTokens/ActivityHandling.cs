using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

[JsonConverter(typeof(JsonStringEnumConverter<ActivityHandling>))]
public enum ActivityHandling
{
    /// <summary>
    /// If unspecified, the default behavior is <c>START_OF_ACTIVITY_INTERRUPTS</c>.
    /// </summary>
    [JsonStringEnumMemberName("ACTIVITY_HANDLING_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// If true, start of activity will interrupt the model's response (also
    /// called "barge in"). The model's current response will be cut-off in the
    /// moment of the interruption. This is the default behavior.
    /// </summary>
    [JsonStringEnumMemberName("START_OF_ACTIVITY_INTERRUPTS")]
    StartOfActivityInterrupts,

    /// <summary>
    /// The model's response will not be interrupted.
    /// </summary>
    [JsonStringEnumMemberName("NO_INTERRUPTION")]
    NoInterruption,
}

