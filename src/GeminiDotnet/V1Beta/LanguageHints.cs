using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Provides hints to the model about possible languages present in the audio.
/// </summary>
[Obsolete]
public sealed record LanguageHints
{
    /// <summary>
    /// Required. BCP-47 language codes.
    /// </summary>
    [Obsolete]
    [JsonPropertyName("languageCodes")]
    public required IReadOnlyList<string> LanguageCodes { get; init; }
}

