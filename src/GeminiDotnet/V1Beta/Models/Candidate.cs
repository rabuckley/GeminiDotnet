using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// A response candidate generated from the model.
/// </summary>
public sealed record Candidate
{
    /// <summary>
    /// Output only. Average log probability score of the candidate.
    /// </summary>
    [JsonPropertyName("avgLogprobs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double? AvgLogprobs { get; init; }

    /// <summary>
    /// Output only. Citation information for model-generated candidate.
    /// This field may be populated with recitation information for any text
    /// included in the <see cref="Content"/>. These are passages that are "recited" from
    /// copyrighted material in the foundational LLM's training data.
    /// </summary>
    [JsonPropertyName("citationMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CitationMetadata? CitationMetadata { get; init; }

    /// <summary>
    /// Output only. Generated content returned from the model.
    /// </summary>
    [JsonPropertyName("content")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Content? Content { get; init; }

    /// <summary>
    /// Optional. Output only. Details the reason why the model stopped generating tokens.
    /// This is populated only when <c>finish_reason</c> is set.
    /// </summary>
    [JsonPropertyName("finishMessage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FinishMessage { get; init; }

    /// <summary>
    /// Optional. Output only. The reason why the model stopped generating tokens.
    /// If empty, the model has not stopped generating tokens.
    /// </summary>
    [JsonPropertyName("finishReason")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CandidateFinishReason? FinishReason { get; init; }

    /// <summary>
    /// Output only. Attribution information for sources that contributed to a grounded answer.
    /// This field is populated for <c>GenerateAnswer</c> calls.
    /// </summary>
    [JsonPropertyName("groundingAttributions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<GroundingAttribution>? GroundingAttributions { get; init; }

    /// <summary>
    /// Output only. Grounding metadata for the candidate.
    /// This field is populated for <c>GenerateContent</c> calls.
    /// </summary>
    [JsonPropertyName("groundingMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GroundingMetadata? GroundingMetadata { get; init; }

    /// <summary>
    /// Output only. Index of the candidate in the list of response candidates.
    /// </summary>
    [JsonPropertyName("index")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? Index { get; init; }

    /// <summary>
    /// Output only. Log-likelihood scores for the response tokens and top tokens
    /// </summary>
    [JsonPropertyName("logprobsResult")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LogprobsResult? LogprobsResult { get; init; }

    /// <summary>
    /// List of ratings for the safety of a response candidate.
    /// There is at most one rating per category.
    /// </summary>
    [JsonPropertyName("safetyRatings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<SafetyRating>? SafetyRatings { get; init; }

    /// <summary>
    /// Output only. Token count for this candidate.
    /// </summary>
    [JsonPropertyName("tokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TokenCount { get; init; }

    /// <summary>
    /// Output only. Metadata related to url context retrieval tool.
    /// </summary>
    [JsonPropertyName("urlContextMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public UrlContextMetadata? UrlContextMetadata { get; init; }
}

