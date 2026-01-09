using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Metadata returned to client when grounding is enabled.
/// </summary>
public sealed record GroundingMetadata
{
    /// <summary>
    /// Optional. Resource name of the Google Maps widget context token that can be used
    /// with the PlacesContextElement widget in order to render contextual data.
    /// Only populated in the case that grounding with Google Maps is enabled.
    /// </summary>
    [JsonPropertyName("googleMapsWidgetContextToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? GoogleMapsWidgetContextToken { get; init; }

    /// <summary>
    /// List of supporting references retrieved from specified grounding source.
    /// When streaming, this only contains the grounding chunks that have not been
    /// included in the grounding metadata of previous responses.
    /// </summary>
    [JsonPropertyName("groundingChunks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<GroundingChunk>? GroundingChunks { get; init; }

    /// <summary>
    /// List of grounding support.
    /// </summary>
    [JsonPropertyName("groundingSupports")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<GroundingSupport>? GroundingSupports { get; init; }

    /// <summary>
    /// Metadata related to retrieval in the grounding flow.
    /// </summary>
    [JsonPropertyName("retrievalMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RetrievalMetadata? RetrievalMetadata { get; init; }

    /// <summary>
    /// Optional. Google search entry for the following-up web searches.
    /// </summary>
    [JsonPropertyName("searchEntryPoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SearchEntryPoint? SearchEntryPoint { get; init; }

    /// <summary>
    /// Web search queries for the following-up web search.
    /// </summary>
    [JsonPropertyName("webSearchQueries")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? WebSearchQueries { get; init; }
}

