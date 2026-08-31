using GeminiDotnet.V1Beta.Environments;

namespace GeminiDotnet.V1Beta;

public interface IEnvironmentsClient
{
    /// <summary>
    /// Lists environments (HTTP endpoint).
    /// </summary>
    /// <param name="pageSize">
    /// Maximum number of environments to return.
    /// If unspecified, defaults to 50. Maximum is 1000.
    /// </param>
    /// <param name="pageToken">Pagination token.</param>
    /// <param name="cancellationToken"></param>
    Task<HttpBody> ListEnvironmentsHttpAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an environment (HTTP endpoint).
    /// </summary>
    /// <param name="request">Required. The environment to create (HTTP request body).</param>
    /// <param name="cancellationToken"></param>
    Task<HttpBody> CreateEnvironmentHttpAsync(
        HttpBody request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a file or directory from an environment's snapshot (HTTP
    /// endpoint).
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
    /// Retrieves a file or directory from an environment's snapshot (HTTP
    /// endpoint).
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
    /// Gets an environment (HTTP endpoint).
    /// </summary>
    /// <param name="id">Required. The identifier of the environment to retrieve.</param>
    /// <param name="cancellationToken"></param>
    Task<HttpBody> GetEnvironmentHttpAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an environment (HTTP endpoint).
    /// </summary>
    /// <param name="id">Required. The identifier of the environment to delete.</param>
    /// <param name="cancellationToken"></param>
    Task<HttpBody> DeleteEnvironmentHttpAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an environment.
    /// </summary>
    /// <param name="id">Required. The identifier of the environment to delete.</param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteEnvironmentAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an environment.
    /// </summary>
    /// <param name="id">Required. The identifier of the environment to retrieve.</param>
    /// <param name="cancellationToken"></param>
    Task<Environment> GetEnvironmentAsync(
        string id,
        CancellationToken cancellationToken = default);

}
