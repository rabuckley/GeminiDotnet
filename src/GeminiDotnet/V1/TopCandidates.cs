using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Candidates with top log probabilities at each decoding step.
/// </summary>
public sealed record TopCandidates
{
    /// <summary>
    /// Sorted by log probability in descending order.
    /// </summary>
    [JsonPropertyName("candidates")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<LogprobsResultCandidate>? Candidates { get; init; }
}

