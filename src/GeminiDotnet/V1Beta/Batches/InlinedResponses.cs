using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Batches;

/// <summary>
/// The responses to the requests in the batch.
/// </summary>
public sealed record InlinedResponses
{
    /// <summary>
    /// Output only. The responses to the requests in the batch.
    /// </summary>
    [JsonPropertyName("inlinedResponses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<InlinedResponse>? InlinedResponsesValue { get; init; }
}

