using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The configuration for the voice to use.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record VoiceConfiguration
{
    /// <summary>
    /// The configuration for the prebuilt voice to use.
    /// </summary>
    [JsonPropertyName("prebuiltVoiceConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PrebuiltVoiceConfiguration? PrebuiltVoiceConfiguration { get; init; }

    /// <summary>
    /// Optional. The speaker identifier for synthesis.
    /// Supported formats:
    /// *   Speaker name for prebuilt voices (for example, <c>Orus</c> or <c>Kore</c>).
    /// *   Voice ID for stored voices (for example, <c>voice_xxx</c>).
    /// *   Voice replication key (for example, <c>voicekey_xxx</c>).
    /// </summary>
    [JsonPropertyName("voice")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Voice { get; init; }
}

