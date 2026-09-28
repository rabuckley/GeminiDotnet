using GeminiDotnet.V1Beta.Environments;

namespace GeminiDotnet.V1Beta;

public partial interface IEnvironmentsClient
{
    /// <summary>
    /// Lists environments.
    /// </summary>
    /// <param name="pageSize">
    /// Optional. Maximum number of environments to return.
    /// If unspecified, defaults to 50. Maximum is 1000.
    /// </param>
    /// <param name="pageToken">Optional. Pagination token.</param>
    /// <param name="cancellationToken"></param>
    Task<HttpBody> ListEnvironmentsHttpAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an environment.
    /// </summary>
    /// <param name="request">Required. The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<HttpBody> CreateEnvironmentHttpAsync(
        HttpBody request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves file metadata or directory contents from an environment's
    /// snapshot. To download file contents directly, pass ?alt=media or use the
    /// files.download helper.
    /// </summary>
    /// <param name="environment">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
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
    Task<GetEnvironmentFilesResponse> GetEnvironmentFilesHttpByEnvironmentAsync(
        string environment,
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
    /// <param name="environment">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="path">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
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
    Task<GetEnvironmentFilesResponse> GetEnvironmentFilesHttpAsync(
        string environment,
        string path,
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
    /// Reads the bytes this operation serves, where <see cref="GetEnvironmentFilesHttpAsync"/> reads its
    /// JSON response. Dispose the returned download once its bytes have been read.
    /// </remarks>
    /// <param name="environment">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="path">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
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
    Task<MediaDownload> GetEnvironmentFilesHttpContentAsync(
        string environment,
        string path,
        bool? recursive = null,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads (creates or updates) a file in an environment's workspace.
    /// </summary>
    /// <param name="environment">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="path">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="media">The bytes to upload, at most 2 GiB.</param>
    /// <param name="cancellationToken"></param>
    Task<UploadEnvironmentFileResponse> UploadEnvironmentFileHttpAsync(
        string environment,
        string path,
        UploadEnvironmentFileRequest request,
        MediaContent media,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an environment.
    /// </summary>
    /// <param name="id">
    /// Required. Resource ID segment making up resource <c>name</c>. It identifies the resource
    /// within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<HttpBody> GetEnvironmentHttpAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an environment.
    /// </summary>
    /// <param name="id">
    /// Required. Resource ID segment making up resource <c>name</c>. It identifies the resource
    /// within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<HttpBody> DeleteEnvironmentHttpAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an environment.
    /// </summary>
    /// <param name="id">
    /// Required. Resource ID segment making up resource <c>name</c>. It identifies the resource
    /// within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteEnvironmentAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an environment.
    /// </summary>
    /// <param name="id">
    /// Required. Resource ID segment making up resource <c>name</c>. It identifies the resource
    /// within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Environment> GetEnvironmentAsync(
        string id,
        CancellationToken cancellationToken = default);

}
