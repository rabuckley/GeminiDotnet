using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The configuration for the prebuilt speaker to use.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record PrebuiltVoiceConfiguration
{
    /// <summary>
    /// Optional. The name of the preset voice to use.
    /// </summary>
    [JsonPropertyName("voiceName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? VoiceName { get; init; }
}

