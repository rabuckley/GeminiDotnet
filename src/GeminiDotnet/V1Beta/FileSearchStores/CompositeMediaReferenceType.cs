using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Describes what the field reference contains.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<CompositeMediaReferenceType>))]
public enum CompositeMediaReferenceType
{
    /// <summary>
    /// Reference contains a GFS path or a local path.
    /// </summary>
    [JsonStringEnumMemberName("PATH")]
    Path,

    /// <summary>
    /// Reference points to a blobstore object. This could be either
    /// a v1 blob_ref or a v2 blobstore2_info. Clients should check
    /// blobstore2_info first, since v1 is being deprecated.
    /// </summary>
    [JsonStringEnumMemberName("BLOB_REF")]
    BlobRef,

    /// <summary>
    /// Data is included into this proto buffer
    /// </summary>
    [JsonStringEnumMemberName("INLINE")]
    Inline,

    /// <summary>
    /// Reference points to a bigstore object
    /// </summary>
    [JsonStringEnumMemberName("BIGSTORE_REF")]
    BigstoreRef,

    /// <summary>
    /// Indicates the data is stored in cosmo_binary_reference.
    /// </summary>
    [JsonStringEnumMemberName("COSMO_BINARY_REFERENCE")]
    CosmoBinaryReference,
}

