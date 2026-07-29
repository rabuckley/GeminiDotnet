using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.AuthTokens;

/// <summary>
/// Configures automatic detection of activity.
/// </summary>
public sealed record AutomaticActivityDetection
{
    /// <summary>
    /// Optional. If enabled (the default), detected voice and text input count as
    /// activity. If disabled, the client must send activity signals.
    /// </summary>
    [JsonPropertyName("disabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? Disabled { get; init; }

    /// <summary>
    /// Optional. Determines how likely detected speech is ended.
    /// </summary>
    [JsonPropertyName("endOfSpeechSensitivity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AutomaticActivityDetectionEndOfSpeechSensitivity? EndOfSpeechSensitivity { get; init; }

    /// <summary>
    /// Optional. The required duration of detected speech before start-of-speech is
    /// committed. The lower this value, the more sensitive the start-of-speech
    /// detection is and shorter speech can be recognized. However, this also
    /// increases the probability of false positives.
    /// </summary>
    [JsonPropertyName("prefixPaddingMs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? PrefixPaddingMs { get; init; }

    /// <summary>
    /// Optional. The required duration of detected non-speech (e.g. silence) before
    /// end-of-speech is committed. The larger this value, the longer speech gaps
    /// can be without interrupting the user's activity but this will increase
    /// the model's latency.
    /// </summary>
    [JsonPropertyName("silenceDurationMs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? SilenceDurationMs { get; init; }

    /// <summary>
    /// Optional. Determines how likely speech is to be detected.
    /// </summary>
    [JsonPropertyName("startOfSpeechSensitivity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AutomaticActivityDetectionStartOfSpeechSensitivity? StartOfSpeechSensitivity { get; init; }
}

