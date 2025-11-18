using System.Text.Json;
using System.Text.Json.Serialization;
using GeminiDotnet.V1.Models;

namespace GeminiDotnet.V1.Batches;

/// <summary>
/// The request to be processed in the batch.
/// </summary>
public sealed record InlinedEmbedContentRequest
{
    /// <summary>
    /// Optional. The metadata to be associated with the request.
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Metadata { get; init; }

    /// <summary>
    /// Required. The request to be processed in the batch.
    /// </summary>
    [JsonPropertyName("request")]
    public required EmbedContentRequest Request { get; init; }
}

