using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// A <see cref="V1Beta.Models.GroundingChunk"/> represents a segment of supporting evidence that grounds
/// the model's response. It can be a chunk from the web, a retrieved context
/// from a file, or information from Google Maps.
/// </summary>
public sealed record GroundingChunk
{
    /// <summary>
    /// Optional. Grounding chunk from image search.
    /// </summary>
    [JsonPropertyName("image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Image? Image { get; init; }

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

