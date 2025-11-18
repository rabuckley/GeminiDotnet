using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Models;

/// <summary>
/// Raw media bytes.
/// Text should not be sent as raw bytes, use the 'text' field.
/// </summary>
public sealed record Blob
{
    /// <summary>
    /// Raw bytes for media formats.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> Data { get; init; }

    /// <summary>
    /// The IANA standard MIME type of the source data.
    /// Examples:
    /// - image/png
    /// - image/jpeg
    /// If an unsupported MIME type is provided, an error will be returned. For a
    /// complete list of supported types, see [Supported file
    /// formats](https://ai.google.dev/gemini-api/docs/prompting_with_media#supported_file_formats).
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }
}

