using GeminiDotnet.V1Beta.Media;

namespace GeminiDotnet.V1Beta;

public partial interface IMediaClient
{
    /// <summary>
    /// Downloads media from a <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    /// <param name="name">
    /// Required. The resource name of the media to download. Example: <c>fileSearchStores/abc-123/media/blob123</c>
    /// A resource name of the form <c>fileSearchStores/{fileSearchStoreId}/media/{mediaId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<DownloadMediaResponse> DownloadAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads media from a <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    /// <remarks>
    /// Reads the bytes this operation serves, where <see cref="DownloadAsync"/> reads its
    /// JSON response. Dispose the returned download once its bytes have been read.
    /// </remarks>
    /// <param name="name">
    /// Required. The resource name of the media to download. Example: <c>fileSearchStores/abc-123/media/blob123</c>
    /// A resource name of the form <c>fileSearchStores/{fileSearchStoreId}/media/{mediaId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<MediaDownload> DownloadContentAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads data to a FileSearchStore, preprocesses and chunks before storing
    /// it in a FileSearchStore Document.
    /// </summary>
    /// <param name="fileSearchStoreName">
    /// Required. Immutable. The name of the <see cref="V1Beta.FileSearchStores.FileSearchStore"/> to upload the file into. Example: <c>fileSearchStores/my-file-search-store-123</c>
    /// A resource name of the form <c>fileSearchStores/{fileSearchStoreId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="media">The bytes to upload, at most 100 MiB.</param>
    /// <param name="cancellationToken"></param>
    Task<UploadToFileSearchStoreOperation> UploadToFileSearchStoreAsync(
        string fileSearchStoreName,
        UploadToFileSearchStoreRequest request,
        MediaContent media,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a <see cref="V1Beta.Files.File"/>.
    /// </summary>
    /// <param name="request">The request body.</param>
    /// <param name="media">The bytes to upload, at most 2 GiB.</param>
    /// <param name="cancellationToken"></param>
    Task<CreateFileResponse> UploadAsync(
        CreateFileRequest request,
        MediaContent media,
        CancellationToken cancellationToken = default);

}
