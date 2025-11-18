using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Response from <c>BatchUpdateChunks</c> containing a list of updated <see cref="V1Beta.Corpora.Chunk"/>s.
/// </summary>
public sealed record BatchUpdateChunksResponse
{
    /// <summary>
    /// <see cref="V1Beta.Corpora.Chunk"/>s updated.
    /// </summary>
    [JsonPropertyName("chunks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Chunk>? Chunks { get; init; }
}

