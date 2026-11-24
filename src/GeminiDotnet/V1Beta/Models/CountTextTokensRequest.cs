using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Counts the number of tokens in the <see cref="Prompt"/> sent to a model.
/// Models may tokenize text differently, so each model may return a different
/// <c>token_count</c>.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record CountTextTokensRequest
{
    /// <summary>
    /// Required. The free-form input text given to the model as a prompt.
    /// </summary>
    [JsonPropertyName("prompt")]
    public required TextPrompt Prompt { get; init; }
}

