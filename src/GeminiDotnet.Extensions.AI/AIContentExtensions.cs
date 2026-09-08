using GeminiDotnet.V1Beta;
using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Stores and retrieves the <see cref="Part.ThoughtSignature"/> associated with mapped
/// <see cref="AIContent"/>.
/// </summary>
/// <remarks>
/// <see cref="AIContent.RawRepresentation"/> is not serialized and can be discarded by stream
/// aggregation. M.E.AI preserves the last fragment's <see cref="TextReasoningContent.ProtectedData"/>
/// when merging reasoning content, but only the first fragment's
/// <see cref="AIContent.AdditionalProperties"/> when merging text. Annotated text is excluded from
/// merging, so storing its signature in an annotation preserves the fragment it covers. Other mapped
/// content uses <see cref="AIContent.AdditionalProperties"/>. These storage locations survive JSON
/// serialization.
/// </remarks>
internal static class AIContentExtensions
{
    public static void AttachThoughtSignature(this AIContent content, string? signature)
    {
        if (signature is null)
        {
            return;
        }

        switch (content)
        {
            case TextReasoningContent reasoning:
                reasoning.ProtectedData = signature;
                break;

            case TextContent:
                (content.Annotations ??= []).Add(new AIAnnotation
                {
                    AnnotatedRegions = null,
                    RawRepresentation = null,
                    AdditionalProperties = new()
                    {
                        [GeminiContentProperties.ThoughtSignature] = signature,
                    },
                });
                break;

            default:
                (content.AdditionalProperties ??= [])[GeminiContentProperties.ThoughtSignature] = signature;
                break;
        }
    }

    /// <summary>
    /// Reads the signature Gemini issued for the part this content came from. The copy
    /// <see cref="AttachThoughtSignature"/> recorded comes first, then
    /// <see cref="GeminiContentProperties.ThoughtSignature"/> in the content's own
    /// <see cref="AIContent.AdditionalProperties"/> for a history built by hand, and last the raw part's
    /// for a content whose recorded copy a consumer stripped.
    /// </summary>
    /// <exception cref="GeminiMappingException">
    /// A <see cref="GeminiContentProperties.ThoughtSignature"/> property consulted during lookup
    /// cannot be converted to a <see cref="string"/>.
    /// </exception>
    /// <remarks>
    /// The recorded signature takes precedence over the raw part's signature, even when they differ.
    /// </remarks>
    public static string? GetThoughtSignature(this AIContent content)
    {
        var recorded = content switch
        {
            // An empty ProtectedData is no signature, which is also how M.E.AI's coalescing reads it.
            TextReasoningContent reasoning => reasoning.ProtectedData is { Length: > 0 } data ? data : null,
            TextContent text => ReadAnnotated(text),
            _ => null,
        };

        return recorded
            ?? content.AdditionalProperties?.GetValueOrThrow<string>(
                GeminiContentProperties.ThoughtSignature,
                fromPropertyName: $"{content.GetType()}.{nameof(AIContent.AdditionalProperties)}",
                toPropertyName: $"{typeof(Part)}.{nameof(Part.ThoughtSignature)}")
            ?? (content.RawRepresentation as Part)?.ThoughtSignature;
    }

    private static string? ReadAnnotated(TextContent content)
    {
        if (content.Annotations is not { Count: > 0 } annotations)
        {
            return null;
        }

        foreach (var annotation in annotations)
        {
            if (annotation.AdditionalProperties?.GetValueOrThrow<string>(
                    GeminiContentProperties.ThoughtSignature,
                    fromPropertyName: $"{typeof(AIAnnotation)}.{nameof(AIAnnotation.AdditionalProperties)}",
                    toPropertyName: $"{typeof(Part)}.{nameof(Part.ThoughtSignature)}") is { } signature)
            {
                return signature;
            }
        }

        return null;
    }
}
