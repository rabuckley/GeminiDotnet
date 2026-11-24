using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Batch request to get embeddings from the model for a list of prompts.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record BatchEmbedContentsRequest
{
    /// <summary>
    /// Required. Embed requests for the batch. The model in each of these requests must
    /// match the model specified <c>BatchEmbedContentsRequest.model</c>.
    /// </summary>
    [JsonPropertyName("requests")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<EmbedContentRequest>? Requests { get; init; }
}

