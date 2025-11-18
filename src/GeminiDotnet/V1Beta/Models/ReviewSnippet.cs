using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Encapsulates a snippet of a user review that answers a question about
/// the features of a specific place in Google Maps.
/// </summary>
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

