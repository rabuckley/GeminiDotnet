using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Models;

/// <summary>
/// The response to a <see cref="V1.Models.BatchEmbedContentsRequest"/>.
/// </summary>
public sealed record BatchEmbedContentsResponse
{
    /// <summary>
    /// Output only. The embeddings for each request, in the same order as provided in the batch
    /// request.
    /// </summary>
    [JsonPropertyName("embeddings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ContentEmbedding>? Embeddings { get; init; }
}

