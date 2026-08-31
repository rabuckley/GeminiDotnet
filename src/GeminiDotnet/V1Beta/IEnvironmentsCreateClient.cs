using GeminiDotnet.V1Beta.EnvironmentsCreate;

namespace GeminiDotnet.V1Beta;

public interface IEnvironmentsCreateClient
{
    /// <summary>
    /// Creates an environment.
    /// </summary>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<Environment> CreateEnvironmentAsync(
        CreateEnvironmentRequest request,
        CancellationToken cancellationToken = default);

}
