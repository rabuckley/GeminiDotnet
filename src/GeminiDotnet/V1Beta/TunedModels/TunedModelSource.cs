using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// Tuned model as a source for training a new model.
/// </summary>
public sealed record TunedModelSource
{
    /// <summary>
    /// Output only. The name of the base <see cref="V1Beta.Models.Model"/> this <see cref="V1Beta.TunedModels.TunedModel"/> was tuned from.
    /// Example: <c>models/gemini-1.5-flash-001</c>
    /// </summary>
    [JsonPropertyName("baseModel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? BaseModel { get; init; }

    /// <summary>
    /// Immutable. The name of the <see cref="V1Beta.TunedModels.TunedModel"/> to use as the starting point for
    /// training the new model.
    /// Example: <c>tunedModels/my-tuned-model</c>
    /// </summary>
    [JsonPropertyName("tunedModel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? TunedModel { get; init; }
}

