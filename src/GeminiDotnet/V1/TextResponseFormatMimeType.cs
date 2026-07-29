using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. The MIME type of the text output.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<TextResponseFormatMimeType>))]
public enum TextResponseFormatMimeType
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("MIME_TYPE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// JSON output format.
    /// </summary>
    [JsonStringEnumMemberName("APPLICATION_JSON")]
    ApplicationJson,

    /// <summary>
    /// Plain text output format.
    /// </summary>
    [JsonStringEnumMemberName("TEXT_PLAIN")]
    TextPlain,
}

