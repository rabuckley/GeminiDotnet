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
using File = GeminiDotnet.V1Beta.Files.File;

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
