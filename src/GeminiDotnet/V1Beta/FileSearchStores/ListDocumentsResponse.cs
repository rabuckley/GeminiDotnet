using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Response from <c>ListDocuments</c> containing a paginated list of <see cref="V1Beta.FileSearchStores.Document"/>s.
/// The <see cref="V1Beta.FileSearchStores.Document"/>s are sorted by ascending <c>document.create_time</c>.
/// </summary>
public sealed record ListDocumentsResponse
{
    /// <summary>
    /// The returned <see cref="V1Beta.FileSearchStores.Document"/>s.
    /// </summary>
    [JsonPropertyName("documents")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Document>? Documents { get; init; }

    /// <summary>
    /// A token, which can be sent as <c>page_token</c> to retrieve the next page.
    /// If this field is omitted, there are no more pages.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }
}

