using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Metadata related to retrieval in the grounding flow.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record RetrievalMetadata
{
    /// <summary>
    /// Optional. Score indicating how likely information from google search could help
    /// answer the prompt. The score is in the range [0, 1], where 0 is the least
    /// likely and 1 is the most likely. This score is only populated when
    /// google search grounding and dynamic retrieval is enabled. It will be
    /// compared to the threshold to determine whether to trigger google search.
    /// </summary>
    [JsonPropertyName("googleSearchDynamicRetrievalScore")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float? GoogleSearchDynamicRetrievalScore { get; init; }
}

