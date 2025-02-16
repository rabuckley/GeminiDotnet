using System.Diagnostics.CodeAnalysis;

namespace GeminiDotnet;

/// <summary>
/// An exception thrown by the <see cref="GeminiClient"/> when a client error (status code 4xx) is returned from
/// the Gemini API.
/// </summary>
public sealed class GeminiClientException : Exception
{
    private GeminiClientException(ErrorDetails response, string message) : base(message)
    {
        Response = response;
    }

    /// <summary>
    /// The error response returned by the Gemini API.
    /// </summary>
    public ErrorDetails Response { get; set; }

    [DoesNotReturn]
    internal static void Throw(ErrorDetails response)
    {
        var message = $"Error {response.StatusCode}: {response.Message}";
        throw new GeminiClientException(response, message);
    }
}
