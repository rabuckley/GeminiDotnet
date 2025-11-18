using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Text given to the model as a prompt.
/// The Model will use this TextPrompt to Generate a text completion.
/// </summary>
public sealed record TextPrompt
{
    /// <summary>
    /// Required. The prompt text.
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }
}

