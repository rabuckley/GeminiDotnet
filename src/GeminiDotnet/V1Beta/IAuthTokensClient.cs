using GeminiDotnet.V1Beta.AuthTokens;

namespace GeminiDotnet.V1Beta;

[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public partial interface IAuthTokensClient
{
    /// <summary>
    /// Creates a token that can be used to constrain the behavior of a
    /// BidiGenerateContent session.
    /// </summary>
    /// <param name="request">Required. The token to create.</param>
    /// <param name="cancellationToken"></param>
    Task<AuthToken> CreateAsync(
        AuthToken request,
        CancellationToken cancellationToken = default);

}
