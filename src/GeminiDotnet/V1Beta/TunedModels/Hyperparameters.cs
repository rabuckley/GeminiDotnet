using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// Hyperparameters controlling the tuning process. Read more at
/// https://ai.google.dev/docs/model_tuning_guidance
/// </summary>
public sealed record Hyperparameters
{
    /// <summary>
    /// Immutable. The batch size hyperparameter for tuning.
    /// If not set, a default of 4 or 16 will be used based on the number of
    /// training examples.
    /// </summary>
    [JsonPropertyName("batchSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? BatchSize { get; init; }

    /// <summary>
    /// Immutable. The number of training epochs. An epoch is one pass through the training
    /// data.
    /// If not set, a default of 5 will be used.
    /// </summary>
    [JsonPropertyName("epochCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? EpochCount { get; init; }

    /// <summary>
    /// Optional. Immutable. The learning rate hyperparameter for tuning.
    /// If not set, a default of 0.001 or 0.0002 will be calculated based on the
    /// number of training examples.
    /// </summary>
    [JsonPropertyName("learningRate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? LearningRate { get; init; }

    /// <summary>
    /// Optional. Immutable. The learning rate multiplier is used to calculate a final learning_rate
    /// based on the default (recommended) value.
    /// Actual learning rate := learning_rate_multiplier * default learning rate
    /// Default learning rate is dependent on base model and dataset size.
    /// If not set, a default of 1.0 will be used.
    /// </summary>
    [JsonPropertyName("learningRateMultiplier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? LearningRateMultiplier { get; init; }
}

