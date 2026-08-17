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

public interface IAuthTokensClient
{
    /// <summary>
    /// Creates a token that can be used to constrain the behavior of a
    /// BidiGenerateContent session.
    /// </summary>
    /// <param name="request">Required. The token to create.</param>
    /// <param name="cancellationToken"></param>
    Task<AuthToken> CreateTokenAsync(
        AuthToken request,
        CancellationToken cancellationToken = default);

}
