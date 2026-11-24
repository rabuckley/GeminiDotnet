using GeminiDotnet.V1;
using Microsoft.Extensions.AI;
using System.Runtime.CompilerServices;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
using Type = System.Type;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// An <see cref="IChatClient"/> implementation for the Gemini AI service.
/// </summary>
public sealed class GeminiChatClient : IChatClient
{
    private readonly IGeminiClient _client;
    private readonly GeminiClient? _ownedClient;
    private readonly TimeProvider _timeProvider;
    private readonly ChatClientMetadata _metadata;

    private IModelsClient ModelsClient => _client.V1.Models;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiChatClient"/> class.
    /// </summary>
    /// <remarks>
    /// The adapter creates and owns a <see cref="GeminiClient"/>, which is disposed when the adapter is disposed.
    /// </remarks>
    /// <param name="options">The options to use for the client.</param>
    public GeminiChatClient(GeminiClientOptions options) : this(ownedClient: new GeminiClient(options))
    {
    }

    private GeminiChatClient(GeminiClient ownedClient) : this(ownedClient, TimeProvider.System)
    {
        _ownedClient = ownedClient;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiChatClient"/> class.
    /// </summary>
    /// <remarks>
    /// The caller retains ownership of <paramref name="client"/>; disposing the adapter does not dispose it.
    /// </remarks>
    /// <param name="client">The <see cref="GeminiClient"/> to use.</param>
    public GeminiChatClient(IGeminiClient client) : this(client, TimeProvider.System)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiChatClient"/> class.
    /// </summary>
    /// <param name="client">The <see cref="IGeminiClient"/> to use.</param>
    /// <param name="timeProvider">The <see cref="TimeProvider"/> to use.</param>
    internal GeminiChatClient(IGeminiClient client, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(timeProvider);

        _client = client;
        _timeProvider = timeProvider;

        _metadata = new ChatClientMetadata(
            providerName: "Gemini",
            providerUri: client.Endpoint,
            defaultModelId: client.Options.ModelId);
    }


    /// <inheritdoc />
    public async Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages);

        var (model, request) = CreateRequest(messages, options);

        var response = await ModelsClient.GenerateContentAsync($"models/{model}", request, cancellationToken).ConfigureAwait(false);

        return GeminiToMEAIMapper.CreateMappedChatResponse(
            response,
            createdAt: _timeProvider.GetUtcNow());
    }

    /// <inheritdoc />
    /// <remarks>
    /// Streamed grounding citations attach to empty <see cref="TextContent"/>. Their
    /// <see cref="TextSpanAnnotatedRegion"/> offsets index <see cref="ChatMessage.Text"/> after aggregation.
    /// <see cref="GetResponseAsync"/> returns offsets into each grounded content item.
    /// </remarks>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages);

        var (model, request) = CreateRequest(messages, options);

        var results = ModelsClient.StreamGenerateContentAsync($"models/{model}", request, cancellationToken);

        var state = new CandidateMappingState();

        await foreach (var response in results.ConfigureAwait(false))
        {
            yield return GeminiToMEAIMapper.CreateMappedChatResponseUpdate(
                response,
                state,
                createdAt: _timeProvider.GetUtcNow());
        }
    }

    private (string Model, GenerateContentRequest Request) CreateRequest(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options)
    {
        var model = ModelIdHelper.GetModelId(options, _metadata);

        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(
            model,
            messages,
            options,
            MEAIToGeminiMapper.CreateRawRepresentation<GenerateContentRequest>(
                options?.RawRepresentationFactory?.Invoke(this),
                $"{typeof(ChatOptions)}.{nameof(ChatOptions.RawRepresentationFactory)}"));

        return (model, request);
    }

    /// <inheritdoc />
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);

        if (serviceType == typeof(IGeminiClient))
        {
            return _client;
        }

        if (serviceType == typeof(ChatClientMetadata))
        {
            return _metadata;
        }

        return null;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _ownedClient?.Dispose();
    }
}
