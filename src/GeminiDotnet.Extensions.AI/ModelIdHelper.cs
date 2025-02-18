using Microsoft.Extensions.AI;
using System.Runtime.CompilerServices;

namespace GeminiDotnet.Extensions.AI;

internal static class ModelIdHelper
{
    public static string GetModelId(
        ChatOptions? options,
        ChatClientMetadata metadata,
        [CallerArgumentExpression(nameof(options))]
        string? parameterName = null)
    {
        var model = options?.ModelId ?? metadata.ModelId;

        if (model is null)
        {
            throw new ArgumentException(
                $"The '{typeof(ChatOptions)}.{nameof(options.ModelId)}' property or must be set, or the model must be provided in the {typeof(GeminiClientOptions)} when constructing this client.",
                parameterName);
        }

        return model;
    }

    public static string GetModelId(EmbeddingGenerationOptions? options, EmbeddingGeneratorMetadata metadata)
    {
        var model = options?.ModelId ?? metadata.ModelId;

        if (model is null)
        {
            throw new ArgumentException(
                $"The '{typeof(EmbeddingGenerationOptions)}.{nameof(options.ModelId)}' property or must be set, or the model must be provided in the {typeof(GeminiClientOptions)} when constructing this client.",
                nameof(options));
        }

        return model;
    }
}
