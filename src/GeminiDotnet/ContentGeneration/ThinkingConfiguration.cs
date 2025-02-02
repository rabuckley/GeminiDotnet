using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

public sealed class ThinkingConfiguration
{
    [JsonPropertyName("includeThoughts")]
    public bool? IncludeThoughts { get; init; }
}