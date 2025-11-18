using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta;

/// <summary>
/// User provided filter to limit retrieval based on <see cref="V1Beta.Corpora.Chunk"/> or <see cref="V1Beta.Document"/> level
/// metadata values.
/// Example (genre = drama OR genre = action):
/// key = "document.custom_metadata.genre"
/// conditions = [{string_value = "drama", operation = EQUAL},
/// {string_value = "action", operation = EQUAL}]
/// </summary>
public sealed record MetadataFilter
{
    /// <summary>
    /// Required. The <see cref="V1Beta.Condition"/>s for the given key that will trigger this filter. Multiple
    /// <see cref="V1Beta.Condition"/>s are joined by logical ORs.
    /// </summary>
    [JsonPropertyName("conditions")]
    public required IReadOnlyList<Condition> Conditions { get; init; }

    /// <summary>
    /// Required. The key of the metadata to filter on.
    /// </summary>
    [JsonPropertyName("key")]
    public required string Key { get; init; }
}

