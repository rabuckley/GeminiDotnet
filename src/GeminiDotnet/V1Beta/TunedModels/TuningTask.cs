using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// Tuning tasks that create tuned models.
/// </summary>
public sealed record TuningTask
{
    /// <summary>
    /// Output only. The timestamp when tuning this model completed.
    /// </summary>
    [JsonPropertyName("completeTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? CompleteTime { get; init; }

    /// <summary>
    /// Immutable. Hyperparameters controlling the tuning process. If not provided, default
    /// values will be used.
    /// </summary>
    [JsonPropertyName("hyperparameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Hyperparameters? Hyperparameters { get; init; }

    /// <summary>
    /// Output only. Metrics collected during tuning.
    /// </summary>
    [JsonPropertyName("snapshots")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<TuningSnapshot>? Snapshots { get; init; }

    /// <summary>
    /// Output only. The timestamp when tuning this model started.
    /// </summary>
    [JsonPropertyName("startTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? StartTime { get; init; }

    /// <summary>
    /// Required. Input only. Immutable. The model training data.
    /// </summary>
    [JsonPropertyName("trainingData")]
    public required Dataset TrainingData { get; init; }
}

