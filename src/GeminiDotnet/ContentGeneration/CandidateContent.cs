using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record CandidateContent
{
    [JsonPropertyName("parts")]
    public required IEnumerable<ContentPart> Parts { get; init; }

    [JsonPropertyName("role")]
    public required string Role { get; init; }
}