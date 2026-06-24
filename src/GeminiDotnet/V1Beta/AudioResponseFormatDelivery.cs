using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. The delivery mode for the audio output.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<AudioResponseFormatDelivery>))]
public enum AudioResponseFormatDelivery
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("DELIVERY_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// Audio data is returned inline in the response.
    /// </summary>
    [JsonStringEnumMemberName("INLINE")]
    Inline,

    /// <summary>
    /// Audio data is returned as a URI.
    /// </summary>
    [JsonStringEnumMemberName("URI")]
    Uri,
}

