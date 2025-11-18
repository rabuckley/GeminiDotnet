using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Batches;

/// <summary>
/// The output of a batch request. This is returned in the
/// <see cref="V1.AsyncBatchEmbedContentResponse"/> or the <c>EmbedContentBatch.output</c> field.
/// </summary>
public sealed record EmbedContentBatchOutput
{
    /// <summary>
    /// Output only. The responses to the requests in the batch. Returned when the batch was
    /// built using inlined requests. The responses will be in the same order as
    /// the input requests.
    /// </summary>
    [JsonPropertyName("inlinedResponses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InlinedEmbedContentResponses? InlinedResponses { get; init; }

    /// <summary>
    /// Output only. The file ID of the file containing the responses.
    /// The file will be a JSONL file with a single response per line.
    /// The responses will be <see cref="V1.Models.EmbedContentResponse"/> messages formatted as JSON.
    /// The responses will be written in the same order as the input requests.
    /// </summary>
    [JsonPropertyName("responsesFile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ResponsesFile { get; init; }
}

