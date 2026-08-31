using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Models;

/// <summary>
/// Batch request to get embeddings from the model for a list of prompts.
/// </summary>
public sealed record BatchEmbedContentsRequest
{
    /// <summary>
    /// Required. Embed requests for the batch. The model in each of these requests must
    /// match the model specified <c>BatchEmbedContentsRequest.model</c>.
    /// </summary>
    [JsonPropertyName("requests")]
    public required IReadOnlyList<EmbedContentRequest> Requests { get; init; }
}

