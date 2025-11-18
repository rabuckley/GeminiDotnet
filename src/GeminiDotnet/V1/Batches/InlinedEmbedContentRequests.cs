using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Batches;

/// <summary>
/// The requests to be processed in the batch if provided as part of the
/// batch creation request.
/// </summary>
public sealed record InlinedEmbedContentRequests
{
    /// <summary>
    /// Required. The requests to be processed in the batch.
    /// </summary>
    [JsonPropertyName("requests")]
    public required IReadOnlyList<InlinedEmbedContentRequest> Requests { get; init; }
}

