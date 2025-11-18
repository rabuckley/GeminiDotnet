using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Passage included inline with a grounding configuration.
/// </summary>
public sealed record GroundingPassage
{
    /// <summary>
    /// Content of the passage.
    /// </summary>
    [JsonPropertyName("content")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Content? Content { get; init; }

    /// <summary>
    /// Identifier for the passage for attributing this passage in grounded
    /// answers.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Id { get; init; }
}

