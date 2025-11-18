using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Content that has been preprocessed and can be used in subsequent request
/// to GenerativeService.
/// Cached content can be only used with model it was created for.
/// </summary>
public sealed record CachedContent
{
    /// <summary>
    /// Optional. Input only. Immutable. The content to cache.
    /// </summary>
    [JsonPropertyName("contents")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Content>? Contents { get; init; }

    /// <summary>
    /// Output only. Creation time of the cache entry.
    /// </summary>
    [JsonPropertyName("createTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? CreateTime { get; init; }

    /// <summary>
    /// Optional. Immutable. The user-generated meaningful display name of the cached content. Maximum
    /// 128 Unicode characters.
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DisplayName { get; init; }

    /// <summary>
    /// Timestamp in UTC of when this resource is considered expired.
    /// This is *always* provided on output, regardless of what was sent
    /// on input.
    /// </summary>
    [JsonPropertyName("expireTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? ExpireTime { get; init; }

    /// <summary>
    /// Required. Immutable. The name of the <see cref="V1Beta.Models.Model"/> to use for cached content
    /// Format: <c>models/{model}</c>
    /// </summary>
    [JsonPropertyName("model")]
    public required string Model { get; init; }

    /// <summary>
    /// Output only. Identifier. The resource name referring to the cached content.
    /// Format: <c>cachedContents/{id}</c>
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; init; }

    /// <summary>
    /// Optional. Input only. Immutable. Developer set system instruction. Currently text only.
    /// </summary>
    [JsonPropertyName("systemInstruction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Content? SystemInstruction { get; init; }

    /// <summary>
    /// Optional. Input only. Immutable. Tool config. This config is shared for all tools.
    /// </summary>
    [JsonPropertyName("toolConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ToolConfiguration? ToolConfiguration { get; init; }

    /// <summary>
    /// Optional. Input only. Immutable. A list of <c>Tools</c> the model may use to generate the next response
    /// </summary>
    [JsonPropertyName("tools")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Tool>? Tools { get; init; }

    /// <summary>
    /// Input only. New TTL for this resource, input only.
    /// </summary>
    [JsonPropertyName("ttl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Ttl { get; init; }

    /// <summary>
    /// Output only. When the cache entry was last updated in UTC time.
    /// </summary>
    [JsonPropertyName("updateTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset? UpdateTime { get; init; }

    /// <summary>
    /// Output only. Metadata on the usage of the cached content.
    /// </summary>
    [JsonPropertyName("usageMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CachedContentUsageMetadata? UsageMetadata { get; init; }
}

