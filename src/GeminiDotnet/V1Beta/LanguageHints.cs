using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Provides hints to the model about possible languages present in the audio.
/// </summary>
[Obsolete]
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record LanguageHints
{
    /// <summary>
    /// Required. BCP-47 language codes.
    /// </summary>
    [Obsolete]
    [JsonPropertyName("languageCodes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? LanguageCodes { get; init; }
}

