using GeminiDotnet.ContentGeneration;
using System.Text.Json.Serialization;

namespace GeminiDotnet.Embeddings;

/// <summary>
/// A request to generate a text embedding vector from the input <see cref="Content" />.
/// </summary>
public sealed record EmbedContentRequest
{
    /// <summary>
    /// The <see cref="ContentGeneration.Content"/> to embed. Only the <see cref="Part"/>s' <see cref="Part.Text"/>
    /// fields will be counted.
    /// </summary>
    [JsonPropertyName("content")]
    public required Content Content { get; init; }

    /// <summary>
    /// Optional reduced dimension for the output embedding. If set, excessive values in the output embedding are
    /// truncated from the end. Supported by newer models since 2024 only. You cannot set this value if using the
    /// earlier model (<c>models/embedding-001</c>).
    /// </summary>
    [JsonPropertyName("outputDimensionality")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? OutputDimensionality { get; init; }
}
