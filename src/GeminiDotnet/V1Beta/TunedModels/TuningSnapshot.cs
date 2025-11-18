using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// Record for a single tuning step.
/// </summary>
public sealed record TuningSnapshot
{
    /// <summary>
    /// Output only. The timestamp when this metric was computed.
    /// </summary>
    [JsonPropertyName("computeTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? ComputeTime { get; init; }

    /// <summary>
    /// Output only. The epoch this step was part of.
    /// </summary>
    [JsonPropertyName("epoch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? Epoch { get; init; }

    /// <summary>
    /// Output only. The mean loss of the training examples for this step.
    /// </summary>
    [JsonPropertyName("meanLoss")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? MeanLoss { get; init; }

    /// <summary>
    /// Output only. The tuning step.
    /// </summary>
    [JsonPropertyName("step")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? Step { get; init; }
}

