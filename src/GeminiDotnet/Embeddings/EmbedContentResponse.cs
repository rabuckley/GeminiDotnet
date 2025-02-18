using System.Text.Json.Serialization;

namespace GeminiDotnet.Embeddings;

/// <summary>
/// The response to an <see cref="EmbedContentRequest"/>.
/// </summary>
public sealed record EmbedContentResponse
{
    /// <summary>
    /// The embedding generated from the input content.
    /// </summary>
    [JsonPropertyName("embedding")]
    public required ContentEmbedding Embedding { get; init; }
}
