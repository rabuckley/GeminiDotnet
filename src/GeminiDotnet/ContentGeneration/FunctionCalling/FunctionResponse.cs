using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

public sealed record FunctionResponse : ContentPart
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    // TODO: This is a JSON object.
    [JsonPropertyName("response")]
    public required string Response { get; init; }
}