using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// URI based data for function response.
/// </summary>
public sealed record FunctionResponseFileData
{
    /// <summary>
    /// Optional. Display name of the file.
    /// Used to provide a label or filename to distinguish file data.
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
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }
}

