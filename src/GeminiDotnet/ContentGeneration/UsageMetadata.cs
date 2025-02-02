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
    /// Total number of tokens across all the generated response candidates.
    /// </summary>
    [JsonPropertyName("candidatesTokenCount")]
    public int? CandidatesTokenCount { get; init; }

    /// <summary>
    /// Total token count for the generation request (prompt + response candidates).
    /// </summary>
    [JsonPropertyName("totalTokenCount")]
    public required int TotalTokenCount { get; init; }

    [JsonPropertyName("promptTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IEnumerable<ModalityTokenCount>? PromptTokenDetails { get; init; }

    [JsonPropertyName("candidatesTokensDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IEnumerable<ModalityTokenCount>? CandidateTokenDetails { get; init; }
}