using GeminiDotnet.V1Beta.TunedModels;

namespace GeminiDotnet.V1Beta;

public partial interface ITunedModelsClient
{
    /// <summary>
    /// Provides access to the Operations API operations.
    /// </summary>
    ITunedModelsOperationsClient Operations { get; }

    /// <summary>
    /// Provides access to the Permissions API operations.
    /// </summary>
    ITunedModelsPermissionsClient Permissions { get; }

    /// <summary>
    /// Lists created tuned models.
    /// </summary>
    /// <param name="pageSize">
    /// Optional. The maximum number of <c>TunedModels</c> to return (per page).
    /// The service may return fewer tuned models.
    /// If unspecified, at most 10 tuned models will be returned.
    /// This method returns at most 1000 models per page, even if you pass a larger
    /// page_size.
    /// </param>
    /// <param name="pageToken">
    /// Optional. A page token, received from a previous <c>ListTunedModels</c> call.
    /// Provide the <c>page_token</c> returned by one request as an argument to the next
    /// request to retrieve the next page.
    /// When paginating, all other parameters provided to <c>ListTunedModels</c>
    /// must match the call that provided the page token.
    /// </param>
    /// <param name="filter">
    /// Optional. A filter is a full text search over the tuned model's description and
    /// display name. By default, results will not include tuned models shared
    /// with everyone.
    /// Additional operators:
    /// - owner:me
    /// - writers:me
    /// - readers:me
    /// - readers:everyone
    /// Examples:
    /// "owner:me" returns all tuned models to which caller has owner role
    /// "readers:me" returns all tuned models to which caller has reader role
    /// "readers:everyone" returns all tuned models that are shared with everyone
    /// </param>
    /// <param name="cancellationToken"></param>
    [Obsolete]
    Task<ListTunedModelsResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        string? filter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a tuned model.
    /// Check intermediate tuning progress (if any) through the
    /// [google.longrunning.Operations] service.
    /// Access status and results through the Operations service.
    /// Example:
    /// GET /v1/tunedModels/az2mb0bpw6i/operations/000-111-222
    /// </summary>
    /// <param name="request">Required. The tuned model to create.</param>
    /// <param name="tunedModelId">
    /// Optional. The unique id for the tuned model if specified.
    /// This value should be up to 40 characters, the first character must be a
    /// letter, the last could be a letter or a number. The id must match the
    /// regular expression: <c>[a-z]([a-z0-9-]{0,38}[a-z0-9])?</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    [Obsolete]
    Task<CreateTunedModelOperation> CreateAsync(
        TunedModel request,
        string? tunedModelId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific TunedModel.
    /// </summary>
    /// <param name="name">
    /// Required. The resource name of the model. Format: <c>tunedModels/my-model-id</c>
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    [Obsolete]
    Task<TunedModel> GetAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a tuned model.
    /// </summary>
    /// <param name="name">
    /// Required. The resource name of the model. Format: <c>tunedModels/my-model-id</c>
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    [Obsolete]
    Task<Empty> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a tuned model.
    /// </summary>
    /// <param name="name">
    /// Output only. The tuned model name. A unique name will be generated on create. Example: <c>tunedModels/az2mb0bpw6i</c> If display_name is set on create, the id portion of the name will be set by concatenating the words of the display_name with hyphens and adding a random portion for uniqueness. Example: * display_name = <c>Sentence Translator</c> * name = <c>tunedModels/sentence-translator-u3b7m</c>
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="request">Required. The tuned model to update.</param>
    /// <param name="updateMask">Optional. The list of fields to update.</param>
    /// <param name="cancellationToken"></param>
    [Obsolete]
    Task<TunedModel> PatchAsync(
        string name,
        TunedModel request,
        string? updateMask = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues a batch of <c>EmbedContent</c> requests for batch processing.
    /// We have a <c>BatchEmbedContents</c> handler in <c>GenerativeService</c>, but it was
    /// synchronized. So we name this one to be <c>Async</c> to avoid confusion.
    /// </summary>
    /// <param name="model">
    /// Required. The name of the <see cref="V1Beta.Models.Model"/> to use for generating the completion. Format: <c>models/{model}</c>.
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
    /// Required. The name of the <see cref="V1Beta.Models.Model"/> to use for generating the completion. Format: <c>models/{model}</c>.
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<BatchGenerateContentOperation> BatchGenerateContentAsync(
        string model,
        BatchGenerateContentRequest request,
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
    /// <param name="model">
    /// Required. The name of the <see cref="V1Beta.Models.Model"/> to use for generating the completion. Format: <c>models/{model}</c>.
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<GenerateContentResponse> GenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a response from the model given an input message.
    /// </summary>
    /// <param name="model">
    /// Required. The name of the <see cref="V1Beta.Models.Model"/> or <see cref="V1Beta.TunedModels.TunedModel"/> to use for generating the completion. Examples: models/text-bison-001 tunedModels/sentence-translator-u3b7m
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    [Obsolete]
    Task<GenerateTextResponse> GenerateTextAsync(
        string model,
        GenerateTextRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a [streamed
    /// response](https://ai.google.dev/gemini-api/docs/text-generation?lang=python#generate-a-text-stream)
    /// from the model given an input <see cref="V1Beta.GenerateContentRequest"/>.
    /// </summary>
    /// <param name="model">
    /// Required. The name of the <see cref="V1Beta.Models.Model"/> to use for generating the completion. Format: <c>models/{model}</c>.
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    IAsyncEnumerable<GenerateContentResponse> StreamGenerateContentAsync(
        string model,
        GenerateContentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Transfers ownership of the tuned model.
    /// This is the only way to change ownership of the tuned model.
    /// The current owner will be downgraded to writer role.
    /// </summary>
    /// <param name="name">
    /// Required. The resource name of the tuned model to transfer ownership. Format: <c>tunedModels/my-model-id</c>
    /// A resource name of the form <c>tunedModels/{tunedModelId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<TransferOwnershipResponse> TransferOwnershipAsync(
        string name,
        TransferOwnershipRequest request,
        CancellationToken cancellationToken = default);

}
