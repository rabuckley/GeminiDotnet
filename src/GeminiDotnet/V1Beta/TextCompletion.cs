using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta.Models;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Output text returned from a model.
/// </summary>
public sealed record TextCompletion
{
    /// <summary>
    /// Output only. Citation information for model-generated <see cref="Output"/> in this
    /// <see cref="V1Beta.TextCompletion"/>.
    /// This field may be populated with attribution information for any text
    /// included in the <see cref="Output"/>.
    /// </summary>
    [JsonPropertyName("citationMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CitationMetadata? CitationMetadata { get; init; }

    /// <summary>
    /// Output only. The generated text returned from the model.
    /// </summary>
    [JsonPropertyName("output")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Output { get; init; }

    /// <summary>
    /// Ratings for the safety of a response.
    /// There is at most one rating per category.
    /// </summary>
    [JsonPropertyName("safetyRatings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<SafetyRating>? SafetyRatings { get; init; }
}

