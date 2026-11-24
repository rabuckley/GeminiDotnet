using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// The response to a EmbedTextRequest.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record BatchEmbedTextResponse
{
    /// <summary>
    /// Output only. The embeddings generated from the input text.
    /// </summary>
    [JsonPropertyName("embeddings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Embedding>? Embeddings { get; init; }
}

