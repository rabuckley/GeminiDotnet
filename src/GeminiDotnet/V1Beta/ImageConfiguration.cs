using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Config for image generation features.
/// </summary>
public sealed record ImageConfiguration
{
    /// <summary>
    /// Optional. The aspect ratio of the image to generate. Supported aspect ratios: <c>1:1</c>,
    /// <c>2:3</c>, <c>3:2</c>, <c>3:4</c>, <c>4:3</c>, <c>4:5</c>, <c>5:4</c>, <c>9:16</c>, <c>16:9</c>, or <c>21:9</c>.
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

