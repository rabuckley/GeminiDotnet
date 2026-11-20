using GeminiDotnet.V1Beta.Interactions;

namespace GeminiDotnet.V1Beta;

public partial interface IInteractionsAuthTokensClient
{
    /// <summary>
    /// Creates a short-lived auth token for client-side authentication (HTTP).
    /// </summary>
    /// <param name="name">
    /// Required. interaction_id
    /// A resource name of the form <c>interactions/{interactionId}</c>.
    /// </param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    Task<HttpBody> CreateAsync(
        string name,
        HttpBody request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes (revokes) an interaction auth token (HTTP).
    /// </summary>
    /// <param name="name">
    /// Required. Resource name of the token to revoke, in the form <c>interactions/{interaction_id}/auth_tokens/{token}</c>, where <c>{token}</c> is the hex token string returned by <c>CreateInteractionAuthToken</c> (the <c>auth_token</c> value without its <c>auth_tokens/</c> prefix).
    /// A resource name of the form <c>interactions/{interactionId}/auth_tokens/{authTokenId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<HttpBody> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default);

}
