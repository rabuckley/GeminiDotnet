using GeminiDotnet.V1Beta.Models;

namespace GeminiDotnet.V1Beta;

public interface IModelsClient
{
    /// <summary>
    /// Lists the [<see cref="V1Beta.Models.Model"/>s](https://ai.google.dev/gemini-api/docs/models/gemini)
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
    Task<ListModelsResponse> ListModelsAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific <see cref="V1Beta.Models.Model"/> such as its version number, token
    /// limits,
    /// [parameters](https://ai.google.dev/gemini-api/docs/models/generative-models#model-parameters)
    /// and other metadata. Refer to the [Gemini models
    /// guide](https://ai.google.dev/gemini-api/docs/models/gemini) for detailed
    /// model information.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<Model> GetModelAsync(
        string model,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues a batch of <c>EmbedContent</c> requests for batch processing.
    /// We have a <c>BatchEmbedContents</c> handler in <c>GenerativeService</c>, but it was
    /// synchronized. So we name this one to be <c>Async</c> to avoid confusion.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<AsyncBatchEmbedContentOperation> AsyncBatchEmbedContentAsync(
        string model,
        AsyncBatchEmbedContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates multiple embedding vectors from the input <see cref="V1Beta.Content"/> which
    /// consists of a batch of strings represented as <see cref="V1Beta.Models.EmbedContentRequest"/>
    /// objects.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<BatchEmbedContentsResponse> BatchEmbedContentsAsync(
        string model,
        BatchEmbedContentsRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates multiple embeddings from the model given input text in a
    /// synchronous call.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<BatchEmbedTextResponse> BatchEmbedTextAsync(
        string model,
        BatchEmbedTextRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues a batch of <c>GenerateContent</c> requests for batch processing.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<BatchGenerateContentOperation> BatchGenerateContentAsync(
        string model,
        BatchGenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Runs a model's tokenizer on a string and returns the token count.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<CountMessageTokensResponse> CountMessageTokensAsync(
        string model,
        CountMessageTokensRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Runs a model's tokenizer on a text and returns the token count.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<CountTextTokensResponse> CountTextTokensAsync(
        string model,
        CountTextTokensRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Runs a model's tokenizer on input <see cref="V1Beta.Content"/> and returns the token count.
    /// Refer to the [tokens guide](https://ai.google.dev/gemini-api/docs/tokens)
    /// to learn more about tokens.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<CountTokensResponse> CountTokensAsync(
        string model,
        CountTokensRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a text embedding vector from the input <see cref="V1Beta.Content"/> using the
    /// specified [Gemini Embedding
    /// model](https://ai.google.dev/gemini-api/docs/models/gemini#text-embedding).
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<EmbedContentResponse> EmbedContentAsync(
        string model,
        EmbedContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates an embedding from the model given an input message.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<EmbedTextResponse> EmbedTextAsync(
        string model,
        EmbedTextRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a grounded answer from the model given an input
    /// <see cref="V1Beta.Models.GenerateAnswerRequest"/>.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<GenerateAnswerResponse> GenerateAnswerAsync(
        string model,
        GenerateAnswerRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a model response given an input <see cref="V1Beta.GenerateContentRequest"/>.
    /// Refer to the [text generation
    /// guide](https://ai.google.dev/gemini-api/docs/text-generation) for detailed
    /// usage information. Input capabilities differ between models, including
    /// tuned models. Refer to the [model
    /// guide](https://ai.google.dev/gemini-api/docs/models/gemini) and [tuning
    /// guide](https://ai.google.dev/gemini-api/docs/model-tuning) for details.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<GenerateContentResponse> GenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a response from the model given an input <see cref="V1Beta.Models.MessagePrompt"/>.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<GenerateMessageResponse> GenerateMessageAsync(
        string model,
        GenerateMessageRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a response from the model given an input message.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<GenerateTextResponse> GenerateTextAsync(
        string model,
        GenerateTextRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a prediction request.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<PredictResponse> PredictAsync(
        string model,
        PredictRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Same as Predict but returns an LRO.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<PredictLongRunningOperation> PredictLongRunningAsync(
        string model,
        PredictLongRunningRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a [streamed
    /// response](https://ai.google.dev/gemini-api/docs/text-generation?lang=python#generate-a-text-stream)
    /// from the model given an input <see cref="V1Beta.GenerateContentRequest"/>.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists operations that match the specified filter in the request. If the
    /// server doesn't support this method, it returns <c>UNIMPLEMENTED</c>.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="filter">The standard list filter.</param>
    /// <param name="pageSize">The standard list page size.</param>
    /// <param name="pageToken">The standard list page token.</param>
    /// <param name="returnPartialSuccess">
    /// When set to <c>true</c>, operations that are reachable are returned as normal,
    /// and those that are unreachable are returned in the
    /// ListOperationsResponse.unreachable
    /// field.
    /// This can only be <c>true</c> when reading across collections. For example, when
    /// <c>parent</c> is set to <c>"projects/example/locations/-"</c>.
    /// This field is not supported by default and will result in an
    /// <c>UNIMPLEMENTED</c> error if set unless explicitly documented otherwise in
    /// service or product specific documentation.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<ListOperationsResponse> ListOperationsByModelAsync(
        string model,
        string? filter = null,
        int? pageSize = null,
        string? pageToken = null,
        bool? returnPartialSuccess = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest state of a long-running operation.  Clients can use this
    /// method to poll the operation result at intervals as recommended by the API
    /// service.
    /// </summary>
    /// <param name="model">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="operation">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<Operation> GetOperationByModelAndOperationAsync(
        string model,
        string operation,
        CancellationToken cancellationToken = default);

}
