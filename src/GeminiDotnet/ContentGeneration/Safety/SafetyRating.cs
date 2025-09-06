using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.Safety;

public sealed class SafetyRating
{
    /// <summary>
    /// The category for this rating.
    /// </summary>
    [JsonPropertyName("category")]
    public required HarmCategory Category { get; init; }

    /// <summary>
    /// The probability of harm for this content.
    /// </summary>
    [JsonPropertyName("probability")]
    public required HarmProbability Probability { get; init; }

    /// <summary>
    /// Was this content blocked because of this rating?
    /// </summary>
    [JsonPropertyName("blocked")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Blocked { get; init; }
}
