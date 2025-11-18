using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Models;

/// <summary>
/// The response to an <see cref="V1.Models.EmbedContentRequest"/>.
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

