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
    /// Examples of supported types:
    /// - Images: image/png, image/jpeg, image/jpg, image/webp, image/heic,
    /// image/heif, image/gif, image/avif
    /// - Audio: audio/*, video/audio/s16le, video/audio/wav
    /// - Video: video/*
    /// - Text: text/plain, text/html, text/css, text/javascript,
    /// text/x-typescript, text/csv, text/markdown, text/x-python, text/xml,
    /// text/rtf, video/text/timestamp
    /// - Applications: application/x-javascript, application/x-typescript,
    /// application/x-python-code, application/json, application/x-ipynb+json,
    /// application/rtf, application/pdf For additional context,
    /// see [Supported file
    /// formats](https://ai.google.dev/gemini-api/docs/file-input-methods#supported-content-types).
    /// //
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }
}

