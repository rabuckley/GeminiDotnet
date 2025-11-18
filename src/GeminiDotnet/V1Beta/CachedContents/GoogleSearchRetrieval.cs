using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Tool to retrieve public web data for grounding, powered by Google.
/// </summary>
public sealed record GoogleSearchRetrieval
{
    /// <summary>
    /// Specifies the dynamic retrieval configuration for the given source.
    /// </summary>
    [JsonPropertyName("dynamicRetrievalConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DynamicRetrievalConfiguration? DynamicRetrievalConfiguration { get; init; }
}

