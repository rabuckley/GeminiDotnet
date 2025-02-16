using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Represents token counting info for a single modality.
/// </summary>
public sealed record ModalityTokenCount
{
    /// <summary>
    /// The modality associated with this token count.
    /// </summary>
    [JsonPropertyName("modality")]
    public Modality Modality { get; init; }

    /// <summary>
    /// Number of tokens.
    /// </summary>
    [JsonPropertyName("tokenCount")]
    public int TokenCount { get; init; }
}
