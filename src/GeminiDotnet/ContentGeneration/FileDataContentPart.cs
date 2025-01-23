using System;
using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// URI-based data with a specified media type.
/// </summary>
public sealed record FileDataContentPart : ContentPart
{
    [JsonPropertyName("mime_type")]
    public required string MimeType { get; init; }

    [JsonPropertyName("uri")]
    public required Uri Uri { get; init; }
}