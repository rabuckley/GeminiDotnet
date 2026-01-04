using GeminiDotnet.V1;
using GeminiDotnet.V1Beta;

namespace GeminiDotnet;

/// <summary>
/// Represents a client for interacting with the Google Gemini API.
/// </summary>
public interface IGeminiClient
{
    /// <summary>
    /// Gets the options that this client is configured with.
    /// </summary>
    IGeminiClientOptions Options { get; }

    /// <summary>
    /// Gets the base endpoint URI for the Gemini API.
    /// </summary>
    Uri? Endpoint { get; }

    /// <summary>
    /// Gets the client for interacting with the Gemini V1 API.
    /// </summary>
    IGeminiV1Client V1 { get; }

    /// <summary>
    /// Gets the client for interacting with the Gemini V1Beta API.
    /// </summary>
    IGeminiV1BetaClient V1Beta { get; }
}
