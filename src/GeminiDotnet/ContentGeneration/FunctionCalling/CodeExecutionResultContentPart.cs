using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

public sealed record CodeExecutionResultContentPart : ContentPart
{
    [JsonPropertyName("outcome")]
    public required CodeExecutionOutcome Outcome { get; init; }

    [JsonPropertyName("output")]
    public required string Output { get; init; }
}