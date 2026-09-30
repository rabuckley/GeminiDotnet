using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.V1;

internal sealed partial class CorporaClient : ICorporaClient
{
    private readonly IGeminiRequester _requester;
    
    internal CorporaClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public ICorporaOperationsClient Operations => field ??= new CorporaOperationsClient(_requester);

}
