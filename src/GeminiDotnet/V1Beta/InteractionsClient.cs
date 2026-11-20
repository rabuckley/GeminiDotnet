using System.Net.Http.Json;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GeminiDotnet.V1Beta;

internal sealed partial class InteractionsClient : IInteractionsClient
{
    private readonly IGeminiRequester _requester;

    internal InteractionsClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public IInteractionsAuthTokensClient AuthTokens => field ??= new InteractionsAuthTokensClient(_requester);

}
