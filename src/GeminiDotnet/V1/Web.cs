using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Chunk from the web.
/// </summary>
public sealed record Web
{
    /// <summary>
    /// Title of the chunk.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Title { get; init; }

    /// <summary>
    /// URI reference of the chunk.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Uri { get; init; }
}

