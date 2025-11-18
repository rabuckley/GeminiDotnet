using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Metadata related to url context retrieval tool.
/// </summary>
public sealed record UrlContextMetadata
{
    /// <summary>
    /// List of url context.
    /// </summary>
    [JsonPropertyName("urlMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<UrlMetadata>? UrlMetadata { get; init; }
}

