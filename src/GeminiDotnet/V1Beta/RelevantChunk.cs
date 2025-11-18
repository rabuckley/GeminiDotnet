using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta.Corpora;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The information for a chunk relevant to a query.
/// </summary>
public sealed record RelevantChunk
{
    /// <summary>
    /// <see cref="V1Beta.Corpora.Chunk"/> associated with the query.
    /// </summary>
    [JsonPropertyName("chunk")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Chunk? Chunk { get; init; }

    /// <summary>
    /// <see cref="V1Beta.Corpora.Chunk"/> relevance to the query.
    /// </summary>
    [JsonPropertyName("chunkRelevanceScore")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? ChunkRelevanceScore { get; init; }

    /// <summary>
    /// <see cref="V1Beta.Document"/> associated with the chunk.
    /// </summary>
    [JsonPropertyName("document")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Document? Document { get; init; }
}

