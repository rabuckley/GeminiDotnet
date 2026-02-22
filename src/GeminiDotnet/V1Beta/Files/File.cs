using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Files;

/// <summary>
/// A file uploaded to the API.
/// Next ID: 15
/// </summary>
public sealed record File
{
    /// <summary>
    /// Output only. The timestamp of when the <see cref="V1Beta.Files.File"/> was created.
    /// </summary>
    [JsonPropertyName("createTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? CreateTime { get; init; }

    /// <summary>
    /// Optional. The human-readable display name for the <see cref="V1Beta.Files.File"/>. The display name must be
    /// no more than 512 characters in length, including spaces.
    /// Example: "Welcome Image"
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Output only. The download uri of the <see cref="V1Beta.Files.File"/>.
    /// </summary>
    [JsonPropertyName("downloadUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DownloadUri { get; init; }

    /// <summary>
    /// Output only. Error status if File processing failed.
    /// </summary>
    [JsonPropertyName("error")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Status? Error { get; init; }

    /// <summary>
    /// Output only. The timestamp of when the <see cref="V1Beta.Files.File"/> will be deleted. Only set if the <see cref="V1Beta.Files.File"/> is
    /// scheduled to expire.
    /// </summary>
    [JsonPropertyName("expirationTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? ExpirationTime { get; init; }

    /// <summary>
    /// Output only. MIME type of the file.
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }

    /// <summary>
    /// Immutable. Identifier. The <see cref="V1Beta.Files.File"/> resource name. The ID (name excluding the "files/" prefix) can
    /// contain up to 40 characters that are lowercase alphanumeric or dashes (-).
    /// The ID cannot start or end with a dash. If the name is empty on create, a
    /// unique name will be generated.
    /// Example: <c>files/123-456</c>
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// Output only. SHA-256 hash of the uploaded bytes.
    /// </summary>
    [JsonPropertyName("sha256Hash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> Sha256Hash { get; init; }

    /// <summary>
    /// Output only. Size of the file in bytes.
    /// </summary>
    [JsonPropertyName("sizeBytes")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? SizeBytes { get; init; }

    /// <summary>
    /// Source of the File.
    /// </summary>
    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileSource? Source { get; init; }

    /// <summary>
    /// Output only. Processing state of the File.
    /// </summary>
    [JsonPropertyName("state")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileState? State { get; init; }

    /// <summary>
    /// Output only. The timestamp of when the <see cref="V1Beta.Files.File"/> was last updated.
    /// </summary>
    [JsonPropertyName("updateTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? UpdateTime { get; init; }

    /// <summary>
    /// Output only. The uri of the <see cref="V1Beta.Files.File"/>.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Uri { get; init; }

    /// <summary>
    /// Output only. Metadata for a video.
    /// </summary>
    [JsonPropertyName("videoMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public VideoFileMetadata? VideoMetadata { get; init; }
}

