using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta.Models;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// A set of the feedback metadata the prompt specified in
/// <c>GenerateContentRequest.content</c>.
/// </summary>
public sealed record PromptFeedback
{
    /// <summary>
    /// Optional. If set, the prompt was blocked and no candidates are returned.
    /// Rephrase the prompt.
    /// </summary>
    [JsonPropertyName("blockReason")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PromptFeedbackBlockReason? BlockReason { get; init; }

    /// <summary>
    /// Ratings for safety of the prompt.
    /// There is at most one rating per category.
    /// </summary>
    [JsonPropertyName("safetyRatings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<SafetyRating>? SafetyRatings { get; init; }
}

