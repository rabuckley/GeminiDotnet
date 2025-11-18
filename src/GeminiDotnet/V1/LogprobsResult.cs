using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Logprobs Result
/// </summary>
public sealed record LogprobsResult
{
    /// <summary>
    /// Length = total number of decoding steps.
    /// The chosen candidates may or may not be in top_candidates.
    /// </summary>
    [JsonPropertyName("chosenCandidates")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<LogprobsResultCandidate>? ChosenCandidates { get; init; }

    /// <summary>
    /// Sum of log probabilities for all tokens.
    /// </summary>
    [JsonPropertyName("logProbabilitySum")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? LogProbabilitySum { get; init; }

    /// <summary>
    /// Length = total number of decoding steps.
    /// </summary>
    [JsonPropertyName("topCandidates")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<TopCandidates>? TopCandidates { get; init; }
}

