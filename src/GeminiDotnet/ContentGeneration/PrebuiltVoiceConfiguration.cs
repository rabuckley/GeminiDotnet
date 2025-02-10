using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// The configuration for the prebuilt speaker to use.
/// </summary>
public sealed record PrebuiltVoiceConfiguration : VoiceConfiguration
{
    /// <summary>
    /// The name of the preset voice to use.
    /// </summary>
    [JsonPropertyName("voiceName")]
    public required string VoiceName { get; init; }
}