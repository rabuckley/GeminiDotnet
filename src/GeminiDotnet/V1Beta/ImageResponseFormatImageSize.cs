using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Optional. The size of the image output.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ImageResponseFormatImageSize>))]
public enum ImageResponseFormatImageSize
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("IMAGE_SIZE_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// 512px image size.
    /// </summary>
    [JsonStringEnumMemberName("IMAGE_SIZE_FIVE_TWELVE")]
    FiveTwelve,

    /// <summary>
    /// 1K image size.
    /// </summary>
    [JsonStringEnumMemberName("IMAGE_SIZE_ONE_K")]
    OneK,

    /// <summary>
    /// 2K image size.
    /// </summary>
    [JsonStringEnumMemberName("IMAGE_SIZE_TWO_K")]
    TwoK,

    /// <summary>
    /// 4K image size.
    /// </summary>
    [JsonStringEnumMemberName("IMAGE_SIZE_FOUR_K")]
    FourK,
}

