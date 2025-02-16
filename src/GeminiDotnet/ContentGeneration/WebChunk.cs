using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Chunk from the web.
/// </summary>
public sealed record WebChunk
{
    /// <summary>
    /// URI reference of the chunk.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    /// <summary>
    /// Title of the chunk.
    /// </summary>
    [JsonPropertyName("title")]
    public required string Title { get; init; }
}
