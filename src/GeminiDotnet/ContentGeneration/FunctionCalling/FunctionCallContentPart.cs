using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

public sealed record FunctionCallContentPart : ContentPart
{
    [JsonPropertyName("functionCall")]
    public required FunctionCall FunctionCall { get; init; }
}