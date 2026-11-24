using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The configuration for the multi-speaker setup.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record MultiSpeakerVoiceConfiguration
{
    /// <summary>
    /// Required. All the enabled speaker voices.
    /// </summary>
    [JsonPropertyName("speakerVoiceConfigs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<SpeakerVoiceConfiguration>? SpeakerVoiceConfigurations { get; init; }
}

