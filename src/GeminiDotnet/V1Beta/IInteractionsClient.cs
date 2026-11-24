
namespace GeminiDotnet.V1Beta;

[global::System.Diagnostics.CodeAnalysis.Experimental("GEMINI001", UrlFormat = "https://github.com/rabuckley/GeminiDotnet#v1beta-stability")]
public partial interface IInteractionsClient
{
    /// <summary>
    /// Provides access to the AuthTokens API operations.
    /// </summary>
    IInteractionsAuthTokensClient AuthTokens { get; }

}
