using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Environments;

/// <summary>
/// Metadata for a file or directory within an environment.
/// </summary>
public sealed record EnvironmentFile
{
    /// <summary>
    /// Output only. The creation time of the file/directory.
    /// </summary>
    [JsonPropertyName("created")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? Created { get; init; }

    /// <summary>
    /// Output only. The MIME type of the file (e.g., "text/python", "image/png").
    /// Empty for directories.
    /// NOLINT
    /// </summary>
    [JsonPropertyName("mime_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }

    /// <summary>
    /// Output only. The modification time of the file/directory.
    /// </summary>
    [JsonPropertyName("modified")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? Modified { get; init; }

    /// <summary>
    /// Output only. The name of the file or directory (e.g., "main.py" or "src").
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// Output only. The full relative path within the environment
    /// (e.g., "workspace/src/main.py").
    /// </summary>
    [JsonPropertyName("path")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Path { get; init; }

    /// <summary>
    /// Output only. The size of the file/directory in bytes.
    /// NOLINT
    /// </summary>
    [JsonPropertyName("size_bytes")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? SizeBytes { get; init; }

    /// <summary>
    /// Output only. The type of the entry.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EnvironmentFileType? Type { get; init; }
}

