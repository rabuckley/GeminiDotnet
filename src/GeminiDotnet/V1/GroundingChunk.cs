using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Grounding chunk.
/// </summary>
public sealed record GroundingChunk
{
    /// <summary>
    /// Grounding chunk from the web.
    /// </summary>
    [JsonPropertyName("web")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Web? Web { get; init; }
}

