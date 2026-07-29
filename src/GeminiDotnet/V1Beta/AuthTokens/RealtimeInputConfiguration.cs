using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// Configures the realtime input behavior in <c>BidiGenerateContent</c>.
/// </summary>
public sealed record RealtimeInputConfiguration
{
    /// <summary>
    /// Optional. Defines what effect activity has.
    /// </summary>
    [JsonPropertyName("activityHandling")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ActivityHandling? ActivityHandling { get; init; }

    /// <summary>
    /// Optional. If not set, automatic activity detection is enabled by default. If
    /// automatic voice detection is disabled, the client must send activity
    /// signals.
    /// </summary>
    [JsonPropertyName("automaticActivityDetection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AutomaticActivityDetection? AutomaticActivityDetection { get; init; }

    /// <summary>
    /// Optional. Defines which input is included in the user's turn.
    /// </summary>
    [JsonPropertyName("turnCoverage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RealtimeInputConfigTurnCoverage? TurnCoverage { get; init; }
}

