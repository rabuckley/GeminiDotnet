using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record SpeechConfiguration
{
    /// <summary>
    /// The configuration for the speaker to use.
    /// </summary>
    [JsonPropertyName("voiceConfig")]
    public required VoiceConfiguration VoiceConfiguration { get; init; }

    /// <summary>
    /// Optional. The configuration for the multi-speaker setup. It is mutually exclusive with the
    /// <see cref="VoiceConfiguration"/> field.
    /// </summary>
    [JsonPropertyName("multiSpeakerVoiceConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MultiSpeakerVoiceConfiguration? MultiSpeakerVoiceConfiguration { get; init; }

    /// <summary>
    /// Optional. Language code (in BCP 47 format, e.g. "en-US") for speech synthesis.
    /// </summary>
    [JsonPropertyName("languageCode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LanguageCode { get; init; }
}
