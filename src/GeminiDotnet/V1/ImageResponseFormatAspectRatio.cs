using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Optional. The aspect ratio for the image output.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ImageResponseFormatAspectRatio>))]
public enum ImageResponseFormatAspectRatio
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_UNSPECIFIED")]
    Unspecified,

    /// <summary>
    /// 1:1 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_ONE_BY_ONE")]
    OneByOne,

    /// <summary>
    /// 2:3 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_TWO_BY_THREE")]
    TwoByThree,

    /// <summary>
    /// 3:2 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_THREE_BY_TWO")]
    ThreeByTwo,

    /// <summary>
    /// 3:4 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_THREE_BY_FOUR")]
    ThreeByFour,

    /// <summary>
    /// 4:3 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_FOUR_BY_THREE")]
    FourByThree,

    /// <summary>
    /// 4:5 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_FOUR_BY_FIVE")]
    FourByFive,

    /// <summary>
    /// 5:4 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_FIVE_BY_FOUR")]
    FiveByFour,

    /// <summary>
    /// 9:16 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_NINE_BY_SIXTEEN")]
    NineBySixteen,

    /// <summary>
    /// 16:9 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_SIXTEEN_BY_NINE")]
    SixteenByNine,

    /// <summary>
    /// 21:9 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_TWENTY_ONE_BY_NINE")]
    TwentyOneByNine,

    /// <summary>
    /// 1:8 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_ONE_BY_EIGHT")]
    OneByEight,

    /// <summary>
    /// 8:1 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_EIGHT_BY_ONE")]
    EightByOne,

    /// <summary>
    /// 1:4 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_ONE_BY_FOUR")]
    OneByFour,

    /// <summary>
    /// 4:1 aspect ratio.
    /// </summary>
    [JsonStringEnumMemberName("ASPECT_RATIO_FOUR_BY_ONE")]
    FourByOne,
}

