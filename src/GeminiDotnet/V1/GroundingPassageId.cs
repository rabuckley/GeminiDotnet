using System.Text.Json.Serialization;

namespace GeminiDotnet.V1;

/// <summary>
/// Identifier for a part within a <c>GroundingPassage</c>.
/// </summary>
public sealed record GroundingPassageId
{
    /// <summary>
    /// Output only. Index of the part within the <c>GenerateAnswerRequest</c>'s
    /// <c>GroundingPassage.content</c>.
    /// </summary>
    [JsonPropertyName("partIndex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? PartIndex { get; init; }

    /// <summary>
    /// Output only. ID of the passage matching the <c>GenerateAnswerRequest</c>'s
    /// <c>GroundingPassage.id</c>.
    /// </summary>
    [JsonPropertyName("passageId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? PassageId { get; init; }
}

