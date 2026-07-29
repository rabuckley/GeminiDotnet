using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Request for an <c>AsyncBatchEmbedContent</c> operation.
/// </summary>
public sealed record AsyncBatchEmbedContentRequest
{
    /// <summary>
    /// Required. The batch to create.
    /// </summary>
    [JsonPropertyName("batch")]
    public required EmbedContentBatch Batch { get; init; }
}

