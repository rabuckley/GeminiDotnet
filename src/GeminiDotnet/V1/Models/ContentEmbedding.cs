using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.Models;

/// <summary>
/// A list of floats representing an embedding.
/// </summary>
public sealed record ContentEmbedding
{
    /// <summary>
    /// The embedding values.
    /// </summary>
    [JsonPropertyName("values")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<float> Values { get; init; }
}

