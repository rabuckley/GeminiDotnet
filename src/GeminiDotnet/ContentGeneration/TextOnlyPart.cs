using GeminiDotnet.ContentGeneration.FunctionCalling;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed record TextOnlyPart
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }

}