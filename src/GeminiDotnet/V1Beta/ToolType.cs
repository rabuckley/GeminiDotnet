using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// The type of tool in the function call.
/// </summary>
public enum ToolType
{
    /// <summary>
    /// Unspecified tool type.
    /// </summary>
    [JsonPropertyName("TOOL_TYPE_UNSPECIFIED")]
    Unspecified = 0,

    /// <summary>
    /// Google search tool, maps to Tool.google_search.search_types.web_search.
    /// </summary>
    [JsonPropertyName("GOOGLE_SEARCH_WEB")]
    GoogleSearchWeb = 1,

    /// <summary>
    /// Image search tool, maps to Tool.google_search.search_types.image_search.
    /// </summary>
    [JsonPropertyName("GOOGLE_SEARCH_IMAGE")]
    GoogleSearchImage = 2,

    /// <summary>
    /// URL context tool, maps to Tool.url_context.
    /// </summary>
    [JsonPropertyName("URL_CONTEXT")]
    UrlContext = 3,

    /// <summary>
    /// Google maps tool, maps to Tool.google_maps.
    /// </summary>
    [JsonStringEnumMemberName("GOOGLE_MAPS")]
    GoogleMaps = 4,

    /// <summary>
    /// File search tool, maps to Tool.file_search.
    /// </summary>
    [JsonStringEnumMemberName("FILE_SEARCH")]
    FileSearch = 5,
}
