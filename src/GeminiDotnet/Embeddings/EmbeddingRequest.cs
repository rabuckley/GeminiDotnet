using System.Text.Json.Serialization;

namespace GeminiDotnet.Embeddings;

public sealed class EmbeddingRequest
{
    [JsonPropertyName("content")]
    public required EmbeddingContent Content { get; init; }
}
