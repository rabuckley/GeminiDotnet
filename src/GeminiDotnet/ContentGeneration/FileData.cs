using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// URI-based data with a specified media type.
/// </summary>
public sealed record FileData
{
    [JsonPropertyName("mime_type")]
    public required string MimeType { get; init; }

    [JsonPropertyName("uri")]
    public required Uri Uri { get; init; }
}
