using GeminiDotnet.V1Beta.Environments;

namespace GeminiDotnet.V1Beta;

public partial interface IEnvironmentsFilesMediaClient
{
    /// <summary>
    /// Retrieves file metadata or directory contents from an environment's
    /// snapshot. To download file contents directly, pass ?alt=media or use the
    /// files.download helper.
    /// </summary>
    /// <param name="parent">
    /// Required. The resource name of the environment. Format: <c>environments/{environment_id}</c>
    /// A resource name of the form <c>environments/{environmentId}</c>.
    /// </param>
    /// <param name="path">
    /// Optional. The path of the file or directory within the environment.
    /// If empty, defaults to the root of the workspace.
    /// Example: "workspace/src/main.py"
    /// </param>
    /// <param name="recursive">
    /// Optional. If true and the path is a directory, recursively lists all files
    /// and subdirectories. Defaults to false (immediate children only).
    /// </param>
    /// <param name="pageSize">
    /// Optional. Maximum number of entries to return per page (for directory
    /// listing). If unspecified, defaults to 100. Maximum is 1000.
    /// NOLINT
    /// </param>
    /// <param name="pageToken">
    /// Optional. Pagination token for directory listing.
    /// NOLINT
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<GetEnvironmentFilesResponse> DownloadAsync(
        string parent,
        string? path = null,
        bool? recursive = null,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves file metadata or directory contents from an environment's
    /// snapshot. To download file contents directly, pass ?alt=media or use the
    /// files.download helper.
    /// </summary>
    /// <remarks>
    /// Reads the bytes this operation serves, where <see cref="DownloadAsync"/> reads its
    /// JSON response. Dispose the returned download once its bytes have been read.
    /// </remarks>
    /// <param name="parent">
    /// Required. The resource name of the environment. Format: <c>environments/{environment_id}</c>
    /// A resource name of the form <c>environments/{environmentId}</c>.
    /// </param>
    /// <param name="path">
    /// Optional. The path of the file or directory within the environment.
    /// If empty, defaults to the root of the workspace.
    /// Example: "workspace/src/main.py"
    /// </param>
    /// <param name="recursive">
    /// Optional. If true and the path is a directory, recursively lists all files
    /// and subdirectories. Defaults to false (immediate children only).
    /// </param>
    /// <param name="pageSize">
    /// Optional. Maximum number of entries to return per page (for directory
    /// listing). If unspecified, defaults to 100. Maximum is 1000.
    /// NOLINT
    /// </param>
    /// <param name="pageToken">
    /// Optional. Pagination token for directory listing.
    /// NOLINT
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<MediaDownload> DownloadContentAsync(
        string parent,
        string? path = null,
        bool? recursive = null,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads (creates or updates) a file in an environment's workspace.
    /// </summary>
    /// <param name="parent">
    /// Required. The resource name of the target environment. Format: <c>environments/{environment_id}</c>
    /// A resource name of the form <c>environments/{environmentId}</c>.
    /// </param>
    /// <param name="path">Required. The destination relative path of the file within the environment. Example: "workspace/src/main.py"</param>
    /// <param name="request">The request body.</param>
    /// <param name="media">The bytes to upload, at most 2 GiB.</param>
    /// <param name="cancellationToken"></param>
    Task<UploadEnvironmentFileResponse> UploadAsync(
        string parent,
        string path,
        UploadEnvironmentFileRequest request,
        MediaContent media,
        CancellationToken cancellationToken = default);

}
