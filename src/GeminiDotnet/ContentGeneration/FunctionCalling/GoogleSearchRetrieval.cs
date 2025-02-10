using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration.FunctionCalling;

/// <summary>
/// Tool to retrieve public web data for grounding, powered by Google.
/// </summary>
public sealed record GoogleSearchRetrieval
{
    /// <summary>
    /// Specifies the dynamic retrieval configuration for the given source.
    /// </summary>
    [JsonPropertyName("dynamicRetrievalConfig")]
    public required GoogleSearchRetrievalConfig DynamicRetrievalConfig { get; init; }
}