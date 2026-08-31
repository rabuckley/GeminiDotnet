using GeminiDotnet.V1Beta.EnvironmentsCreate;

namespace GeminiDotnet.V1Beta;

internal sealed class EnvironmentsCreateClient : IEnvironmentsCreateClient
{
    private readonly IGeminiRequester _requester;
    
    internal EnvironmentsCreateClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<Environment> CreateEnvironmentAsync(
        CreateEnvironmentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1beta/environments:create";
        return _requester.ExecuteAsync<CreateEnvironmentRequest, Environment>(HttpMethod.Post, path, request, cancellationToken);
    }

}
