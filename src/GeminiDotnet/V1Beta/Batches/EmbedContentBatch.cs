using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Batches;

/// <summary>
/// A resource representing a batch of <c>EmbedContent</c> requests.
/// </summary>
public sealed record EmbedContentBatch
{
    /// <summary>
    /// Output only. Stats about the batch.
    /// </summary>
    [JsonPropertyName("batchStats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EmbedContentBatchStats? BatchStats { get; init; }

    /// <summary>
    /// Output only. The time at which the batch was created.
    /// </summary>
    [JsonPropertyName("createTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? CreateTime { get; init; }

    /// <summary>
    /// Required. The user-defined name of this batch.
    /// </summary>
    [JsonPropertyName("displayName")]
    public required string DisplayName { get; init; }

    /// <summary>
    /// Output only. The time at which the batch processing completed.
    /// </summary>
    [JsonPropertyName("endTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? EndTime { get; init; }

    /// <summary>
    /// Required. Input configuration of the instances on which batch processing
    /// are performed.
    /// </summary>
    [JsonPropertyName("inputConfig")]
    public required InputEmbedContentConfiguration InputConfiguration { get; init; }

    /// <summary>
    /// Required. The name of the <see cref="V1Beta.Models.Model"/> to use for generating the completion.
    /// Format: <c>models/{model}</c>.
    /// </summary>
    [JsonPropertyName("model")]
    public required string Model { get; init; }

    /// <summary>
    /// Output only. Identifier. Resource name of the batch.
    /// Format: <c>batches/{batch_id}</c>.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// Output only. The output of the batch request.
    /// </summary>
    [JsonPropertyName("output")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EmbedContentBatchOutput? Output { get; init; }

    /// <summary>
    /// Optional. The priority of the batch. Batches with a higher priority value will be
    /// processed before batches with a lower priority value. Negative values are
    /// allowed. Default is 0.
    /// </summary>
    [JsonPropertyName("priority")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? Priority { get; init; }

    /// <summary>
    /// Output only. The state of the batch.
    /// </summary>
    [JsonPropertyName("state")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public BatchState? State { get; init; }

    /// <summary>
    /// Output only. The time at which the batch was last updated.
    /// </summary>
    [JsonPropertyName("updateTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? UpdateTime { get; init; }
}

