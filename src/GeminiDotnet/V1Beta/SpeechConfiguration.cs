using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The speech generation config.
/// </summary>
public sealed record SpeechConfiguration
{
    /// <summary>
    /// Optional. Language code (in BCP 47 format, e.g. "en-US") for speech synthesis.
    /// Valid values are: de-DE, en-AU, en-GB, en-IN, en-US, es-US, fr-FR, hi-IN,
    /// pt-BR, ar-XA, es-ES, fr-CA, id-ID, it-IT, ja-JP, tr-TR, vi-VN, bn-IN,
    /// gu-IN, kn-IN, ml-IN, mr-IN, ta-IN, te-IN, nl-NL, ko-KR, cmn-CN, pl-PL,
    /// ru-RU, and th-TH.
    /// </summary>
    [JsonPropertyName("languageCode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? LanguageCode { get; init; }

    /// <summary>
    /// Optional. The configuration for the multi-speaker setup.
    /// It is mutually exclusive with the voice_config field.
    /// </summary>
    [JsonPropertyName("multiSpeakerVoiceConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public MultiSpeakerVoiceConfiguration? MultiSpeakerVoiceConfiguration { get; init; }

    /// <summary>
    /// The configuration in case of single-voice output.
    /// </summary>
    [JsonPropertyName("voiceConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public VoiceConfiguration? VoiceConfiguration { get; init; }
}

