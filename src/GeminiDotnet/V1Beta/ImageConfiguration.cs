using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Config for image generation features.
/// </summary>
public sealed record ImageConfiguration
{
    /// <summary>
    /// Optional. The aspect ratio of the image to generate. Supported aspect ratios: 1:1,
    /// 2:3, 3:2, 3:4, 4:3, 9:16, 16:9, 21:9.
    /// If not specified, the model will choose a default aspect ratio based on any
    /// reference images provided.
    /// </summary>
    [JsonPropertyName("aspectRatio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? AspectRatio { get; init; }

    /// <summary>
    /// Optional. Specifies the size of generated images. Supported values are <c>1K</c>, <c>2K</c>,
    /// <c>4K</c>. If not specified, the model will use default value <c>1K</c>.
    /// </summary>
    [JsonPropertyName("imageSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ImageSize { get; init; }
}

