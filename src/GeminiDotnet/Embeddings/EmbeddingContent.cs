using GeminiDotnet.ContentGeneration;
using System.Text.Json.Serialization;

namespace GeminiDotnet.Embeddings;

public sealed class EmbeddingContent
{
    [JsonPropertyName("parts")]
    public required IEnumerable<Part> Parts { get; init; }
}
