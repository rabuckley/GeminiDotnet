using GeminiDotnet.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Raw data with a specified media type.
/// </summary>
public sealed record Blob
{
    /// <summary>
    /// The IANA standard MIME type of the source data.
    /// </summary>
    [JsonPropertyName("mime_type")]
    public required string MimeType { get; init; }

    /// <summary>
    /// Raw bytes for media formats. From a base64-encoded string.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonConverter(typeof(JsonStringBase64Converter))]
    public required ReadOnlyMemory<byte> Data { get; init; }
}
