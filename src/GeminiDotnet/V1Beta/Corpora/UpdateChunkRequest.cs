using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Request to update a <see cref="V1Beta.Corpora.Chunk"/>.
/// </summary>
public sealed record UpdateChunkRequest
{
    /// <summary>
    /// Required. The <see cref="V1Beta.Corpora.Chunk"/> to update.
    /// </summary>
    [JsonPropertyName("chunk")]
    public required Chunk Chunk { get; init; }

    /// <summary>
    /// Required. The list of fields to update.
    /// Currently, this only supports updating <c>custom_metadata</c> and <c>data</c>.
    /// </summary>
    [JsonPropertyName("updateMask")]
    public required string UpdateMask { get; init; }
}

