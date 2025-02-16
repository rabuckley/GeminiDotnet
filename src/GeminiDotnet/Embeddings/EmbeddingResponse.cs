using System.Text.Json.Serialization;

namespace GeminiDotnet.Embeddings;

public sealed class EmbeddingResponse
{
    [JsonPropertyName("embedding")]
    public required Embedding Embedding { get; init; }
}
