using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Safety rating for a piece of content.
/// The safety rating contains the category of harm and the
/// harm probability level in that category for a piece of content.
/// Content is classified for safety across a number of
/// harm categories and the probability of the harm classification is included
/// here.
/// </summary>
public sealed record SafetyRating
{
    /// <summary>
    /// Was this content blocked because of this rating?
    /// </summary>
    [JsonPropertyName("blocked")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? Blocked { get; init; }

    /// <summary>
    /// Required. The category for this rating.
    /// </summary>
    [JsonPropertyName("category")]
    public required HarmCategory Category { get; init; }

    /// <summary>
    /// Required. The probability of harm for this content.
    /// </summary>
    [JsonPropertyName("probability")]
    public required SafetyRatingProbability Probability { get; init; }
}

