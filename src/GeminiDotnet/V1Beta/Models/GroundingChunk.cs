using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Grounding chunk.
/// </summary>
public sealed record GroundingChunk
{
    /// <summary>
    /// Optional. Grounding chunk from Google Maps.
    /// </summary>
    [JsonPropertyName("maps")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Maps? Maps { get; init; }

    /// <summary>
    /// Optional. Grounding chunk from context retrieved by the file search tool.
    /// </summary>
    [JsonPropertyName("retrievedContext")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RetrievedContext? RetrievedContext { get; init; }

    /// <summary>
    /// Grounding chunk from the web.
    /// </summary>
    [JsonPropertyName("web")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Web? Web { get; init; }
}

