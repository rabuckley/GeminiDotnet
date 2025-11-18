namespace GeminiDotnet.V1Beta;

public interface IDynamicClient
{
    /// <summary>
    /// Generates a model response given an input <see cref="V1Beta.GenerateContentRequest"/>.
    /// Refer to the [text generation
    /// guide](https://ai.google.dev/gemini-api/docs/text-generation) for detailed
    /// usage information. Input capabilities differ between models, including
    /// tuned models. Refer to the [model
    /// guide](https://ai.google.dev/gemini-api/docs/models/gemini) and [tuning
    /// guide](https://ai.google.dev/gemini-api/docs/model-tuning) for details.
    /// </summary>
    /// <param name="dynamicId">
    /// Part of <c>model</c>. Required. The name of the <see cref="V1Beta.Models.Model"/> to use for generating the completion.
    /// Format: <c>models/{model}</c>.
    /// </param>
    /// <param name="request">
    /// The request body.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<GenerateContentResponse> GenerateContentByDynamicIdAsync(
        string dynamicId,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a [streamed
    /// response](https://ai.google.dev/gemini-api/docs/text-generation?lang=python#generate-a-text-stream)
    /// from the model given an input <see cref="V1Beta.GenerateContentRequest"/>.
    /// </summary>
    /// <param name="dynamicId">
    /// Part of <c>model</c>. Required. The name of the <see cref="V1Beta.Models.Model"/> to use for generating the completion.
    /// Format: <c>models/{model}</c>.
    /// </param>
    /// <param name="request">
    /// The request body.
    /// </param>
    /// <param name="cancellationToken"></param>
    IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentByDynamicIdAsync(
        string dynamicId,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default);

}
