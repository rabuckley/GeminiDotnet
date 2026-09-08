using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

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
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Text { get; init; }
}

