using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// URI-based data with a specified media type.
/// </summary>
public sealed record FileData
{
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MimeType { get; init; }

    [JsonPropertyName("fileUri")]
    public required Uri Uri { get; init; }
}
