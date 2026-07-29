using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Media resolution for tokenization.
/// </summary>
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

