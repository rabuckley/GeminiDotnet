using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Metadata on the usage of the cached content.
/// </summary>
public sealed record CachedContentUsageMetadata
{
    /// <summary>
    /// Total number of tokens that the cached content consumes.
    /// </summary>
    [JsonPropertyName("totalTokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TotalTokenCount { get; init; }
}

