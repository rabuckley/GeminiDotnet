using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Config for thinking features.
/// </summary>
public sealed record ThinkingConfiguration
{
    /// <summary>
    /// Indicates whether to include thoughts in the response. If true, thoughts are returned only when available.
    /// </summary>
    [JsonPropertyName("includeThoughts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IncludeThoughts { get; init; }

    /// <summary>
    /// The number of thoughts tokens that the model should generate.
    /// </summary>
    [JsonPropertyName("thinkingBudget")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ThinkingBudget { get; init; }
}
