using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

public sealed record ExecutableCodeContentPart : ContentPart
{
    [JsonPropertyName("language")]
    public required string Language { get; init; }

    [JsonPropertyName("code")]
    public required string Code { get; init; }
}