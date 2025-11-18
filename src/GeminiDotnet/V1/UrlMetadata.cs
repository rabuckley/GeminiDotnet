using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Context of the a single url retrieval.
/// </summary>
public sealed record UrlMetadata
{
    /// <summary>
    /// Retrieved url by the tool.
    /// </summary>
    [JsonPropertyName("retrievedUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? RetrievedUrl { get; init; }

    /// <summary>
    /// Status of the url retrieval.
    /// </summary>
    [JsonPropertyName("urlRetrievalStatus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public UrlMetadataUrlRetrievalStatus? UrlRetrievalStatus { get; init; }
}

