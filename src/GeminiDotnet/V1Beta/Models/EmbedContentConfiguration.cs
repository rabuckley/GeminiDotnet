using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Configurations for the EmbedContent request.
/// </summary>
public sealed record EmbedContentConfiguration
{
    /// <summary>
    /// Optional. Whether to extract audio from video content.
    /// </summary>
    [JsonPropertyName("audioTrackExtraction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? AudioTrackExtraction { get; init; }

    /// <summary>
    /// Optional. Whether to silently truncate the input content if it's longer
    /// than the maximum sequence length.
    /// </summary>
    [JsonPropertyName("autoTruncate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? AutoTruncate { get; init; }

    /// <summary>
    /// Optional. Whether to enable OCR for document content.
    /// </summary>
    [JsonPropertyName("documentOcr")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? DocumentOcr { get; init; }

    /// <summary>
    /// Optional. Reduced dimension for the output embedding. If set, excessive
    /// values in the output embedding are truncated from the end.
    /// </summary>
    [JsonPropertyName("outputDimensionality")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? OutputDimensionality { get; init; }

    /// <summary>
    /// Optional. The task type of the embedding.
    /// </summary>
    [JsonPropertyName("taskType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TaskType? TaskType { get; init; }

    /// <summary>
    /// Optional. The title for the text.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Title { get; init; }
}

