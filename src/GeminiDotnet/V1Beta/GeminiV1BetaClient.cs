namespace GeminiDotnet.V1Beta;

public sealed partial class GeminiV1BetaClient : IGeminiV1BetaClient
{
    private readonly IGeminiRequester _requester;
    
    internal GeminiV1BetaClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public IBatchesClient Batches => field ??= new BatchesClient(_requester);

    public ICachedContentsClient CachedContents => field ??= new CachedContentsClient(_requester);

    public ICorporaClient Corpora => field ??= new CorporaClient(_requester);

    public IDynamicClient Dynamic => field ??= new DynamicClient(_requester);

    public IFilesClient Files => field ??= new FilesClient(_requester);

    public IFileSearchStoresClient FileSearchStores => field ??= new FileSearchStoresClient(_requester);

    public IFilesRegisterClient FilesRegister => field ??= new FilesRegisterClient(_requester);

    public IGeneratedFilesClient GeneratedFiles => field ??= new GeneratedFilesClient(_requester);

    public IModelsClient Models => field ??= new ModelsClient(_requester);

    public ITunedModelsClient TunedModels => field ??= new TunedModelsClient(_requester);

}
