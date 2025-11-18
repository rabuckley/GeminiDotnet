using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Collection of sources that provide answers about the features of a given
/// place in Google Maps. Each PlaceAnswerSources message corresponds to a
/// specific place in Google Maps. The Google Maps tool used these sources in
/// order to answer questions about features of the place (e.g: "does Bar Foo
/// have Wifi" or "is Foo Bar wheelchair accessible?"). Currently we only
/// support review snippets as sources.
/// </summary>
public sealed record PlaceAnswerSources
{
    /// <summary>
    /// Snippets of reviews that are used to generate answers about the
    /// features of a given place in Google Maps.
    /// </summary>
    [JsonPropertyName("reviewSnippets")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<ReviewSnippet>? ReviewSnippets { get; init; }
}

