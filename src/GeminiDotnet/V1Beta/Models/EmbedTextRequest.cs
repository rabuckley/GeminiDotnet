using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Request to get a text embedding from the model.
/// </summary>
public sealed record EmbedTextRequest
{
    /// <summary>
    /// Required. The model name to use with the format model=models/{model}.
    /// </summary>
    [JsonPropertyName("model")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Model { get; init; }

    /// <summary>
    /// Optional. The free-form input text that the model will turn into an embedding.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Text { get; init; }
}

