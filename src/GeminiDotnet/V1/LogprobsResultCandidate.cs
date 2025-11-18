using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Candidate for the logprobs token and score.
/// </summary>
public sealed record LogprobsResultCandidate
{
    /// <summary>
    /// The candidate's log probability.
    /// </summary>
    [JsonPropertyName("logProbability")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? LogProbability { get; init; }

    /// <summary>
    /// The candidate’s token string value.
    /// </summary>
    [JsonPropertyName("token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Token { get; init; }

    /// <summary>
    /// The candidate’s token id value.
    /// </summary>
    [JsonPropertyName("tokenId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TokenId { get; init; }
}

