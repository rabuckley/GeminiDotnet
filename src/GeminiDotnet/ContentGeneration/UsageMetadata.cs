using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Metadata on the generation request's token usage.
/// </summary>
public sealed record UsageMetadata
{
    /// <summary>
    /// Number of tokens in the prompt.
    /// </summary>
    [JsonPropertyName("promptTokenCount")]
    public required int PromptTokenCount { get; init; }

    /// <summary>
    /// Number of tokens in the cached part of the prompt (the cached content)
    /// </summary>
    [JsonPropertyName("cachedContentTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int CachedContentTokenCount { get; init; }

    /// <summary>
    /// Total number of tokens across all the generated response candidates.
    /// </summary>
    [JsonPropertyName("candidatesTokenCount")]
    public int? CandidatesTokenCount { get; init; }

    /// <summary>
    /// Number of tokens present in tool-use prompt(s).
    /// </summary>
    [JsonPropertyName("toolUsePromptTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ToolUsePromptTokenCount { get; init; }

    /// <summary>
    /// Number of tokens of thoughts for thinking models.
    /// </summary>
    [JsonPropertyName("thoughtsTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ThoughtsTokenCount { get; init; }

    /// <summary>
    /// Total token count for the generation request (prompt + response candidates).
    /// </summary>
    [JsonPropertyName("totalTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public required int TotalTokenCount { get; init; }

    /// <summary>
    /// Output only. List of modalities that were processed in the request input.
    /// </summary>
    [JsonPropertyName("promptTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<ModalityTokenCount>? PromptTokenDetails { get; init; }

    /// <summary>
    /// Output only. List of modalities that were processed in the request input.
    /// </summary>
    [JsonPropertyName("cacheTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<ModalityTokenCount>? CacheTokenDetails { get; init; }

    /// <summary>
    /// Output only. List of modalities that were returned in the response.
    /// </summary>
    [JsonPropertyName("candidatesTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<ModalityTokenCount>? CandidateTokenDetails { get; init; }

    /// <summary>
    /// Output only. List of modalities that were returned in the response.
    /// </summary>
    [JsonPropertyName("toolUsePromptTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<ModalityTokenCount>? ToolUsePromptTokenDetails { get; init; }
}
