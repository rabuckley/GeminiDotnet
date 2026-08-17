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

public interface IEnvironmentsCreateClient
{
    /// <summary>
    /// Creates an environment.
    /// </summary>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<Environment> CreateEnvironmentAsync(
        CreateEnvironmentRequest request,
        CancellationToken cancellationToken = default);

}
