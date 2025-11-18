using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// Metadata about the state and progress of creating a tuned model returned from
/// the long-running operation
/// </summary>
public sealed record CreateTunedModelMetadata
{
    /// <summary>
    /// The completed percentage for the tuning operation.
    /// </summary>
    [JsonPropertyName("completedPercent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? CompletedPercent { get; init; }

    /// <summary>
    /// The number of steps completed.
    /// </summary>
    [JsonPropertyName("completedSteps")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? CompletedSteps { get; init; }

    /// <summary>
    /// Metrics collected during tuning.
    /// </summary>
    [JsonPropertyName("snapshots")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<TuningSnapshot>? Snapshots { get; init; }

    /// <summary>
    /// The total number of tuning steps.
    /// </summary>
    [JsonPropertyName("totalSteps")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TotalSteps { get; init; }

    /// <summary>
    /// Name of the tuned model associated with the tuning operation.
    /// </summary>
    [JsonPropertyName("tunedModel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? TunedModel { get; init; }
}

