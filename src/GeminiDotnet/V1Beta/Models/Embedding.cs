using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Models;

/// <summary>
/// A list of floats representing the embedding.
/// </summary>
public sealed record Embedding
{
    /// <summary>
    /// The embedding values.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<float> Value { get; init; }
}

