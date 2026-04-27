using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Metadata on the usage of the embedding request.
/// </summary>
public sealed record EmbeddingUsageMetadata
{
    /// <summary>
    /// Output only. Number of tokens in the prompt.
    /// </summary>
    [JsonPropertyName("promptTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? PromptTokenCount { get; init; }

    /// <summary>
    /// Output only. List of modalities that were processed in the request input.
    /// </summary>
    [JsonPropertyName("promptTokenDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ModalityTokenCount>? PromptTokenDetails { get; init; }
}

