using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// A <see cref="V1Beta.Document"/> is a collection of <see cref="V1Beta.Corpora.Chunk"/>s.
/// </summary>
public sealed record Document
{
    /// <summary>
    /// Output only. The Timestamp of when the <see cref="V1Beta.Document"/> was created.
    /// </summary>
    [JsonPropertyName("createTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? CreateTime { get; init; }

    /// <summary>
    /// Optional. User provided custom metadata stored as key-value pairs used for querying.
    /// A <see cref="V1Beta.Document"/> can have a maximum of 20 <see cref="V1Beta.CustomMetadata"/>.
    /// </summary>
    [JsonPropertyName("customMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<CustomMetadata>? CustomMetadata { get; init; }

    /// <summary>
    /// Optional. The human-readable display name for the <see cref="V1Beta.Document"/>. The display name must
    /// be no more than 512 characters in length, including spaces.
    /// Example: "Semantic Retriever Documentation"
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Output only. The mime type of the Document.
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MimeType { get; init; }

    /// <summary>
    /// Immutable. Identifier. The <see cref="V1Beta.Document"/> resource name. The ID (name excluding the
    /// "fileSearchStores/*/documents/" prefix) can contain up to 40 characters
    /// that are lowercase alphanumeric or dashes (-). The ID cannot start or end
    /// with a dash. If the name is empty on create, a unique name will be derived
    /// from <c>display_name</c> along with a 12 character random suffix. Example:
    /// <c>fileSearchStores/{file_search_store_id}/documents/my-awesome-doc-123a456b789c</c>
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// Output only. The size of raw bytes ingested into the Document.
    /// </summary>
    [JsonPropertyName("sizeBytes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? SizeBytes { get; init; }

    /// <summary>
    /// Output only. Current state of the <see cref="V1Beta.Document"/>.
    /// </summary>
    [JsonPropertyName("state")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentState? State { get; init; }

    /// <summary>
    /// Output only. The Timestamp of when the <see cref="V1Beta.Document"/> was last updated.
    /// </summary>
    [JsonPropertyName("updateTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? UpdateTime { get; init; }
}

