using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Candidates with top log probabilities at each decoding step.
/// </summary>
public sealed record TopCandidates
{
    /// <summary>
    /// Sorted by log probability in descending order.
    /// </summary>
    [JsonPropertyName("candidates")]
    public required IReadOnlyList<LogprobsCandidate> Candidates { get; init; }
}
