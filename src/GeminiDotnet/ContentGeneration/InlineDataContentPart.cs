using System;
using System.Text.Json.Serialization;

using GeminiDotnet.Text.Json;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Raw data with a specified media type.
/// </summary>
public sealed record InlineDataContentPart : ContentPart
{
    [JsonPropertyName("mime_type")]
    public required string MimeType { get; init; }

    [JsonPropertyName("data")]
    [JsonConverter(typeof(JsonStringBase64Converter))]
    public required ReadOnlyMemory<byte> Data { get; init; }
}