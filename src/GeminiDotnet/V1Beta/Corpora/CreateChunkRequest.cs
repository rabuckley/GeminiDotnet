using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Request to create a <see cref="V1Beta.Corpora.Chunk"/>.
/// </summary>
public sealed record CreateChunkRequest
{
    /// <summary>
    /// Required. The <see cref="V1Beta.Corpora.Chunk"/> to create.
    /// </summary>
    [JsonPropertyName("chunk")]
    public required Chunk Chunk { get; init; }

    /// <summary>
    /// Required. The name of the <see cref="V1Beta.Document"/> where this <see cref="V1Beta.Corpora.Chunk"/> will be created.
    /// Example: <c>corpora/my-corpus-123/documents/the-doc-abc</c>
    /// </summary>
    [JsonPropertyName("parent")]
    public required string Parent { get; init; }
}

