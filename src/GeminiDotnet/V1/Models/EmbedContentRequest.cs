using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Models;

/// <summary>
/// Request containing the <see cref="V1.Models.Content"/> for the model to embed.
/// </summary>
public sealed record EmbedContentRequest
{
    /// <summary>
    /// Required. The content to embed. Only the <c>parts.text</c> fields will be counted.
    /// </summary>
    [JsonPropertyName("content")]
    public required Content Content { get; init; }

    /// <summary>
    /// Required. The model's resource name. This serves as an ID for the Model to use.
    /// This name should match a model name returned by the <c>ListModels</c> method.
    /// Format: <c>models/{model}</c>
    /// </summary>
    [JsonPropertyName("model")]
    public required string Model { get; init; }

    /// <summary>
    /// Optional. Optional reduced dimension for the output embedding. If set, excessive
    /// values in the output embedding are truncated from the end. Supported by
    /// newer models since 2024 only. You cannot set this value if using the
    /// earlier model (<c>models/embedding-001</c>).
    /// </summary>
    [JsonPropertyName("outputDimensionality")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? OutputDimensionality { get; init; }

    /// <summary>
    /// Optional. Optional task type for which the embeddings will be used. Not supported on
    /// earlier models (<c>models/embedding-001</c>).
    /// </summary>
    [JsonPropertyName("taskType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TaskType? TaskType { get; init; }

    /// <summary>
    /// Optional. An optional title for the text. Only applicable when TaskType is
    /// <c>RETRIEVAL_DOCUMENT</c>.
    /// Note: Specifying a <see cref="Title"/> for <c>RETRIEVAL_DOCUMENT</c> provides better quality
    /// embeddings for retrieval.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Title { get; init; }
}

