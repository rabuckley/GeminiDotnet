using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The configuration for the prebuilt speaker to use.
/// </summary>
public sealed record PrebuiltVoiceConfiguration
{
    /// <summary>
    /// The name of the preset voice to use.
    /// </summary>
    [JsonPropertyName("voiceName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? VoiceName { get; init; }
}

