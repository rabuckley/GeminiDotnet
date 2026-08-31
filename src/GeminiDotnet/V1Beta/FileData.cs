using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// URI based data.
/// </summary>
public sealed record FileData
{
    /// <summary>
    /// Optional. Specifies the name used to refer to this file to the model (e.g.
    /// "my_file.pdf"). Used as the file reference identifier when
    /// <c>verbalization_mode</c> is set to <c>REFERENCE_ONLY</c>.
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DisplayName { get; init; }

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

