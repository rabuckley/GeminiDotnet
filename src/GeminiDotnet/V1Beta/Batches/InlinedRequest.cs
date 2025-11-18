using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Batches;

/// <summary>
/// The request to be processed in the batch.
/// </summary>
public sealed record InlinedRequest
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
    public required GenerateContentRequest Request { get; init; }
}

