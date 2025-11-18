using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Extracted data that represents the <see cref="V1Beta.Corpora.Chunk"/> content.
/// </summary>
public sealed record ChunkData
{
    /// <summary>
    /// The <see cref="V1Beta.Corpora.Chunk"/> content as a string.
    /// The maximum number of tokens per chunk is 2043.
    /// </summary>
    [JsonPropertyName("stringValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? StringValue { get; init; }
}

