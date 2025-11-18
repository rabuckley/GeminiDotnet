using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// The response to an <see cref="V1Beta.Models.EmbedContentRequest"/>.
/// </summary>
public sealed record EmbedContentResponse
{
    /// <summary>
    /// Output only. The embedding generated from the input content.
    /// </summary>
    [JsonPropertyName("embedding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ContentEmbedding? Embedding { get; init; }
}

