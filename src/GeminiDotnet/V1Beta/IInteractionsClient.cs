
namespace GeminiDotnet.V1Beta;

public partial interface IInteractionsClient
{
    /// <summary>
    /// Provides access to the AuthTokens API operations.
    /// </summary>
    IInteractionsAuthTokensClient AuthTokens { get; }

}
