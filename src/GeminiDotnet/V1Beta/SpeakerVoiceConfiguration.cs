using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The configuration for a single speaker in a multi speaker setup.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record SpeakerVoiceConfiguration
{
    /// <summary>
    /// Required. The name of the speaker to use. Should be the same as in the prompt.
    /// </summary>
    [JsonPropertyName("speaker")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Speaker { get; init; }

    /// <summary>
    /// Required. The configuration for the voice to use.
    /// </summary>
    [JsonPropertyName("voiceConfig")]
    public required VoiceConfiguration VoiceConfiguration { get; init; }
}

