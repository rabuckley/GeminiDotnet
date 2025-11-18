namespace GeminiDotnet.V1Beta;

public interface IGeminiV1BetaClient
{
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
    /// Provides access to the Files API operations.
    /// </summary>
    IFilesClient Files { get; }

    /// <summary>
    /// Provides access to the FileSearchStores API operations.
    /// </summary>
    IFileSearchStoresClient FileSearchStores { get; }

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
