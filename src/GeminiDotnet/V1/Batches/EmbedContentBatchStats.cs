using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Batches;

/// <summary>
/// Stats about the batch.
/// </summary>
public sealed record EmbedContentBatchStats
{
    /// <summary>
    /// Output only. The number of requests that failed to be processed.
    /// </summary>
    [JsonPropertyName("failedRequestCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? FailedRequestCount { get; init; }

    /// <summary>
    /// Output only. The number of requests that are still pending processing.
    /// </summary>
    [JsonPropertyName("pendingRequestCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? PendingRequestCount { get; init; }

    /// <summary>
    /// Output only. The number of requests in the batch.
    /// </summary>
    [JsonPropertyName("requestCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? RequestCount { get; init; }

    /// <summary>
    /// Output only. The number of requests that were successfully processed.
    /// </summary>
    [JsonPropertyName("successfulRequestCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? SuccessfulRequestCount { get; init; }
}

