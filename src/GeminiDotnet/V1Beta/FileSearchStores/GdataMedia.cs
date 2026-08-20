using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// A reference to data stored on the filesystem, on GFS or in blobstore.
/// </summary>
public sealed record GdataMedia
{
    /// <summary>
    /// Deprecated, use one of explicit hash type fields instead.
    /// Algorithm used for calculating the hash.
    /// As of 2011/01/21, "MD5" is the only possible value for this field.
    /// New values may be added at any time.
    /// </summary>
    [Obsolete("Use one of explicit hash type fields instead.")]
    [JsonPropertyName("algorithm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Algorithm { get; init; }

    /// <summary>
    /// Use object_id instead.
    /// </summary>
    [Obsolete]
    [JsonPropertyName("bigstoreObjectRef")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> BigstoreObjectRef { get; init; }

    /// <summary>
    /// Blobstore v1 reference, set if reference_type is BLOBSTORE_REF
    /// This should be the byte representation of a blobstore.BlobRef.
    /// Since Blobstore is deprecating v1, use blobstore2_info instead.
    /// For now, any v2 blob will also be represented in this field as
    /// v1 BlobRef.
    /// </summary>
    [Obsolete]
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
    /// A composite media composed of one or more media objects, set if
    /// reference_type is COMPOSITE_MEDIA. The media length field must be set
    /// to the sum of the lengths of all composite media objects.
    /// Note: All composite media must have length specified.
    /// </summary>
    [JsonPropertyName("compositeMedia")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyList<CompositeMedia>? CompositeMedia { get; init; }

    /// <summary>
    /// MIME type of the data
    /// </summary>
    [JsonPropertyName("contentType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ContentType { get; init; }

    /// <summary>
    /// Extended content type information provided for Scotty uploads.
    /// </summary>
    [JsonPropertyName("contentTypeInfo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ContentTypeInfo? ContentTypeInfo { get; init; }

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
    /// For Scotty Uploads:
    /// Scotty-provided hashes for uploads
    /// For Scotty Downloads:
    /// (WARNING: DO NOT USE WITHOUT PERMISSION FROM THE SCOTTY TEAM.)
    /// A Hash provided by the agent to be used to verify the data being
    /// downloaded. Currently only supported for inline payloads.
    /// Further, only crc32c_hash is currently supported.
    /// </summary>
    [JsonPropertyName("crc32cHash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? Crc32cHash { get; init; }

    /// <summary>
    /// Set if reference_type is DIFF_CHECKSUMS_RESPONSE.
    /// </summary>
    [JsonPropertyName("diffChecksumsResponse")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DiffChecksumsResponse? DiffChecksumsResponse { get; init; }

    /// <summary>
    /// Set if reference_type is DIFF_DOWNLOAD_RESPONSE.
    /// </summary>
    [JsonPropertyName("diffDownloadResponse")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DiffDownloadResponse? DiffDownloadResponse { get; init; }

    /// <summary>
    /// Set if reference_type is DIFF_UPLOAD_REQUEST.
    /// </summary>
    [JsonPropertyName("diffUploadRequest")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DiffUploadRequest? DiffUploadRequest { get; init; }

    /// <summary>
    /// Set if reference_type is DIFF_UPLOAD_RESPONSE.
    /// </summary>
    [JsonPropertyName("diffUploadResponse")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DiffUploadResponse? DiffUploadResponse { get; init; }

    /// <summary>
    /// Set if reference_type is DIFF_VERSION_RESPONSE.
    /// </summary>
    [JsonPropertyName("diffVersionResponse")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DiffVersionResponse? DiffVersionResponse { get; init; }

    /// <summary>
    /// Parameters for a media download.
    /// </summary>
    [JsonPropertyName("downloadParameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DownloadParameters? DownloadParameters { get; init; }

    /// <summary>
    /// Original file name
    /// </summary>
    [JsonPropertyName("filename")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Filename { get; init; }

    /// <summary>
    /// Deprecated, use one of explicit hash type fields instead.
    /// These two hash related fields will only be populated on Scotty based media
    /// uploads and will contain the content of the hash group in the
    /// NotificationRequest:
    /// http://cs/#google3/blobstore2/api/scotty/service/proto/upload_listener.proto&q=class:Hash
    /// Hex encoded hash value of the uploaded media.
    /// </summary>
    [Obsolete("Use one of explicit hash type fields instead.")]
    [JsonPropertyName("hash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Hash { get; init; }

    /// <summary>
    /// For Scotty uploads only. If a user sends a hash code and the backend has
    /// requested that Scotty verify the upload against the client hash,
    /// Scotty will perform the check on behalf of the backend and will reject it
    /// if the hashes don't match. This is set to true if Scotty performed
    /// this verification.
    /// </summary>
    [JsonPropertyName("hashVerified")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? HashVerified { get; init; }

    /// <summary>
    /// Media data, set if reference_type is INLINE
    /// </summary>
    [JsonPropertyName("inline")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> Inline { get; init; }

    /// <summary>
    /// |is_potential_retry| is set false only when Scotty is
    /// certain that it has not sent the request before. When a client resumes
    /// an upload, this field must be set true in agent calls, because Scotty
    /// cannot be certain that it has never sent the request before due
    /// to potential failure in the session state persistence.
    /// </summary>
    [JsonPropertyName("isPotentialRetry")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? IsPotentialRetry { get; init; }

    /// <summary>
    /// Size of the data, in bytes
    /// </summary>
    [JsonPropertyName("length")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? Length { get; init; }

    /// <summary>
    /// Scotty-provided MD5 hash for an upload.
    /// </summary>
    [JsonPropertyName("md5Hash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> Md5Hash { get; init; }

    /// <summary>
    /// Media id to forward to the operation GetMedia.
    /// Can be set if reference_type is GET_MEDIA.
    /// </summary>
    [JsonPropertyName("mediaId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> MediaId { get; init; }

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
    public GdataMediaReferenceType? ReferenceType { get; init; }

    /// <summary>
    /// Scotty-provided SHA1 hash for an upload.
    /// </summary>
    [JsonPropertyName("sha1Hash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> Sha1Hash { get; init; }

    /// <summary>
    /// Scotty-provided SHA256 hash for an upload.
    /// </summary>
    [JsonPropertyName("sha256Hash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> Sha256Hash { get; init; }

    /// <summary>
    /// Scotty-provided SHA512 hash for an upload.
    /// </summary>
    [JsonPropertyName("sha512Hash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> Sha512Hash { get; init; }

    /// <summary>
    /// Time at which the media data was last updated,
    /// in milliseconds since UNIX epoch
    /// </summary>
    [JsonPropertyName("timestamp")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ulong? Timestamp { get; init; }

    /// <summary>
    /// A unique fingerprint/version id for the media data
    /// </summary>
    [JsonPropertyName("token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Token { get; init; }
}

