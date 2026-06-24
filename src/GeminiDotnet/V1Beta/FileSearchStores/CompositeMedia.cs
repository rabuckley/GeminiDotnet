using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// A sequence of media data references representing composite data.
/// Introduced to support Bigstore composite objects. For details, visit
/// http://go/bigstore-composites.
/// </summary>
public sealed record CompositeMedia
{
    /// <summary>
    /// Blobstore v1 reference, set if reference_type is BLOBSTORE_REF
    /// This should be the byte representation of a blobstore.BlobRef.
    /// Since Blobstore is deprecating v1, use blobstore2_info instead.
    /// For now, any v2 blob will also be represented in this field as
    /// v1 BlobRef.
    /// </summary>
    [JsonPropertyName("blobRef")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> BlobRef { get; init; }

    /// <summary>
    /// Blobstore v2 info, set if reference_type is BLOBSTORE_REF and it refers
    /// to a v2 blob.
    /// </summary>
    [JsonPropertyName("blobstore2Info")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Blobstore2Info? Blobstore2Info { get; init; }

    /// <summary>
    /// A binary data reference for a media download. Serves as a
    /// technology-agnostic binary reference in some Google infrastructure.
    /// This value is a serialized storage_cosmo.BinaryReference proto. Storing
    /// it as bytes is a hack to get around the fact that the cosmo proto
    /// (as well as others it includes) doesn't support JavaScript. This
    /// prevents us from including the actual type of this field.
    /// </summary>
    [JsonPropertyName("cosmoBinaryReference")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> CosmoBinaryReference { get; init; }

    /// <summary>
    /// crc32.c hash for the payload.
    /// </summary>
    [JsonPropertyName("crc32cHash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? Crc32cHash { get; init; }

    /// <summary>
    /// Media data, set if reference_type is INLINE
    /// </summary>
    [JsonPropertyName("inline")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> Inline { get; init; }

    /// <summary>
    /// Size of the data, in bytes
    /// </summary>
    [JsonPropertyName("length")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? Length { get; init; }

    /// <summary>
    /// MD5 hash for the payload.
    /// </summary>
    [JsonPropertyName("md5Hash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> Md5Hash { get; init; }

    /// <summary>
    /// Reference to a TI Blob, set if reference_type is BIGSTORE_REF.
    /// </summary>
    [JsonPropertyName("objectId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ObjectId? ObjectId { get; init; }

    /// <summary>
    /// Path to the data, set if reference_type is PATH
    /// </summary>
    [JsonPropertyName("path")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Path { get; init; }

    /// <summary>
    /// Describes what the field reference contains.
    /// </summary>
    [JsonPropertyName("referenceType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompositeMediaReferenceType? ReferenceType { get; init; }

    /// <summary>
    /// SHA-1 hash for the payload.
    /// </summary>
    [JsonPropertyName("sha1Hash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> Sha1Hash { get; init; }
}

