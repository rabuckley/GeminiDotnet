using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Grounding support.
/// </summary>
public sealed record GroundingSupport
{
    /// <summary>
    /// Optional. Confidence score of the support references. Ranges from 0 to 1. 1 is the
    /// most confident. This list must have the same size as the
    /// grounding_chunk_indices.
    /// </summary>
    [JsonPropertyName("confidenceScores")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<float> ConfidenceScores { get; init; }

    /// <summary>
    /// Optional. A list of indices (into 'grounding_chunk' in
    /// <c>response.candidate.grounding_metadata</c>) specifying the citations
    /// associated with the claim. For instance [1,3,4] means that
    /// grounding_chunk[1], grounding_chunk[3], grounding_chunk[4] are the
    /// retrieved content attributed to the claim. If the response is streaming,
    /// the grounding_chunk_indices refer to the indices across all responses.
    /// It is the client's responsibility to accumulate the grounding chunks from
    /// all responses (while maintaining the same order).
    /// </summary>
    [JsonPropertyName("groundingChunkIndices")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<int> GroundingChunkIndices { get; init; }

    /// <summary>
    /// Segment of the content this support belongs to.
    /// </summary>
    [JsonPropertyName("segment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Segment? Segment { get; init; }
}

