using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Response from <c>ListModel</c> containing a paginated list of Models.
/// </summary>
public sealed record ListModelsResponse
{
    /// <summary>
    /// The returned Models.
    /// </summary>
    [JsonPropertyName("models")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Model>? Models { get; init; }

    /// <summary>
    /// A token, which can be sent as <c>page_token</c> to retrieve the next page.
    /// If this field is omitted, there are no more pages.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }
}

