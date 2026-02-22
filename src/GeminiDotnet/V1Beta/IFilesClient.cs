using GeminiDotnet.V1Beta.Files;
using File = GeminiDotnet.V1Beta.Files.File;

namespace GeminiDotnet.V1Beta;

public partial interface IFilesClient
{
    /// <summary>
    /// Lists the metadata for <see cref="V1Beta.Files.File"/>s owned by the requesting project.
    /// </summary>
    /// <param name="pageSize">
    /// Optional. Maximum number of <see cref="V1Beta.Files.File"/>s to return per page.
    /// If unspecified, defaults to 10. Maximum <c>page_size</c> is 100.
    /// </param>
    /// <param name="pageToken">Optional. A page token from a previous <c>ListFiles</c> call.</param>
    /// <param name="cancellationToken"></param>
    Task<ListFilesResponse> ListFilesAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a <see cref="V1Beta.Files.File"/>.
    /// </summary>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<CreateFileResponse> CreateFileAsync(
        CreateFileRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the metadata for the given <see cref="V1Beta.Files.File"/>.
    /// </summary>
    /// <param name="file">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<File> GetFileAsync(
        string file,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the <see cref="V1Beta.Files.File"/>.
    /// </summary>
    /// <param name="file">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteFileAsync(
        string file,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Download the <see cref="V1Beta.Files.File"/>.
    /// </summary>
    /// <param name="file">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<DownloadFileResponse> DownloadFileAsync(
        string file,
        CancellationToken cancellationToken = default);

}
