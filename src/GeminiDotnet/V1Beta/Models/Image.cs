using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Chunk from image search.
/// </summary>
public sealed record Image
{
    /// <summary>
    /// The root domain of the web page that the image is from, e.g.
    /// "example.com".
    /// </summary>
    [JsonPropertyName("domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Domain { get; init; }

    /// <summary>
    /// The image asset URL.
    /// </summary>
    [JsonPropertyName("imageUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ImageUri { get; init; }

    /// <summary>
    /// The web page URI for attribution.
    /// </summary>
    [JsonPropertyName("sourceUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? SourceUri { get; init; }

    /// <summary>
    /// The title of the web page that the image is from.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Title { get; init; }
}

