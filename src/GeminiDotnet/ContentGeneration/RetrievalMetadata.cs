using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Metadata related to retrieval in the grounding flow.
/// </summary>
public sealed record RetrievalMetadata
{
    /// <summary>
    /// Score indicating how likely information from google search could help answer the prompt. The score is in the
    /// range <c>[0, 1]</c>, where 0 is the least likely and 1 is the most likely. This score is only populated when
    /// Google search grounding and dynamic retrieval is enabled. It will be compared to the threshold to determine
    /// whether to trigger Google search.
    /// </summary>
    [JsonPropertyName("googleSearchDynamicRetrievalScore")]
    public double? GoogleSearchDynamicRetrievalScore { get; init; }
}
