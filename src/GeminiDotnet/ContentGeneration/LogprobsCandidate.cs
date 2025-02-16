using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Candidate for the logprobs token and score.
/// </summary>
public sealed record LogprobsCandidate
{
    /// <summary>
    /// The candidate’s token string value.
    /// </summary>
    [JsonPropertyName("token")]
    public required string Token { get; init; }

    /// <summary>
    /// The candidate’s token id value.
    /// </summary>
    [JsonPropertyName("tokenId")]
    public required int TokenId { get; init; }

    /// <summary>
    /// The candidate's log probability.
    /// </summary>
    [JsonPropertyName("logProbability")]
    public required double LogProbability { get; init; }
}
