using GeminiDotnet.V1Beta.AuthTokens;
using GeminiDotnet.V1Beta.CachedContents;
using GeminiDotnet.V1Beta.Corpora;
using GeminiDotnet.V1Beta.Environments;
using GeminiDotnet.V1Beta.EnvironmentsCreate;
using GeminiDotnet.V1Beta.EnvironmentsList;
using GeminiDotnet.V1Beta.Files;
using GeminiDotnet.V1Beta.FileSearchStores;
using GeminiDotnet.V1Beta.FilesRegister;
using GeminiDotnet.V1Beta.GeneratedFiles;
using GeminiDotnet.V1Beta.Models;
using GeminiDotnet.V1Beta.TunedModels;
using Environment = GeminiDotnet.V1Beta.EnvironmentsList.Environment;
using File = GeminiDotnet.V1Beta.Files.File;

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
