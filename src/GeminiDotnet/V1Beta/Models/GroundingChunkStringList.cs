using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// A list of string values.
/// </summary>
public sealed record GroundingChunkStringList
{
    /// <summary>
    /// The string values of the list.
    /// </summary>
    [JsonPropertyName("values")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? Values { get; init; }
}

