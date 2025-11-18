using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// The response to a EmbedTextRequest.
/// </summary>
public sealed record EmbedTextResponse
{
    /// <summary>
    /// Output only. The embedding generated from the input text.
    /// </summary>
    [JsonPropertyName("embedding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Embedding? Embedding { get; init; }
}

