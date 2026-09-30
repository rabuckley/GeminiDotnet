using GeminiDotnet.V1Beta.Environments;

namespace GeminiDotnet.V1Beta;

public partial interface IEnvironmentsFilesClient
{
    /// <summary>
    /// Provides access to the Media API operations.
    /// </summary>
    IEnvironmentsFilesMediaClient Media { get; }

    /// <summary>
    /// Retrieves file metadata or directory contents from an environment's
    /// snapshot. To download file contents directly, pass ?alt=media or use the
    /// files.download helper.
    /// </summary>
    /// <param name="parent">A resource name of the form <c>environments/{environmentId}</c>.</param>
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
    Task<GetEnvironmentFilesResponse> ListAsync(
        string parent,
        string? path = null,
        bool? recursive = null,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

}
