using System.Text.Json.Serialization;
using GeminiDotnet.V1.Batches;

namespace GeminiDotnet.V1;

/// <summary>
/// This resource represents a long-running operation where metadata and response fields are strongly typed.
/// </summary>
public sealed record AsyncBatchEmbedContentOperation : BaseOperation
{
    /// <summary>
    /// A resource representing a batch of <c>EmbedContent</c> requests.
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EmbedContentBatch? Metadata { get; init; }

    /// <summary>
    /// Response for a <c>BatchGenerateContent</c> operation.
    /// </summary>
    [JsonPropertyName("response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AsyncBatchEmbedContentResponse? Response { get; init; }
}

