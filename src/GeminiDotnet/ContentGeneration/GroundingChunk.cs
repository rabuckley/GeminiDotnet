using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Grounding chunk.
/// </summary>
public sealed record GroundingChunk
{
    /// <summary>
    /// Grounding chunk from the web.
    /// </summary>
    [JsonPropertyName("web")]
    public WebChunk? Web { get; init; }
}