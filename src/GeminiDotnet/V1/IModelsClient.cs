using GeminiDotnet.V1.Models;

namespace GeminiDotnet.V1;

public partial interface IModelsClient
{
    /// <summary>
    /// Provides access to the Operations API operations.
    /// </summary>
    IModelsOperationsClient Operations { get; }

    /// <summary>
    /// Lists the [<see cref="V1.Models.Model"/>s](https://ai.google.dev/gemini-api/docs/models/gemini)
    /// available through the Gemini API.
    /// </summary>
    /// <param name="pageSize">
    /// The maximum number of <c>Models</c> to return (per page).
    /// If unspecified, 50 models will be returned per page.
    /// This method returns at most 1000 models per page, even if you pass a larger
    /// page_size.
    /// </param>
    /// <param name="pageToken">
    /// A page token, received from a previous <c>ListModels</c> call.
    /// Provide the <c>page_token</c> returned by one request as an argument to the next
    /// request to retrieve the next page.
    /// When paginating, all other parameters provided to <c>ListModels</c> must match
    /// the call that provided the page token.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<ListModelsResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific <see cref="V1.Models.Model"/> such as its version number, token
    /// limits,
    /// [parameters](https://ai.google.dev/gemini-api/docs/models/generative-models#model-parameters)
    /// and other metadata. Refer to the [Gemini models
    /// guide](https://ai.google.dev/gemini-api/docs/models/gemini) for detailed
    /// model information.
    /// </summary>
    /// <param name="name">
    /// Required. The resource name of the model. This name should match a model name returned by the <c>ListModels</c> method. Format: <c>models/{model}</c>
    /// A resource name of the form <c>models/{modelId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Model> GetAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues a batch of <c>EmbedContent</c> requests for batch processing.
    /// We have a <c>BatchEmbedContents</c> handler in <c>GenerativeService</c>, but it was
    /// synchronized. So we name this one to be <c>Async</c> to avoid confusion.
    /// </summary>
    /// <param name="model">
    /// Required. The name of the <see cref="V1.Models.Model"/> to use for generating the completion. Format: <c>models/{model}</c>.
    /// A resource name of the form <c>models/{modelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<AsyncBatchEmbedContentOperation> AsyncBatchEmbedContentAsync(
        string model,
        AsyncBatchEmbedContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates multiple embedding vectors from the input <see cref="V1.Content"/> which
    /// consists of a batch of strings represented as <see cref="V1.EmbedContentRequest"/>
    /// objects.
    /// </summary>
    /// <param name="model">
    /// Required. The model's resource name. This serves as an ID for the Model to use. This name should match a model name returned by the <c>ListModels</c> method. Format: <c>models/{model}</c>
    /// A resource name of the form <c>models/{modelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<BatchEmbedContentsResponse> BatchEmbedContentsAsync(
        string model,
        BatchEmbedContentsRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues a batch of <c>GenerateContent</c> requests for batch processing.
    /// </summary>
    /// <param name="model">
    /// Required. The name of the <see cref="V1.Models.Model"/> to use for generating the completion. Format: <c>models/{model}</c>.
    /// A resource name of the form <c>models/{modelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<BatchGenerateContentOperation> BatchGenerateContentAsync(
        string model,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Runs a model's tokenizer on input <see cref="V1.Content"/> and returns the token count.
    /// Refer to the [tokens guide](https://ai.google.dev/gemini-api/docs/tokens)
    /// to learn more about tokens.
    /// </summary>
    /// <param name="model">
    /// Required. The model's resource name. This serves as an ID for the Model to use. This name should match a model name returned by the <c>ListModels</c> method. Format: <c>models/{model}</c>
    /// A resource name of the form <c>models/{modelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<CountTokensResponse> CountTokensAsync(
        string model,
        CountTokensRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a text embedding vector from the input <see cref="V1.Content"/> using the
    /// specified [Gemini Embedding
    /// model](https://ai.google.dev/gemini-api/docs/models/gemini#text-embedding).
    /// </summary>
    /// <param name="model">
    /// Required. The model's resource name. This serves as an ID for the Model to use. This name should match a model name returned by the <c>ListModels</c> method. Format: <c>models/{model}</c>
    /// A resource name of the form <c>models/{modelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<EmbedContentResponse> EmbedContentAsync(
        string model,
        EmbedContentRequest request,
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
    /// A resource name of the form <c>models/{modelId}</c>.
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
    /// A resource name of the form <c>models/{modelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default);

}
