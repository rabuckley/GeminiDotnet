using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Context of a single url retrieval.
/// </summary>
public sealed record UrlMetadata
{
    /// <summary>
    /// Retrieved url by the tool.
    /// </summary>
    [JsonPropertyName("retrievedUrl")]
    public required string RetrievedUrl { get; init; }

    /// <summary>
    /// Status of the url retrieval.
    /// </summary>
    [JsonPropertyName("urlRetrievalStatus")]
    public required UrlRetrievalStatus UrlRetrievalStatus { get; init; }
}
