using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Configuration for image output format.
/// </summary>
public sealed record ImageResponseFormat
{
    /// <summary>
    /// Optional. The aspect ratio for the image output.
    /// </summary>
    [JsonPropertyName("aspectRatio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImageResponseFormatAspectRatio? AspectRatio { get; init; }

    /// <summary>
    /// Optional. The delivery mode for the image output.
    /// </summary>
    [JsonPropertyName("delivery")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImageResponseFormatDelivery? Delivery { get; init; }

    /// <summary>
    /// Optional. The size of the image output.
    /// </summary>
    [JsonPropertyName("imageSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImageResponseFormatImageSize? ImageSize { get; init; }

    /// <summary>
    /// Optional. The MIME type of the image output.
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImageResponseFormatMimeType? MimeType { get; init; }
}

