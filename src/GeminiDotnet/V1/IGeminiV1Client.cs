namespace GeminiDotnet.V1;

public interface IGeminiV1Client
{
    /// <summary>
    /// Provides access to the Batches API operations.
    /// </summary>
    IBatchesClient Batches { get; }

    /// <summary>
    /// Provides access to the Corpora API operations.
    /// </summary>
    ICorporaClient Corpora { get; }

    /// <summary>
    /// Provides access to the Dynamic API operations.
    /// </summary>
    IDynamicClient Dynamic { get; }

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
    /// Provides access to the Operations API operations.
    /// </summary>
    IOperationsClient Operations { get; }

    /// <summary>
    /// Provides access to the TunedModels API operations.
    /// </summary>
    ITunedModelsClient TunedModels { get; }

}
