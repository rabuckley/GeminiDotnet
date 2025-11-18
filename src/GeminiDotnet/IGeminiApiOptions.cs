namespace GeminiDotnet;

/// <summary>
/// A public view over the options for configuring a <see cref="GeminiClient"/>.
/// </summary>
public interface IGeminiClientOptions
{
    /// <summary>
    /// The model id to use unless overridden in by the caller.
    /// </summary>
    public string? ModelId { get; }

    /// <summary>
    /// The endpoint to use for the Gemini API.
    /// </summary>
    public Uri? Endpoint { get; }
}
