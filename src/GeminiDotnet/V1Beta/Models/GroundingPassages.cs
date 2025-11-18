using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// A repeated list of passages.
/// </summary>
public sealed record GroundingPassages
{
    /// <summary>
    /// List of passages.
    /// </summary>
    [JsonPropertyName("passages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<GroundingPassage>? Passages { get; init; }
}

