using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Request to delete a <see cref="V1Beta.Corpora.Chunk"/>.
/// </summary>
public sealed record DeleteChunkRequest
{
    /// <summary>
    /// Required. The resource name of the <see cref="V1Beta.Corpora.Chunk"/> to delete.
    /// Example: <c>corpora/my-corpus-123/documents/the-doc-abc/chunks/some-chunk</c>
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}

