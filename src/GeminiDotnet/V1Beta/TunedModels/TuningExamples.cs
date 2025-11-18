using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// A set of tuning examples. Can be training or validation data.
/// </summary>
public sealed record TuningExamples
{
    /// <summary>
    /// The examples. Example input can be for text or discuss, but all examples
    /// in a set must be of the same type.
    /// </summary>
    [JsonPropertyName("examples")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<TuningExample>? Examples { get; init; }
}

