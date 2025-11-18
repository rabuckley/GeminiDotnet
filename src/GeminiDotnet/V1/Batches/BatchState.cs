using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Batches;

[JsonConverter(typeof(JsonStringEnumConverter<BatchState>))]
public enum BatchState
{
    /// <summary>
    /// The batch state is unspecified.
    /// </summary>
    [JsonStringEnumMemberName("BATCH_STATE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// The service is preparing to run the batch.
    /// </summary>
    [JsonStringEnumMemberName("BATCH_STATE_PENDING")]
    Pending,

    /// <summary>
    /// The batch is in progress.
    /// </summary>
    [JsonStringEnumMemberName("BATCH_STATE_RUNNING")]
    Running,

    /// <summary>
    /// The batch completed successfully.
    /// </summary>
    [JsonStringEnumMemberName("BATCH_STATE_SUCCEEDED")]
    Succeeded,

    /// <summary>
    /// The batch failed.
    /// </summary>
    [JsonStringEnumMemberName("BATCH_STATE_FAILED")]
    Failed,

    /// <summary>
    /// The batch has been cancelled.
    /// </summary>
    [JsonStringEnumMemberName("BATCH_STATE_CANCELLED")]
    Cancelled,

    /// <summary>
    /// The batch has expired.
    /// </summary>
    [JsonStringEnumMemberName("BATCH_STATE_EXPIRED")]
    Expired,
}

