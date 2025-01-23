using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record ChatMessage
{
    [JsonPropertyName("role")]
    public required ChatRole Role { get; init; }

    [JsonPropertyName("parts")]
    public required IEnumerable<ContentPart> Parts { get; init; }
}