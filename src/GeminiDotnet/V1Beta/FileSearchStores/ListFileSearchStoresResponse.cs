using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Response from <c>ListFileSearchStores</c> containing a paginated list of
/// <c>FileSearchStores</c>. The results are sorted by ascending
/// <c>file_search_store.create_time</c>.
/// </summary>
public sealed record ListFileSearchStoresResponse
{
    /// <summary>
    /// The returned rag_stores.
    /// </summary>
    [JsonPropertyName("fileSearchStores")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<FileSearchStore>? FileSearchStores { get; init; }

    /// <summary>
    /// A token, which can be sent as <c>page_token</c> to retrieve the next page.
    /// If this field is omitted, there are no more pages.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }
}

