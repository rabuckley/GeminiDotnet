using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The configuration for the voice to use.
/// </summary>
public sealed record VoiceConfiguration
{
    /// <summary>
    /// The configuration for the prebuilt voice to use.
    /// </summary>
    [JsonPropertyName("prebuiltVoiceConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PrebuiltVoiceConfiguration? PrebuiltVoiceConfiguration { get; init; }
}

