using GeminiDotnet.ContentGeneration.Safety;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// A set of the feedback metadata the prompt specified in <see cref="GenerateContentRequest.Contents"/>.
/// </summary>
public sealed class PromptFeedback
{
    /// <summary>
    /// Ratings for safety of the prompt. There is at most one rating per category.
    /// </summary>
    [JsonPropertyName("safetyRatings")]
    public required IReadOnlyCollection<SafetyRating> SafetyRatings { get; init; }

    /// <summary>
    /// If set, the prompt was blocked and no candidates are returned. Rephrase the prompt.
    /// </summary>
    [JsonPropertyName("blockReason")]
    public BlockReason? BlockReason { get; init; }
}
