using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. The delivery mode for the image output.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ImageResponseFormatDelivery>))]
public enum ImageResponseFormatDelivery
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("DELIVERY_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Image data is returned inline in the response.
    /// </summary>
    [JsonStringEnumMemberName("INLINE")]
    Inline,

    /// <summary>
    /// Image data is returned as a URI.
    /// </summary>
    [JsonStringEnumMemberName("URI")]
    Uri,
}

