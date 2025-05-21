using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Context of the a single url retrieval.
/// </summary>
public sealed record UrlRetrievalContext
{
    /// <summary>
    /// Retrieved url by the tool.
    /// </summary>
    [JsonPropertyName("retrievedUrl")]
    public required string RetrievedUrl { get; init; }
}
