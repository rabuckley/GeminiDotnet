using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// A grounding chunk from Google Maps. A Maps chunk corresponds to a single
/// place.
/// </summary>
public sealed record Maps
{
    /// <summary>
    /// Sources that provide answers about the features of a given place in
    /// Google Maps.
    /// </summary>
    [JsonPropertyName("placeAnswerSources")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PlaceAnswerSources? PlaceAnswerSources { get; init; }

    /// <summary>
    /// This ID of the place, in <c>places/{place_id}</c> format. A user can use this
    /// ID to look up that place.
    /// </summary>
    [JsonPropertyName("placeId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? PlaceId { get; init; }

    /// <summary>
    /// Text description of the place answer.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Text { get; init; }

    /// <summary>
    /// Title of the place.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Title { get; init; }

    /// <summary>
    /// URI reference of the place.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Uri { get; init; }
}

