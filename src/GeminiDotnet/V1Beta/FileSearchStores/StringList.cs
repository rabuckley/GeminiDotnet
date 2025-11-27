using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// User provided string values assigned to a single metadata key.
/// </summary>
public sealed record StringList
{
    /// <summary>
    /// The string values of the metadata to store.
    /// </summary>
    [JsonPropertyName("values")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<string>? Values { get; init; }
}

