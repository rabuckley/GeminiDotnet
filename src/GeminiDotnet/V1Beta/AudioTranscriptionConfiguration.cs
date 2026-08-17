using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The audio transcription configuration.
/// </summary>
public sealed record AudioTranscriptionConfiguration
{
    /// <summary>
    /// Optional. A list of phrases used for speech adaptation, which biases the ASR model to
    /// improve recognition of these specific terms.
    /// </summary>
    [Obsolete]
    [JsonPropertyName("adaptationPhrases")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? AdaptationPhrases { get; init; }

    /// <summary>
    /// Optional. A list of custom vocabulary phrases to bias the speech recognition model
    /// toward recognizing specific terms (product names, proper nouns, jargon).
    /// </summary>
    [JsonPropertyName("customVocabulary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? CustomVocabulary { get; init; }

    /// <summary>
    /// Optional. Configures speaker diarization.
    /// </summary>
    [JsonPropertyName("diarization")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? Diarization { get; init; }

    /// <summary>
    /// Optional. The model will detect the language automatically.
    /// </summary>
    [Obsolete]
    [JsonPropertyName("languageAuto")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LanguageAuto? LanguageAuto { get; init; }

    /// <summary>
    /// Optional. BCP-47 language codes providing hints about the languages present in the
    /// audio. If omitted or empty, defaults to automatic language detection.
    /// </summary>
    [JsonPropertyName("languageCodes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? LanguageCodes { get; init; }

    /// <summary>
    /// Optional. Specifies one or more languages in the audio.
    /// </summary>
    [Obsolete]
    [JsonPropertyName("languageHints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LanguageHints? LanguageHints { get; init; }

    /// <summary>
    /// Optional. Configures transcription mode. Supported values: <c>VERBATIM</c>,
    /// <c>SMART</c>. If unspecified, defaults to <c>VERBATIM</c> transcription.
    /// In <c>SMART</c> mode, the model performs disfluency removal (eliminating
    /// filler words, repetitions, and false starts), light grammatical cleanup,
    /// automatic formatting (paragraphs, bullet points, numbered lists), and
    /// minor user edits (inline self-corrections).
    /// Timestamps and diarization are incompatible with mode <c>SMART</c>.
    /// </summary>
    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AudioTranscriptionConfigMode? Mode { get; init; }

    /// <summary>
    /// Optional. Configures word-level timestamp generation.
    /// </summary>
    [JsonPropertyName("wordTimestamp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? WordTimestamp { get; init; }
}

