using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// A single example for tuning.
/// </summary>
public sealed record TuningExample
{
    /// <summary>
    /// Required. The expected model output.
    /// </summary>
    [JsonPropertyName("output")]
    public required string Output { get; init; }

    /// <summary>
    /// Optional. Text model input.
    /// </summary>
    [JsonPropertyName("textInput")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? TextInput { get; init; }
}

