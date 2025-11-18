using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// A <see cref="V1Beta.Corpora.Corpus"/> is a collection of <see cref="V1Beta.Document"/>s.
/// A project can create up to 10 corpora.
/// </summary>
public sealed record Corpus
{
    /// <summary>
    /// Output only. The Timestamp of when the <see cref="V1Beta.Corpora.Corpus"/> was created.
    /// </summary>
    [JsonPropertyName("createTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? CreateTime { get; init; }

    /// <summary>
    /// Optional. The human-readable display name for the <see cref="V1Beta.Corpora.Corpus"/>. The display name must be
    /// no more than 512 characters in length, including spaces.
    /// Example: "Docs on Semantic Retriever"
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Output only. Immutable. Identifier. The <see cref="V1Beta.Corpora.Corpus"/> resource name. The ID (name excluding the "corpora/" prefix)
    /// can contain up to 40 characters that are lowercase alphanumeric or dashes
    /// (-). The ID cannot start or end with a dash. If the name is empty on
    /// create, a unique name will be derived from <c>display_name</c> along with a 12
    /// character random suffix.
    /// Example: <c>corpora/my-awesome-corpora-123a456b789c</c>
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// Output only. The Timestamp of when the <see cref="V1Beta.Corpora.Corpus"/> was last updated.
    /// </summary>
    [JsonPropertyName("updateTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? UpdateTime { get; init; }
}

