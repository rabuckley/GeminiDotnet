using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// A <see cref="V1Beta.FileSearchStores.FileSearchStore"/> is a collection of <see cref="V1Beta.FileSearchStores.Document"/>s.
/// </summary>
public sealed record FileSearchStore
{
    /// <summary>
    /// Output only. The number of documents in the <see cref="V1Beta.FileSearchStores.FileSearchStore"/> that are active and ready
    /// for retrieval.
    /// </summary>
    [JsonPropertyName("activeDocumentsCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? ActiveDocumentsCount { get; init; }

    /// <summary>
    /// Output only. The Timestamp of when the <see cref="V1Beta.FileSearchStores.FileSearchStore"/> was created.
    /// </summary>
    [JsonPropertyName("createTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? CreateTime { get; init; }

    /// <summary>
    /// Optional. The human-readable display name for the <see cref="V1Beta.FileSearchStores.FileSearchStore"/>. The display name
    /// must be no more than 512 characters in length, including spaces. Example:
    /// "Docs on Semantic Retriever"
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Output only. The number of documents in the <see cref="V1Beta.FileSearchStores.FileSearchStore"/> that have failed
    /// processing.
    /// </summary>
    [JsonPropertyName("failedDocumentsCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? FailedDocumentsCount { get; init; }

    /// <summary>
    /// Output only. Immutable. Identifier. The <see cref="V1Beta.FileSearchStores.FileSearchStore"/> resource name. It is an ID (name excluding the
    /// "fileSearchStores/" prefix) that can contain up to 40 characters that are
    /// lowercase alphanumeric or dashes
    /// (-). It is output only. The unique name will be derived from
    /// <c>display_name</c> along with a 12 character random suffix. Example:
    /// <c>fileSearchStores/my-awesome-file-search-store-123a456b789c</c>
    /// If <c>display_name</c> is not provided, the name will be randomly generated.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// Output only. The number of documents in the <see cref="V1Beta.FileSearchStores.FileSearchStore"/> that are being processed.
    /// </summary>
    [JsonPropertyName("pendingDocumentsCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? PendingDocumentsCount { get; init; }

    /// <summary>
    /// Output only. The size of raw bytes ingested into the <see cref="V1Beta.FileSearchStores.FileSearchStore"/>. This is the
    /// total size of all the documents in the <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    [JsonPropertyName("sizeBytes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? SizeBytes { get; init; }

    /// <summary>
    /// Output only. The Timestamp of when the <see cref="V1Beta.FileSearchStores.FileSearchStore"/> was last updated.
    /// </summary>
    [JsonPropertyName("updateTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? UpdateTime { get; init; }
}

