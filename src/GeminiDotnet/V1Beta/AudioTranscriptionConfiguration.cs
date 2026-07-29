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
    [JsonPropertyName("languageAuto")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LanguageAuto? LanguageAuto { get; init; }

    /// <summary>
    /// Optional. Specifies one or more languages in the audio.
    /// </summary>
    [JsonPropertyName("languageHints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LanguageHints? LanguageHints { get; init; }

    /// <summary>
    /// Optional. Configures word-level timestamp generation.
    /// </summary>
    [JsonPropertyName("wordTimestamp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? WordTimestamp { get; init; }
}

