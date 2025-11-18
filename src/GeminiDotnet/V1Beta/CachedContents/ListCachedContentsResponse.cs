using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Response with CachedContents list.
/// </summary>
public sealed record ListCachedContentsResponse
{
    /// <summary>
    /// List of cached contents.
    /// </summary>
    [JsonPropertyName("cachedContents")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<CachedContent>? CachedContents { get; init; }

    /// <summary>
    /// A token, which can be sent as <c>page_token</c> to retrieve the next page.
    /// If this field is omitted, there are no subsequent pages.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }
}

