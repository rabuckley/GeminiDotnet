using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Represents token counting info for a single modality.
/// </summary>
public sealed record ModalityTokenCount
{
    /// <summary>
    /// The modality associated with this token count.
    /// </summary>
    [JsonPropertyName("modality")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GenerativeLanguageModality? Modality { get; init; }

    /// <summary>
    /// Number of tokens.
    /// </summary>
    [JsonPropertyName("tokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TokenCount { get; init; }
}

