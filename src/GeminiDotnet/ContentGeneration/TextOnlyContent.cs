using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record TextOnlyContent
{
    [JsonPropertyName("parts")]
    public required TextOnlyPart Parts { get; init; }

}