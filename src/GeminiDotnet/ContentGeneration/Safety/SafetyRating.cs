using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.Safety;

public sealed class SafetyRating
{
    [JsonPropertyName("category")]
    public required string Category { get; init; }

    [JsonPropertyName("probability")]
    public required string Probability { get; init; }
}