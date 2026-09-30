using GeminiDotnet.V1Beta.Environments;
using Environment = GeminiDotnet.V1Beta.Environments.Environment;

namespace GeminiDotnet.V1Beta;

public partial interface IEnvironmentsClient
{
    /// <summary>
    /// Provides access to the Files API operations.
    /// </summary>
    IEnvironmentsFilesClient Files { get; }

    /// <summary>
    /// Deletes an environment.
    /// </summary>
    /// <param name="id">Required. Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an environment.
    /// </summary>
    /// <param name="id">Required. Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<Environment> GetAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an environment.
    /// </summary>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<Environment> CreateAsync(
        CreateEnvironmentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists environments.
    /// </summary>
    /// <param name="pageSize">
    /// Optional. Maximum number of environments to return.
    /// If unspecified, defaults to 50. Maximum is 1000.
    /// </param>
    /// <param name="pageToken">Optional. Pagination token.</param>
    /// <param name="cancellationToken"></param>
    Task<ListEnvironmentsResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

}
