using System.Text.Json.Serialization;

namespace GeminiDotnet.Embeddings;

public sealed class Embedding
{
    [JsonPropertyName("values")]
    public required float[] Values { get; init; }
}