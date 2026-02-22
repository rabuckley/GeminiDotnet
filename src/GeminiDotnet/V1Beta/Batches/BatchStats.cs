using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Batches;

/// <summary>
/// Stats about the batch.
/// </summary>
public sealed record BatchStats
{
    /// <summary>
    /// Output only. The number of requests that failed to be processed.
    /// </summary>
    [JsonPropertyName("failedRequestCount")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? FailedRequestCount { get; init; }

    /// <summary>
    /// Output only. The number of requests that are still pending processing.
    /// </summary>
    [JsonPropertyName("pendingRequestCount")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? PendingRequestCount { get; init; }

    /// <summary>
    /// Output only. The number of requests in the batch.
    /// </summary>
    [JsonPropertyName("requestCount")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? RequestCount { get; init; }

    /// <summary>
    /// Output only. The number of requests that were successfully processed.
    /// </summary>
    [JsonPropertyName("successfulRequestCount")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? SuccessfulRequestCount { get; init; }
}

