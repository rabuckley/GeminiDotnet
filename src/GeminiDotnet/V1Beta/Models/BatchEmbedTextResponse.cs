using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// The response to a EmbedTextRequest.
/// </summary>
public sealed record BatchEmbedTextResponse
{
    /// <summary>
    /// Output only. The embeddings generated from the input text.
    /// </summary>
    [JsonPropertyName("embeddings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Embedding>? Embeddings { get; init; }
}

