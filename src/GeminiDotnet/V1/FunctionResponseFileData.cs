using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// URI based data for function response.
/// </summary>
public sealed record FunctionResponseFileData
{
    /// <summary>
    /// Optional. Display name of the file.
    /// Used to provide a label or filename to distinguish file data, and to
    /// reference the file from <c>FunctionResponse.response</c> (for example, via
    /// <c>{"$ref": "DISPLAY_NAME"}</c>).
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Required. URI.
    /// </summary>
    [JsonPropertyName("fileUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FileUri { get; init; }

    /// <summary>
    /// The IANA standard MIME type of the source data.
    /// If an unsupported MIME type is provided, an error will be returned. For a
    /// complete list of supported types, see [Multimodal function
    /// responses](https://aistudio.google.com/docs/function-calling#multimodal)
    /// and [Supported file
    /// formats](https://aistudio.google.com/docs/file-input-methods#supported-content-types).
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }
}

