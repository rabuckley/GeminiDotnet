using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// A repeated list of passages.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record GroundingPassages
{
    /// <summary>
    /// List of passages.
    /// </summary>
    [JsonPropertyName("passages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<GroundingPassage>? Passages { get; init; }
}

