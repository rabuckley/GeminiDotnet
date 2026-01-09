using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The status of the underlying model. This is used to indicate the stage of the
/// underlying model and the retirement time if applicable.
/// </summary>
public sealed record ModelStatus
{
    /// <summary>
    /// A message explaining the model status.
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Message { get; init; }

    /// <summary>
    /// The stage of the underlying model.
    /// </summary>
    [JsonPropertyName("modelStage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ModelStage? ModelStage { get; init; }

    /// <summary>
    /// The time at which the model will be retired.
    /// </summary>
    [JsonPropertyName("retirementTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? RetirementTime { get; init; }
}

