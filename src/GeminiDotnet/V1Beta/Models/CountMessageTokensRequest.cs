using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Counts the number of tokens in the <see cref="Prompt"/> sent to a model.
/// Models may tokenize text differently, so each model may return a different
/// <c>token_count</c>.
/// </summary>
public sealed record CountMessageTokensRequest
{
    /// <summary>
    /// Required. The prompt, whose token count is to be returned.
    /// </summary>
    [JsonPropertyName("prompt")]
    public required MessagePrompt Prompt { get; init; }
}

