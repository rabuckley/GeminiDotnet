using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Candidate for the logprobs token and score.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
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

