using GeminiDotnet.V1Beta.AuthTokens;

namespace GeminiDotnet.V1Beta;

public interface IAuthTokensClient
{
    /// <summary>
    /// Creates a token that can be used to constrain the behavior of a
    /// BidiGenerateContent session.
    /// </summary>
    /// <param name="request">Required. The token to create.</param>
    /// <param name="cancellationToken"></param>
    Task<AuthToken> CreateTokenAsync(
        AuthToken request,
        CancellationToken cancellationToken = default);

}
