using GeminiDotnet.V1Beta.AuthTokens;

namespace GeminiDotnet.V1Beta;

internal sealed class AuthTokensClient : IAuthTokensClient
{
    private readonly IGeminiRequester _requester;
    
    internal AuthTokensClient(IGeminiRequester requester)
    {
        ArgumentNullException.ThrowIfNull(requester);
        _requester = requester;
    }

    public Task<AuthToken> CreateTokenAsync(
        AuthToken request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        const string path = "/v1beta/auth_tokens";
        return _requester.ExecuteAsync<AuthToken, AuthToken>(HttpMethod.Post, path, request, cancellationToken);
    }

}
