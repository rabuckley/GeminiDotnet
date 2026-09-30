using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.V1;

internal sealed partial class GeneratedFilesClient : IGeneratedFilesClient
{
    private readonly IGeminiRequester _requester;
    
    internal GeneratedFilesClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public IGeneratedFilesOperationsClient Operations => field ??= new GeneratedFilesOperationsClient(_requester);

}
