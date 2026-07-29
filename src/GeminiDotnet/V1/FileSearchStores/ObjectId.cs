using System.Text.Json.Serialization;

namespace GeminiDotnet.V1.FileSearchStores;

/// <summary>
/// This is a copy of the tech.blob.ObjectId proto, which could not
/// be used directly here due to transitive closure issues with
/// JavaScript support; see http://b/8801763.
/// </summary>
public sealed record ObjectId
{
    /// <summary>
    /// The name of the bucket to which this object belongs.
    /// </summary>
    [JsonPropertyName("bucketName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? BucketName { get; init; }

    /// <summary>
    /// Generation of the object. Generations are monotonically increasing
    /// across writes, allowing them to be be compared to determine which
    /// generation is newer. If this is omitted in a request, then you are
    /// requesting the live object.
    /// See http://go/bigstore-versions
    /// </summary>
    [JsonPropertyName("generation")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? Generation { get; init; }

    /// <summary>
    /// The name of the object.
    /// </summary>
    [JsonPropertyName("objectName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ObjectName { get; init; }
}

