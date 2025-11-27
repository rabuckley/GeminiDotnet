using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Config for image generation features.
/// </summary>
public sealed record ImageConfiguration
{
    /// <summary>
    /// Optional. The aspect ratio of the image to generate. Supported aspect ratios: 1:1,
    /// 2:3, 3:2, 3:4, 4:3, 5:4, 4:5, 9:16, 16:9, 21:9.
    /// If not specified, the model will choose a default aspect ratio based on any
    /// reference images provided.
    /// </summary>
    [JsonPropertyName("aspectRatio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? AspectRatio { get; init; }

    /// <summary>
    /// Optional. Supported by gemini-3-pro-image. Allowed values: 1K, 2K, 4K.
    /// </summary>
    [JsonPropertyName("image_size")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ImageSize { get; init; }
}

