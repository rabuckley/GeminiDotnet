using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Batches;

/// <summary>
/// The requests to be processed in the batch if provided as part of the
/// batch creation request.
/// </summary>
public sealed record InlinedRequests
{
    /// <summary>
    /// Required. The requests to be processed in the batch.
    /// </summary>
    [JsonPropertyName("requests")]
    public required IReadOnlyList<InlinedRequest> Requests { get; init; }
}

