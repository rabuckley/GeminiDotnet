using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Request to generate a message response from the model.
/// </summary>
public sealed record GenerateMessageRequest
{
    /// <summary>
    /// Optional. The number of generated response messages to return.
    /// This value must be between
    /// <c>[1, 8]</c>, inclusive. If unset, this will default to <c>1</c>.
    /// </summary>
    [JsonPropertyName("candidateCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? CandidateCount { get; init; }

    /// <summary>
    /// Required. The structured textual input given to the model as a prompt.
    /// Given a
    /// prompt, the model will return what it predicts is the next message in the
    /// discussion.
    /// </summary>
    [JsonPropertyName("prompt")]
    public required MessagePrompt Prompt { get; init; }

    /// <summary>
    /// Optional. Controls the randomness of the output.
    /// Values can range over <c>[0.0,1.0]</c>,
    /// inclusive. A value closer to <c>1.0</c> will produce responses that are more
    /// varied, while a value closer to <c>0.0</c> will typically result in
    /// less surprising responses from the model.
    /// </summary>
    [JsonPropertyName("temperature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? Temperature { get; init; }

    /// <summary>
    /// Optional. The maximum number of tokens to consider when sampling.
    /// The model uses combined Top-k and nucleus sampling.
    /// Top-k sampling considers the set of <c>top_k</c> most probable tokens.
    /// </summary>
    [JsonPropertyName("topK")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TopK { get; init; }

    /// <summary>
    /// Optional. The maximum cumulative probability of tokens to consider when sampling.
    /// The model uses combined Top-k and nucleus sampling.
    /// Nucleus sampling considers the smallest set of tokens whose probability
    /// sum is at least <c>top_p</c>.
    /// </summary>
    [JsonPropertyName("topP")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? TopP { get; init; }
}

