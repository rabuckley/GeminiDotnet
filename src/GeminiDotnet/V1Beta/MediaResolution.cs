using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Media resolution for tokenization.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record MediaResolution
{
    /// <summary>
    /// The tokenization quality used for given media.
    /// for Gemini API support .
    /// </summary>
    [JsonPropertyName("level")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public MediaResolutionLevel? Level { get; init; }
}

