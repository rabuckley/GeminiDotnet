using System.Text.Json.Serialization;
using GeminiDotnet.V1.Models;

namespace GeminiDotnet.V1;

/// <summary>
/// Metadata on the generation request's token usage.
/// </summary>
public sealed record UsageMetadata
{
    /// <summary>
    /// Output only. List of modalities of the cached content in the request input.
    /// </summary>
    [JsonPropertyName("cacheTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ModalityTokenCount>? CacheTokensDetails { get; init; }

    /// <summary>
    /// Total number of tokens across all the generated response candidates.
    /// </summary>
    [JsonPropertyName("candidatesTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? CandidatesTokenCount { get; init; }

    /// <summary>
    /// Output only. List of modalities that were returned in the response.
    /// </summary>
    [JsonPropertyName("candidatesTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ModalityTokenCount>? CandidatesTokensDetails { get; init; }

    /// <summary>
    /// Number of tokens in the prompt. When <c>cached_content</c> is set, this is
    /// still the total effective prompt size meaning this includes the number of
    /// tokens in the cached content.
    /// </summary>
    [JsonPropertyName("promptTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? PromptTokenCount { get; init; }

    /// <summary>
    /// Output only. List of modalities that were processed in the request input.
    /// </summary>
    [JsonPropertyName("promptTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ModalityTokenCount>? PromptTokensDetails { get; init; }

    /// <summary>
    /// Output only. Number of tokens of thoughts for thinking models.
    /// </summary>
    [JsonPropertyName("thoughtsTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? ThoughtsTokenCount { get; init; }

    /// <summary>
    /// Output only. Number of tokens present in tool-use prompt(s).
    /// </summary>
    [JsonPropertyName("toolUsePromptTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? ToolUsePromptTokenCount { get; init; }

    /// <summary>
    /// Output only. List of modalities that were processed for tool-use request inputs.
    /// </summary>
    [JsonPropertyName("toolUsePromptTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ModalityTokenCount>? ToolUsePromptTokensDetails { get; init; }

    /// <summary>
    /// Total token count for the generation request (prompt + response
    /// candidates).
    /// </summary>
    [JsonPropertyName("totalTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TotalTokenCount { get; init; }
}

