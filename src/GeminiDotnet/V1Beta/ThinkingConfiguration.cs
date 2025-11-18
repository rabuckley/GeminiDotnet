using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Config for thinking features.
/// </summary>
public sealed record ThinkingConfiguration
{
    /// <summary>
    /// Indicates whether to include thoughts in the response.
    /// If true, thoughts are returned only when available.
    /// </summary>
    [JsonPropertyName("includeThoughts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? IncludeThoughts { get; init; }

    /// <summary>
    /// The number of thoughts tokens that the model should generate.
    /// </summary>
    [JsonPropertyName("thinkingBudget")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? ThinkingBudget { get; init; }

    /// <summary>
    /// Optional. The level of thoughts tokens that the model should generate.
    /// </summary>
    [JsonPropertyName("thinkingLevel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ThinkingConfigThinkingLevel? ThinkingLevel { get; init; }
}

