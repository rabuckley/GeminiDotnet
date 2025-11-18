using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Response from <c>ListCorpora</c> containing a paginated list of <c>Corpora</c>.
/// The results are sorted by ascending <c>corpus.create_time</c>.
/// </summary>
public sealed record ListCorporaResponse
{
    /// <summary>
    /// The returned corpora.
    /// </summary>
    [JsonPropertyName("corpora")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Corpus>? Corpora { get; init; }

    /// <summary>
    /// A token, which can be sent as <c>page_token</c> to retrieve the next page.
    /// If this field is omitted, there are no more pages.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }
}

