using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// A response from <c>CountTokens</c>.
/// It returns the model's <c>token_count</c> for the <c>prompt</c>.
/// </summary>
public sealed record CountTokensResponse
{
    /// <summary>
    /// Number of tokens in the cached part of the prompt (the cached content).
    /// </summary>
    [JsonPropertyName("cachedContentTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? CachedContentTokenCount { get; init; }

    /// <summary>
    /// Output only. List of modalities that were processed in the cached content.
    /// </summary>
    [JsonPropertyName("cacheTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ModalityTokenCount>? CacheTokensDetails { get; init; }

    /// <summary>
    /// Output only. List of modalities that were processed in the request input.
    /// </summary>
    [JsonPropertyName("promptTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ModalityTokenCount>? PromptTokensDetails { get; init; }

    /// <summary>
    /// The number of tokens that the <see cref="V1Beta.Models.Model"/> tokenizes the <c>prompt</c> into. Always
    /// non-negative.
    /// </summary>
    [JsonPropertyName("totalTokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TotalTokens { get; init; }
}

