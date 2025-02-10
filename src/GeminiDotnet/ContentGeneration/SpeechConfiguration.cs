using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record SpeechConfiguration
{
    /// <summary>
    /// The configuration for the speaker to use.
    /// </summary>
    [JsonPropertyName("voiceConfig")]
    public required VoiceConfiguration VoiceConfiguration { get; init; }
}