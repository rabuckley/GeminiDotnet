using GeminiDotnet.V1.Files;
using File = GeminiDotnet.V1.Files.File;

namespace GeminiDotnet.V1;

public partial interface IFilesClient
{
    /// <summary>
    /// Lists the metadata for <see cref="V1.Files.File"/>s owned by the requesting project.
    /// </summary>
    /// <param name="pageSize">
    /// Optional. Maximum number of <see cref="V1.Files.File"/>s to return per page.
    /// If unspecified, defaults to 10. Maximum <c>page_size</c> is 100.
    /// </param>
    /// <param name="pageToken">Optional. A page token from a previous <c>ListFiles</c> call.</param>
    /// <param name="cancellationToken"></param>
    Task<ListFilesResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the metadata for the given <see cref="V1.Files.File"/>.
    /// </summary>
    /// <param name="name">
    /// Required. The name of the <see cref="V1.Files.File"/> to get. Example: <c>files/abc-123</c>
    /// A resource name of the form <c>files/{fileId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<File> GetAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the <see cref="V1.Files.File"/>.
    /// </summary>
    /// <param name="name">
    /// Required. The name of the <see cref="V1.Files.File"/> to delete. Example: <c>files/abc-123</c>
    /// A resource name of the form <c>files/{fileId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Download the <see cref="V1.Files.File"/>.
    /// </summary>
    /// <remarks>
    /// Reads the bytes this operation serves, which is all it serves. Dispose the returned
    /// download once its bytes have been read.
    /// </remarks>
    /// <param name="name">A resource name of the form <c>files/{fileId}</c>.</param>
    /// <param name="cancellationToken"></param>
    Task<MediaDownload> DownloadAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Registers a Google Cloud Storage files with FileService. The user is
    /// expected to provide Google Cloud Storage URIs and will receive a File
    /// resource for each URI in return. Note that the files are not copied, just
    /// registered with File API. If one file fails to register, the whole request
    /// fails.
    /// </summary>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<RegisterFilesResponse> RegisterAsync(
        RegisterFilesRequest request,
        CancellationToken cancellationToken = default);

}
