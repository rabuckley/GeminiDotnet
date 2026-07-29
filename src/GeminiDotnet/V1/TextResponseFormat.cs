using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Configuration for text output format.
/// </summary>
public sealed record TextResponseFormat
{
    /// <summary>
    /// Optional. The MIME type of the text output.
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TextResponseFormatMimeType? MimeType { get; init; }

    /// <summary>
    /// Optional. The JSON schema that the output should conform to. Only applicable when
    /// mime_type is APPLICATION_JSON.
    /// </summary>
    [JsonPropertyName("schema")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Schema { get; init; }
}

