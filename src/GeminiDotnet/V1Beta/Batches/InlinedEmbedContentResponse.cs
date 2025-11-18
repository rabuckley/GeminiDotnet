using System.Text.Json;
using System.Text.Json.Serialization;
using GeminiDotnet.V1Beta.Models;

namespace GeminiDotnet.V1Beta.Batches;

/// <summary>
/// The response to a single request in the batch.
/// </summary>
public sealed record InlinedEmbedContentResponse
{
    /// <summary>
    /// Output only. The error encountered while processing the request.
    /// </summary>
    [JsonPropertyName("error")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Status? Error { get; init; }

    /// <summary>
    /// Output only. The metadata associated with the request.
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Metadata { get; init; }

    /// <summary>
    /// Output only. The response to the request.
    /// </summary>
    [JsonPropertyName("response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EmbedContentResponse? Response { get; init; }
}

