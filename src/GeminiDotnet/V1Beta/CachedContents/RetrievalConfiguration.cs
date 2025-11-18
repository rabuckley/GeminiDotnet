using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.CachedContents;

/// <summary>
/// Retrieval config.
/// </summary>
public sealed record RetrievalConfiguration
{
    /// <summary>
    /// Optional. The language code of the user.
    /// Language code for content. Use language tags defined by
    /// [BCP47](https://www.rfc-editor.org/rfc/bcp/bcp47.txt).
    /// </summary>
    [JsonPropertyName("languageCode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? LanguageCode { get; init; }

    /// <summary>
    /// Optional. The location of the user.
    /// </summary>
    [JsonPropertyName("latLng")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LatLng? LatLng { get; init; }
}

