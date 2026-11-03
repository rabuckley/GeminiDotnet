using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Raw media bytes for function response.
/// Text should not be sent as raw bytes, use the 'FunctionResponse.response'
/// field.
/// </summary>
public sealed record FunctionResponseBlob
{
    /// <summary>
    /// Raw bytes for media formats.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> Data { get; init; }

    /// <summary>
    /// Optional. Display name of the blob.
    /// Used to provide a label or filename to distinguish blobs.
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// The IANA standard MIME type of the source data.
    /// Examples:
    /// - image/png
    /// - image/jpeg
    /// If an unsupported MIME type is provided, an error will be returned. For a
    /// complete list of supported types, see [Supported file
    /// formats](https://aistudio.google.com/docs/file-input-methods#supported-content-types).
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }
}

