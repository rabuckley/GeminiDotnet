using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Logprobs Result
/// </summary>
public sealed record LogprobsResult
{
    /// <summary>
    /// Length = total number of decoding steps.
    /// </summary>
    [JsonPropertyName("topCandidates")]
    public required IReadOnlyCollection<TopCandidates> TopCandidates { get; init; }

    /// <summary>
    /// Length = total number of decoding steps. The chosen candidates may or may not be in topCandidates.
    /// </summary>
    [JsonPropertyName("chosenCandidates")]
    public required IReadOnlyCollection<LogprobsCandidate> ChosenCandidates { get; init; }
}