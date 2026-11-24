using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Encapsulates a snippet of a user review that answers a question about
/// the features of a specific place in Google Maps.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record ReviewSnippet
{
    /// <summary>
    /// A link that corresponds to the user review on Google Maps.
    /// </summary>
    [JsonPropertyName("googleMapsUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? GoogleMapsUri { get; init; }

    /// <summary>
    /// The ID of the review snippet.
    /// </summary>
    [JsonPropertyName("reviewId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ReviewId { get; init; }

    /// <summary>
    /// Title of the review.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Title { get; init; }
}

