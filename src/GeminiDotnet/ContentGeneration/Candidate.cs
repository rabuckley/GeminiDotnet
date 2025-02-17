using GeminiDotnet.ContentGeneration.Safety;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// A response candidate generated from the model.
/// </summary>
public sealed record Candidate
{
    /// <summary>
    /// Generated content returned from the model.
    /// </summary>
    [JsonPropertyName("content")]
    public required Content Content { get; init; }

    /// <summary>
    /// The reason why the model stopped generating tokens. If empty, the model has not stopped generating tokens.
    /// </summary>
    [JsonPropertyName("finishReason")]
    public FinishReason? FinishReason { get; init; }

    /// <summary>
    /// List of ratings for the safety of a response candidate. There is at most one rating per category.
    /// </summary>
    [JsonPropertyName("safetyRatings")]
    public IReadOnlyList<SafetyRating>? SafetyRatings { get; init; }

    /// <summary>
    /// Citation information for model-generated candidate.
    /// This field may be populated with recitation information for any text included in the content. These are passages that are "recited" from copyrighted material in the foundational LLM's training data.
    /// </summary>
    [JsonPropertyName("citationMetadata")]
    public CitationMetadata? CitationMetadata { get; init; }

    /// <summary>
    /// Token count for this candidate.
    /// </summary>
    [JsonPropertyName("tokenCount")]
    public int? TokenCount { get; init; }

    /// <summary>
    /// Attribution information for sources that contributed to a grounded answer. This field is populated for GenerateAnswer calls.
    /// </summary>
    [JsonPropertyName("groundingAttributions")]
    public IReadOnlyList<GroundingAttribution>? GroundingAttributions { get; init; }

    /// <summary>
    /// Grounding metadata for the candidate. This field is populated for GenerateContent calls.
    /// </summary>
    [JsonPropertyName("groundingMetadata")]
    public GroundingMetadata? GroundingMetadata { get; init; }

    /// <summary>
    /// Average log probability score of the candidate.
    /// </summary>
    [JsonPropertyName("avgLogprobs")]
    public double? AverageLogProbability { get; init; }

    /// <summary>
    /// Log-likelihood scores for the response tokens and top tokens
    /// </summary>
    [JsonPropertyName("logprobsResult")]
    public LogprobsResult? LogprobsResult { get; init; }

    /// <summary>
    /// Index of the candidate in the list of response candidates.
    /// </summary>
    [JsonPropertyName("index")]
    public int Index { get; init; }
}
