using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// Metadata related to url context retrieval tool.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record UrlContextMetadata
{
    /// <summary>
    /// List of url context.
    /// </summary>
    [JsonPropertyName("urlMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<UrlMetadata>? UrlMetadata { get; init; }
}

