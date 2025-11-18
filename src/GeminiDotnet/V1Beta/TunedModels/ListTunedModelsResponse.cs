using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.TunedModels;

/// <summary>
/// Response from <c>ListTunedModels</c> containing a paginated list of Models.
/// </summary>
public sealed record ListTunedModelsResponse
{
    /// <summary>
    /// A token, which can be sent as <c>page_token</c> to retrieve the next page.
    /// If this field is omitted, there are no more pages.
    /// </summary>
    [JsonPropertyName("nextPageToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NextPageToken { get; init; }

    /// <summary>
    /// The returned Models.
    /// </summary>
    [JsonPropertyName("tunedModels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<TunedModel>? TunedModels { get; init; }
}

