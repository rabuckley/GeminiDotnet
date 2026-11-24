using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// A response from <c>CountMessageTokens</c>.
/// It returns the model's <c>token_count</c> for the <c>prompt</c>.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public sealed record CountMessageTokensResponse
{
    /// <summary>
    /// The number of tokens that the <c>model</c> tokenizes the <c>prompt</c> into.
    /// Always non-negative.
    /// </summary>
    [JsonPropertyName("tokenCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? TokenCount { get; init; }
}

