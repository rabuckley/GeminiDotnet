using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Request to batch delete <see cref="V1Beta.Corpora.Chunk"/>s.
/// </summary>
public sealed record BatchDeleteChunksRequest
{
    /// <summary>
    /// Required. The request messages specifying the <see cref="V1Beta.Corpora.Chunk"/>s to delete.
    /// </summary>
    [JsonPropertyName("requests")]
    public required IReadOnlyList<DeleteChunkRequest> Requests { get; init; }
}

