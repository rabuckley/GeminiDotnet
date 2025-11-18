using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Parameters for telling the service how to chunk the file.
/// inspired by
/// google3/cloud/ai/platform/extension/lib/retrieval/config/chunker_config.proto
/// </summary>
public sealed record ChunkingConfiguration
{
    /// <summary>
    /// White space chunking configuration.
    /// </summary>
    [JsonPropertyName("whiteSpaceConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WhiteSpaceConfiguration? WhiteSpaceConfiguration { get; init; }
}

