using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. The MIME type of the image output.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ImageResponseFormatMimeType>))]
public enum ImageResponseFormatMimeType
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("MIME_TYPE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// JPEG image format.
    /// </summary>
    [JsonStringEnumMemberName("IMAGE_JPEG")]
    ImageJpeg,
}

