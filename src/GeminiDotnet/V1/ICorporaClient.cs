
namespace GeminiDotnet.V1;

public partial interface ICorporaClient
{
    /// <summary>
    /// Provides access to the Operations API operations.
    /// </summary>
    ICorporaOperationsClient Operations { get; }

}
