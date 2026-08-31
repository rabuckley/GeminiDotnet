using GeminiDotnet.V1Beta.EnvironmentsList;

namespace GeminiDotnet.V1Beta;

public interface IEnvironmentsListClient
{
    /// <summary>
    /// Lists environments.
    /// </summary>
    /// <param name="pageSize">
    /// Maximum number of environments to return.
    /// If unspecified, defaults to 50. Maximum is 1000.
    /// </param>
    /// <param name="pageToken">Pagination token.</param>
    /// <param name="cancellationToken"></param>
    Task<ListEnvironmentsResponse> ListEnvironmentsAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

}
