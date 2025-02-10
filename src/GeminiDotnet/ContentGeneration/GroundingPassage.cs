using System.Text.Json.Serialization;

namespace GeminiDotnet.ContentGeneration;

/// <summary>
/// Identifier for an inline passage.
/// </summary>
public sealed record GroundingPassage
{
    /// <summary>
    /// ID of the passage matching the GenerateAnswerRequest's GroundingPassage.id.
    /// </summary>
    [JsonPropertyName("passageId")]
    public required string PassageId { get; init; }

    /// <summary>
    /// Index of the part within the GenerateAnswerRequest's GroundingPassage.content.
    /// </summary>
    [JsonPropertyName("partIndex")]
    public required int PartIndex { get; init; }
}