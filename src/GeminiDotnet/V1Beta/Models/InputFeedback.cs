using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Feedback related to the input data used to answer the question, as opposed
/// to the model-generated response to the question.
/// </summary>
public sealed record InputFeedback
{
    /// <summary>
    /// Optional. If set, the input was blocked and no candidates are returned.
    /// Rephrase the input.
    /// </summary>
    [JsonPropertyName("blockReason")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InputFeedbackBlockReason? BlockReason { get; init; }

    /// <summary>
    /// Ratings for safety of the input.
    /// There is at most one rating per category.
    /// </summary>
    [JsonPropertyName("safetyRatings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<SafetyRating>? SafetyRatings { get; init; }
}

