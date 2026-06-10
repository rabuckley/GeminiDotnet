using System.Text.Json.Serialization;

namespace GeminiDotnet.V1Beta.FileSearchStores;

/// <summary>
/// Information to read/write to blobstore2.
/// </summary>
public sealed record Blobstore2Info
{
    /// <summary>
    /// The blob generation id.
    /// </summary>
    [JsonPropertyName("blobGeneration")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? BlobGeneration { get; init; }

    /// <summary>
    /// The blob id, e.g., /blobstore/prod/playground/scotty
    /// </summary>
    [JsonPropertyName("blobId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? BlobId { get; init; }

    /// <summary>
    /// A serialized External Read Token passed from Bigstore -> Scotty for a GCS
    /// download. This field must never be consumed outside of Bigstore, and is not
    /// applicable to non-GCS media uploads.
    /// </summary>
    [JsonPropertyName("downloadExternalReadToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> DownloadExternalReadToken { get; init; }

    /// <summary>
    /// Read handle passed from Bigstore -> Scotty for a GCS download.
    /// This is a signed, serialized blobstore2.ReadHandle proto which must never
    /// be set outside of Bigstore, and is not applicable to non-GCS media
    /// downloads.
    /// </summary>
    [JsonPropertyName("downloadReadHandle")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> DownloadReadHandle { get; init; }

    /// <summary>
    /// The blob read token. Needed to read blobs that have not been
    /// replicated. Might not be available until the final call.
    /// </summary>
    [JsonPropertyName("readToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ReadToken { get; init; }

    /// <summary>
    /// A serialized Object Fragment List Creation Info passed from Bigstore ->
    /// Scotty for a GCS upload. This field must never be consumed outside of
    /// Bigstore, and is not applicable to non-GCS media uploads.
    /// </summary>
    [JsonPropertyName("uploadFragmentListCreationInfo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> UploadFragmentListCreationInfo { get; init; }

    /// <summary>
    /// Metadata passed from Blobstore -> Scotty for a new GCS upload.
    /// This is a signed, serialized blobstore2.BlobMetadataContainer proto which
    /// must never be consumed outside of Bigstore, and is not applicable to
    /// non-GCS media uploads.
    /// </summary>
    [JsonPropertyName("uploadMetadataContainer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReadOnlyMemory<byte> UploadMetadataContainer { get; init; }
}

