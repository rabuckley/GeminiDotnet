using System.Text.Json.Serialization;

namespace GeminiDotnet;

/// <summary>
/// An error response from the Gemini API.
/// </summary>
public sealed record ErrorResponse
{
    /// <summary>
    /// The error details.
    /// </summary>
    [JsonPropertyName("error")]
    public required ErrorDetails Error { get; init; }
}
