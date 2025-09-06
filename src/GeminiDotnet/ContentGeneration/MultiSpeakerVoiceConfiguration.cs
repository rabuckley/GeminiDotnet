using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// The configuration for the multi-speaker setup.
/// </summary>
public sealed record MultiSpeakerVoiceConfiguration
{
    /// <summary>
    /// Required. All the enabled speaker voices.
    /// </summary>
    [JsonPropertyName("speakerVoiceConfigs")]
    public required IReadOnlyList<SpeakerVoiceConfiguration> SpeakerVoiceConfigurations { get; init; }
}
