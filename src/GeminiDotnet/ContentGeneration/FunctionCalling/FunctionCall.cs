using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

public sealed record FunctionCall
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    // TODO: This is a JSON object
    [JsonPropertyName("arguments")]
    public required IEnumerable<string> Arguments { get; init; }
}