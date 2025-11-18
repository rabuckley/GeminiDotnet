using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.Corpora;

/// <summary>
/// Response from <c>BatchCreateChunks</c> containing a list of created <see cref="V1Beta.Corpora.Chunk"/>s.
/// </summary>
public sealed record BatchCreateChunksResponse
{
    /// <summary>
    /// <see cref="V1Beta.Corpora.Chunk"/>s created.
    /// </summary>
    [JsonPropertyName("chunks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<Chunk>? Chunks { get; init; }
}

