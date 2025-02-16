using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.Safety;

public sealed record SafetySetting
{
    [JsonPropertyName("category")]
    public required string HarmCategory { get; init; }

    [JsonPropertyName("threshold")]
    public required string BlockThreshold { get; init; }
}
