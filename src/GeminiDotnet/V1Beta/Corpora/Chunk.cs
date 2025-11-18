using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// A <see cref="V1Beta.Corpora.Chunk"/> is a subpart of a <see cref="V1Beta.Document"/> that is treated as an independent unit
/// for the purposes of vector representation and storage.
/// </summary>
public sealed record Chunk
{
    /// <summary>
    /// Output only. The Timestamp of when the <see cref="V1Beta.Corpora.Chunk"/> was created.
    /// </summary>
    [JsonPropertyName("createTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? CreateTime { get; init; }

    /// <summary>
    /// Optional. User provided custom metadata stored as key-value pairs.
    /// The maximum number of <see cref="V1Beta.CustomMetadata"/> per chunk is 20.
    /// </summary>
    [JsonPropertyName("customMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<CustomMetadata>? CustomMetadata { get; init; }

    /// <summary>
    /// Required. The content for the <see cref="V1Beta.Corpora.Chunk"/>, such as the text string.
    /// The maximum number of tokens per chunk is 2043.
    /// </summary>
    [JsonPropertyName("data")]
    public required ChunkData Data { get; init; }

    /// <summary>
    /// Immutable. Identifier. The <see cref="V1Beta.Corpora.Chunk"/> resource name. The ID (name excluding the
    /// "corpora/*/documents/*/chunks/" prefix) can contain up to 40 characters
    /// that are lowercase alphanumeric or dashes (-). The ID cannot start or end
    /// with a dash. If the name is empty on create, a random 12-character unique
    /// ID will be generated.
    /// Example: <c>corpora/{corpus_id}/documents/{document_id}/chunks/123a456b789c</c>
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// Output only. Current state of the <see cref="V1Beta.Corpora.Chunk"/>.
    /// </summary>
    [JsonPropertyName("state")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ChunkState? State { get; init; }

    /// <summary>
    /// Output only. The Timestamp of when the <see cref="V1Beta.Corpora.Chunk"/> was last updated.
    /// </summary>
    [JsonPropertyName("updateTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? UpdateTime { get; init; }
}

