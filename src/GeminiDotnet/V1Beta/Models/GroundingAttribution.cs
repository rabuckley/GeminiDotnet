using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Attribution for a source that contributed to an answer.
/// </summary>
public sealed record GroundingAttribution
{
    /// <summary>
    /// Grounding source content that makes up this attribution.
    /// </summary>
    [JsonPropertyName("content")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Content? Content { get; init; }

    /// <summary>
    /// Output only. Identifier for the source contributing to this attribution.
    /// </summary>
    [JsonPropertyName("sourceId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AttributionSourceId? SourceId { get; init; }
}

