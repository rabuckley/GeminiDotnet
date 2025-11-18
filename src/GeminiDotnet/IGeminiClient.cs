using GeminiDotnet.V1;
using GeminiDotnet.V1Beta;

namespace GeminiDotnet;

public interface IGeminiClient
{
    IGeminiClientOptions Options { get; }

    public Uri? Endpoint { get; }

    public IGeminiV1Client V1 { get; }

    public IGeminiV1BetaClient V1Beta { get; }
}
