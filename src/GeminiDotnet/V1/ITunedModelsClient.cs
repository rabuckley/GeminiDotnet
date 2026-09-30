
namespace GeminiDotnet.V1;

public partial interface ITunedModelsClient
{
    /// <summary>
    /// Provides access to the Operations API operations.
    /// </summary>
    ITunedModelsOperationsClient Operations { get; }

    /// <summary>
    /// Enqueues a batch of <c>EmbedContent</c> requests for batch processing.
    /// We have a <c>BatchEmbedContents</c> handler in <c>GenerativeService</c>, but it was
    /// synchronized. So we name this one to be <c>Async</c> to avoid confusion.
    /// </summary>
    /// <param name="model">
    /// Required. The name of the <see cref="V1.Models.Model"/> to use for generating the completion. Format: <c>models/{model}</c>.
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<AsyncBatchEmbedContentOperation> AsyncBatchEmbedContentAsync(
        string model,
        AsyncBatchEmbedContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues a batch of <c>GenerateContent</c> requests for batch processing.
    /// </summary>
    /// <param name="model">
    /// Required. The name of the <see cref="V1.Models.Model"/> to use for generating the completion. Format: <c>models/{model}</c>.
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<BatchGenerateContentOperation> BatchGenerateContentAsync(
        string model,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a model response given an input <see cref="V1.GenerateContentRequest"/>.
    /// Refer to the [text generation
    /// guide](https://ai.google.dev/gemini-api/docs/text-generation) for detailed
    /// usage information. Input capabilities differ between models, including
    /// tuned models. Refer to the [model
    /// guide](https://ai.google.dev/gemini-api/docs/models/gemini) and [tuning
    /// guide](https://ai.google.dev/gemini-api/docs/model-tuning) for details.
    /// </summary>
    /// <param name="model">
    /// Required. The name of the <see cref="V1.Models.Model"/> to use for generating the completion. Format: <c>models/{model}</c>.
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<GenerateContentResponse> GenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a [streamed
    /// response](https://ai.google.dev/gemini-api/docs/text-generation?lang=python#generate-a-text-stream)
    /// from the model given an input <see cref="V1.GenerateContentRequest"/>.
    /// </summary>
    /// <param name="model">
    /// Required. The name of the <see cref="V1.Models.Model"/> to use for generating the completion. Format: <c>models/{model}</c>.
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default);

}
