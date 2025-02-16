using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Attribution for a source that contributed to an answer.
/// </summary>
public sealed record GroundingAttribution
{
    /// <summary>
    /// Identifier for the source contributing to this attribution.
    /// </summary>
    [JsonPropertyName("sourceId")]
    public required AttributionSourceId SourceId { get; init; }

    /// <summary>
    /// Grounding source content that makes up this attribution.
    /// </summary>
    [JsonPropertyName("content")]
    public required Content Content { get; init; }
}
