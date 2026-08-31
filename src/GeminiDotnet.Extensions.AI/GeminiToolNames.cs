using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// The Gemini tool names reported on <see cref="CitationAnnotation.ToolName"/>, naming the tool the
/// evidence behind a citation came from.
/// </summary>
public static class GeminiToolNames
{
    /// <summary>
    /// Grounding with Google Search. Reported for both web and image results.
    /// </summary>
    public const string GoogleSearch = "google_search";

    /// <summary>
    /// Retrieval from a file search store.
    /// </summary>
    public const string FileSearch = "file_search";

    /// <summary>
    /// Grounding with Google Maps.
    /// </summary>
    public const string GoogleMaps = "google_maps";
}
