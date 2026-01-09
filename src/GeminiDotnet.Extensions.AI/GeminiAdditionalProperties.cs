using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Defines keys for Gemini-specific properties in <see cref="ChatOptions.AdditionalProperties"/>.
/// </summary>
public static class GeminiAdditionalProperties
{
    /// <summary>
    /// Key for the <see cref="ThinkingConfiguration"/> property that configures thinking/reasoning behavior.
    /// </summary>
    public const string ThinkingConfiguration = "thinkingConfig";

    /// <summary>
    /// Key for the response modalities property that specifies the desired output types (e.g., text, image).
    /// </summary>
    public const string ResponseModalities = "responseModalities";

    /// <summary>
    /// Key for the <see cref="V1Beta.ImageConfiguration"/> property that configures image generation settings.
    /// </summary>
    public const string ImageConfiguration = "imageConfig";
}
