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

public interface IGeminiV1BetaClient
{
    /// <summary>
    /// Provides access to the AuthTokens API operations.
    /// </summary>
    IAuthTokensClient AuthTokens { get; }

    /// <summary>
    /// Provides access to the Batches API operations.
    /// </summary>
    IBatchesClient Batches { get; }

    /// <summary>
    /// Provides access to the CachedContents API operations.
    /// </summary>
    ICachedContentsClient CachedContents { get; }

    /// <summary>
    /// Provides access to the Corpora API operations.
    /// </summary>
    ICorporaClient Corpora { get; }

    /// <summary>
    /// Provides access to the Dynamic API operations.
    /// </summary>
    IDynamicClient Dynamic { get; }

    /// <summary>
    /// Provides access to the Environments API operations.
    /// </summary>
    IEnvironmentsClient Environments { get; }

    /// <summary>
    /// Provides access to the EnvironmentsCreate API operations.
    /// </summary>
    IEnvironmentsCreateClient EnvironmentsCreate { get; }

    /// <summary>
    /// Provides access to the EnvironmentsList API operations.
    /// </summary>
    IEnvironmentsListClient EnvironmentsList { get; }

    /// <summary>
    /// Provides access to the Files API operations.
    /// </summary>
    IFilesClient Files { get; }

    /// <summary>
    /// Provides access to the FileSearchStores API operations.
    /// </summary>
    IFileSearchStoresClient FileSearchStores { get; }

    /// <summary>
    /// Provides access to the FilesRegister API operations.
    /// </summary>
    IFilesRegisterClient FilesRegister { get; }

    /// <summary>
    /// Provides access to the GeneratedFiles API operations.
    /// </summary>
    IGeneratedFilesClient GeneratedFiles { get; }

    /// <summary>
    /// Provides access to the Models API operations.
    /// </summary>
    IModelsClient Models { get; }

    /// <summary>
    /// Provides access to the TunedModels API operations.
    /// </summary>
    ITunedModelsClient TunedModels { get; }

}
