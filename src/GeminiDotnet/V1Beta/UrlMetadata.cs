using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Context of the a single url retrieval.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
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

