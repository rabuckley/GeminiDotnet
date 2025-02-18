using System.Text.Json.Serialization;

namespace GeminiDotnet.Embeddings;

/// <summary>
/// A list of floats representing an embedding.
/// </summary>
public sealed record ContentEmbedding
{
    /// <summary>
    /// The embedding values.
    /// </summary>
    [JsonPropertyName("values")]
    public required float[] Values { get; init; }
}
