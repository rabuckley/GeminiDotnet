using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Grounding support.
/// </summary>
public sealed record GroundingSupport
{
    /// <summary>
    /// A list of indices (into 'grounding_chunk') specifying the citations associated with the claim. For instance
    /// <c>[1,3,4]</c> means that <c>grounding_chunk[1]</c>, <c>grounding_chunk[3]</c>, <c>grounding_chunk[4]</c> are
    /// the retrieved content attributed to the claim.
    /// </summary>
    [JsonPropertyName("groundingChunkIndices")]
    public required IReadOnlyList<int> GroundingChunkIndices { get; init; }

    /// <summary>
    /// Confidence score of the support references. Ranges from 0 to 1. 1 is the most confident. This list must have
    /// the same size as the groundingChunkIndices.
    /// </summary>
    [JsonPropertyName("confidenceScores")]
    public required IReadOnlyList<double> ConfidenceScores { get; init; }

    /// <summary>
    /// Segment of the content this support belongs to.
    /// </summary>
    [JsonPropertyName("segment")]
    public required Segment Segment { get; init; }
}
