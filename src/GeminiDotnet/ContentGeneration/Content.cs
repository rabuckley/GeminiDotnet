using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record Content
{
    [JsonPropertyName("parts")]
    public required IReadOnlyCollection<Part> Parts { get; init; }

    [JsonPropertyName("role")]
    public required string Role { get; init; }
}