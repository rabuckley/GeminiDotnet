using GeminiDotnet.ContentGeneration;

using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

internal static class ModelMapper
{
    internal static GeminiModel GetModel(ChatOptions? options)
    {
        if (options?.ModelId is null)
        {
            throw new InvalidOperationException(
                $"Callers must specify a {nameof(options.ModelId)} in the {nameof(ChatOptions)}.");
        }

        if (!GeminiModel.TryParse(options.ModelId, null, out var model))
        {
            throw new InvalidOperationException($"Invalid {nameof(options.ModelId)}: '{options.ModelId}'");
        }

        return model;
    }

    internal static GeminiModel GetModel(EmbeddingGenerationOptions? options)
    {
        if (options?.ModelId is null)
        {
            throw new InvalidOperationException(
                $"Callers must specify a {nameof(options.ModelId)} in the {nameof(EmbeddingGenerationOptions)}.");
        }

        if (!GeminiModel.TryParse(options.ModelId, null, out var model))
        {
            throw new InvalidOperationException($"Invalid {nameof(options.ModelId)}: '{options.ModelId}'");
        }

        return model;
    }
}