using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Response from <c>ListChunks</c> containing a paginated list of <see cref="V1Beta.Corpora.Chunk"/>s.
/// The <see cref="V1Beta.Corpora.Chunk"/>s are sorted by ascending <c>chunk.create_time</c>.
/// </summary>
public sealed record ListChunksResponse
{
    /// <summary>
    /// The returned <see cref="V1Beta.Corpora.Chunk"/>s.
    /// </summary>
    [JsonPropertyName("chunks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Chunk>? Chunks { get; init; }

    /// <summary>
    /// A token, which can be sent as <c>page_token</c> to retrieve the next page.
    /// If this field is omitted, there are no more pages.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }
}

