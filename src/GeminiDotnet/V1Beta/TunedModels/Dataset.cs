using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// Dataset for training or validation.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record Dataset
{
    /// <summary>
    /// Optional. Inline examples with simple input/output text.
    /// </summary>
    [JsonPropertyName("examples")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TuningExamples? Examples { get; init; }
}

