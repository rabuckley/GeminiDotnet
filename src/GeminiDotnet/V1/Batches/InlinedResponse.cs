using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Batches;

/// <summary>
/// The response to a single request in the batch.
/// </summary>
public sealed record InlinedResponse
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
    public GenerateContentResponse? Response { get; init; }
}

