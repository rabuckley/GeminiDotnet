using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Request for querying a <see cref="V1Beta.Document"/>.
/// </summary>
public sealed record QueryDocumentRequest
{
    /// <summary>
    /// Optional. Filter for <see cref="V1Beta.Corpora.Chunk"/> metadata. Each <see cref="V1Beta.MetadataFilter"/> object should
    /// correspond to a unique key. Multiple <see cref="V1Beta.MetadataFilter"/> objects are joined by
    /// logical "AND"s.
    /// Note: <see cref="V1Beta.Document"/>-level filtering is not supported for this request because
    /// a <see cref="V1Beta.Document"/> name is already specified.
    /// Example query:
    /// (year >= 2020 OR year < 2010) AND (genre = drama OR genre = action)
    /// <see cref="V1Beta.MetadataFilter"/> object list:
    /// metadata_filters = [
    /// {key = "chunk.custom_metadata.year"
    /// conditions = [{int_value = 2020, operation = GREATER_EQUAL},
    /// {int_value = 2010, operation = LESS}},
    /// {key = "chunk.custom_metadata.genre"
    /// conditions = [{string_value = "drama", operation = EQUAL},
    /// {string_value = "action", operation = EQUAL}}]
    /// Example query for a numeric range of values:
    /// (year > 2015 AND year <= 2020)
    /// <see cref="V1Beta.MetadataFilter"/> object list:
    /// metadata_filters = [
    /// {key = "chunk.custom_metadata.year"
    /// conditions = [{int_value = 2015, operation = GREATER}]},
    /// {key = "chunk.custom_metadata.year"
    /// conditions = [{int_value = 2020, operation = LESS_EQUAL}]}]
    /// Note: "AND"s for the same key are only supported for numeric values. String
    /// values only support "OR"s for the same key.
    /// </summary>
    [JsonPropertyName("metadataFilters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<MetadataFilter>? MetadataFilters { get; init; }

    /// <summary>
    /// Required. Query string to perform semantic search.
    /// </summary>
    [JsonPropertyName("query")]
    public required string Query { get; init; }

    /// <summary>
    /// Optional. The maximum number of <see cref="V1Beta.Corpora.Chunk"/>s to return.
    /// The service may return fewer <see cref="V1Beta.Corpora.Chunk"/>s.
    /// If unspecified, at most 10 <see cref="V1Beta.Corpora.Chunk"/>s will be returned.
    /// The maximum specified result count is 100.
    /// </summary>
    [JsonPropertyName("resultsCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? ResultsCount { get; init; }
}

