using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Tool to retrieve public web data for grounding, powered by Google.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record GoogleSearchRetrieval
{
    /// <summary>
    /// Specifies the dynamic retrieval configuration for the given source.
    /// </summary>
    [JsonPropertyName("dynamicRetrievalConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DynamicRetrievalConfiguration? DynamicRetrievalConfiguration { get; init; }
}

