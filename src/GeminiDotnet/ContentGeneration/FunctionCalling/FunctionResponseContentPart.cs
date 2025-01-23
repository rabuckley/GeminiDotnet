using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

public sealed record FunctionResponseContentPart : ContentPart
{
    [JsonPropertyName("functionResponse")]
    public required FunctionResponse FunctionResponse { get; init; }
}