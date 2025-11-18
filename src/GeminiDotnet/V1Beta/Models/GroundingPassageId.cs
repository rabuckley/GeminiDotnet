using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// Identifier for a part within a <see cref="V1Beta.Models.GroundingPassage"/>.
/// </summary>
public sealed record GroundingPassageId
{
    /// <summary>
    /// Output only. Index of the part within the <see cref="V1Beta.Models.GenerateAnswerRequest"/>'s
    /// <c>GroundingPassage.content</c>.
    /// </summary>
    [JsonPropertyName("partIndex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? PartIndex { get; init; }

    /// <summary>
    /// Output only. ID of the passage matching the <see cref="V1Beta.Models.GenerateAnswerRequest"/>'s
    /// <c>GroundingPassage.id</c>.
    /// </summary>
    [JsonPropertyName("passageId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? PassageId { get; init; }
}

