using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// The configuration for a single speaker in a multi speaker setup.
/// </summary>
public sealed record SpeakerVoiceConfiguration
{
    /// <summary>
    /// Required. The name of the speaker to use. Should be the same as in the prompt.
    /// </summary>
    [JsonPropertyName("speaker")]
    public required string Speaker { get; init; }

    /// <summary>
    /// Required. The configuration for the voice to use.
    /// </summary>
    [JsonPropertyName("voiceConfig")]
    public required VoiceConfiguration VoiceConfiguration { get; init; }
}
