using GeminiDotnet.V1Beta;
using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Provides access to Gemini metadata on <see cref="AIContent"/>.
/// </summary>
public static class GeminiContentExtensions
{
    /// <summary>
    /// Gets the audio transcription from the content's annotations.
    /// </summary>
    /// <param name="content">The content to read.</param>
    /// <returns>
    /// The first non-null transcription stored under <see cref="GeminiContentProperties.AudioTranscription"/>,
    /// or <see langword="null"/> if none is found.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="content"/> is <see langword="null"/>.</exception>
    /// <exception cref="GeminiMappingException">
    /// An entry encountered before a transcription is found has an incompatible type.
    /// </exception>
    /// <remarks>
    /// Reads typed values and values deserialized from JSON. The transcript may differ from the
    /// content's text.
    /// </remarks>
    public static AudioTranscription? GetAudioTranscription(this AIContent content)
    {
        ArgumentNullException.ThrowIfNull(content);

        if (content.Annotations is not { Count: > 0 } annotations)
        {
            return null;
        }

        foreach (var annotation in annotations)
        {
            if (annotation.AdditionalProperties?.GetValueOrThrow<AudioTranscription>(
                    GeminiContentProperties.AudioTranscription,
                    fromPropertyName: $"{typeof(AIAnnotation)}.{nameof(AIAnnotation.AdditionalProperties)}",
                    toPropertyName: $"{typeof(Part)}.{nameof(Part.AudioTranscription)}") is { } transcription)
            {
                return transcription;
            }
        }

        return null;
    }
}
