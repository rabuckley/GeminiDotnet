using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// URI based data.
/// </summary>
public sealed record FileData
{
    /// <summary>
    /// Required. URI.
    /// </summary>
    [JsonPropertyName("fileUri")]
    public required string FileUri { get; init; }

    /// <summary>
    /// Optional. The IANA standard MIME type of the source data.
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }
}

