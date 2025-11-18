using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Request to batch create <see cref="V1Beta.Corpora.Chunk"/>s.
/// </summary>
public sealed record BatchCreateChunksRequest
{
    /// <summary>
    /// Required. The request messages specifying the <see cref="V1Beta.Corpora.Chunk"/>s to create.
    /// A maximum of 100 <see cref="V1Beta.Corpora.Chunk"/>s can be created in a batch.
    /// </summary>
    [JsonPropertyName("requests")]
    public required IReadOnlyList<CreateChunkRequest> Requests { get; init; }
}

