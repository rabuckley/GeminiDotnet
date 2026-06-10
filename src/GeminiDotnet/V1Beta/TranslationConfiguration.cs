using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Config for translation features.
/// </summary>
public sealed record TranslationConfiguration
{
    /// <summary>
    /// Optional. If true, the model will generate audio when the target language is spoken,
    /// essentially it will parrot the input. If false, we will not produce audio
    /// for the target language.
    /// </summary>
    [JsonPropertyName("echoTargetLanguage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? EchoTargetLanguage { get; init; }

    /// <summary>
    /// Required. The target language for translation. Supported values are BCP-47 language
    /// codes (e.g. "en", "es", "fr").
    /// </summary>
    [JsonPropertyName("targetLanguageCode")]
    public required string TargetLanguageCode { get; init; }
}

