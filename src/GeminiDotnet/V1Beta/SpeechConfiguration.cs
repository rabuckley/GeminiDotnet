using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Config for speech generation and transcription.
/// </summary>
public sealed record SpeechConfiguration
{
    /// <summary>
    /// Optional. The IETF [BCP-47](https://www.rfc-editor.org/rfc/bcp/bcp47.txt) language
    /// code that the user configured the app to use. Used for speech recognition
    /// and synthesis.
    /// Valid values are: <c>de-DE</c>, <c>en-AU</c>, <c>en-GB</c>, <c>en-IN</c>, <c>en-US</c>, <c>es-US</c>,
    /// <c>fr-FR</c>, <c>hi-IN</c>, <c>pt-BR</c>, <c>ar-XA</c>, <c>es-ES</c>, <c>fr-CA</c>, <c>id-ID</c>, <c>it-IT</c>,
    /// <c>ja-JP</c>, <c>tr-TR</c>, <c>vi-VN</c>, <c>bn-IN</c>, <c>gu-IN</c>, <c>kn-IN</c>, <c>ml-IN</c>, <c>mr-IN</c>,
    /// <c>ta-IN</c>, <c>te-IN</c>, <c>nl-NL</c>, <c>ko-KR</c>, <c>cmn-CN</c>, <c>pl-PL</c>, <c>ru-RU</c>, and
    /// <c>th-TH</c>.
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

