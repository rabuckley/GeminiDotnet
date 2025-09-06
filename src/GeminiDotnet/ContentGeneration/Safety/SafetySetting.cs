using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.Safety;

public sealed record SafetySetting
{
    [JsonPropertyName("category")]
    public required HarmCategory HarmCategory { get; init; }

    [JsonPropertyName("threshold")]
    public required HarmBlockThreshold BlockThreshold { get; init; }
}
