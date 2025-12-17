using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Media resolution for the input media.
/// </summary>
public sealed record MediaResolution
{
    [JsonPropertyName("level")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public MediaResolutionLevel? Level { get; init; }
}

