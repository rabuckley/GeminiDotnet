using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

[JsonConverter(typeof(JsonStringEnumConverter<ToolType>))]
public enum ToolType
{
    /// <summary>
    /// Unspecified tool type.
    /// </summary>
    [JsonStringEnumMemberName("TOOL_TYPE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Google search tool, maps to Tool.google_search.search_types.web_search.
    /// </summary>
    [JsonStringEnumMemberName("GOOGLE_SEARCH_WEB")]
    GoogleSearchWeb,

    /// <summary>
    /// Image search tool, maps to Tool.google_search.search_types.image_search.
    /// </summary>
    [JsonStringEnumMemberName("GOOGLE_SEARCH_IMAGE")]
    GoogleSearchImage,

    /// <summary>
    /// URL context tool, maps to Tool.url_context.
    /// </summary>
    [JsonStringEnumMemberName("URL_CONTEXT")]
    UrlContext,

    /// <summary>
    /// Google maps tool, maps to Tool.google_maps.
    /// </summary>
    [JsonStringEnumMemberName("GOOGLE_MAPS")]
    GoogleMaps,

    /// <summary>
    /// File search tool, maps to Tool.file_search.
    /// </summary>
    [JsonStringEnumMemberName("FILE_SEARCH")]
    FileSearch,
}

