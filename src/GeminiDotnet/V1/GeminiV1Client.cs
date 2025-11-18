namespace GeminiDotnet.V1;

public sealed partial class GeminiV1Client : IGeminiV1Client
{
    private readonly IGeminiRequester _requester;
    
    internal GeminiV1Client(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public IBatchesClient Batches => field ??= new BatchesClient(_requester);

    public ICorporaClient Corpora => field ??= new CorporaClient(_requester);

    public IDynamicClient Dynamic => field ??= new DynamicClient(_requester);

    public IFileSearchStoresClient FileSearchStores => field ??= new FileSearchStoresClient(_requester);

    public IGeneratedFilesClient GeneratedFiles => field ??= new GeneratedFilesClient(_requester);

    public IModelsClient Models => field ??= new ModelsClient(_requester);

    public IOperationsClient Operations => field ??= new OperationsClient(_requester);

    public ITunedModelsClient TunedModels => field ??= new TunedModelsClient(_requester);

}
