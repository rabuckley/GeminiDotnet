using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Request to batch update <see cref="V1Beta.Corpora.Chunk"/>s.
/// </summary>
public sealed record BatchUpdateChunksRequest
{
    /// <summary>
    /// Required. The request messages specifying the <see cref="V1Beta.Corpora.Chunk"/>s to update.
    /// A maximum of 100 <see cref="V1Beta.Corpora.Chunk"/>s can be updated in a batch.
    /// </summary>
    [JsonPropertyName("requests")]
    public required IReadOnlyList<UpdateChunkRequest> Requests { get; init; }
}

