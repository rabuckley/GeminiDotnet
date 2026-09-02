using GeminiDotnet.V1Beta;
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
    private readonly TimeProvider _timeProvider;
    private readonly ChatClientMetadata _metadata;

    private IModelsClient ModelsClient => _client.V1Beta.Models;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiChatClient"/> class.
    /// </summary>
    /// <param name="options">The options to use for the client.</param>
    public GeminiChatClient(GeminiClientOptions options) : this(new GeminiClient(options))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiChatClient"/> class.
    /// </summary>
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

        var model = ModelIdHelper.GetModelId(options, _metadata);

        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(
            model,
            messages,
            options,
            options?.RawRepresentationFactory?.Invoke(this) as GenerateContentRequest);

        var response = await ModelsClient.GenerateContentAsync(model, request, cancellationToken).ConfigureAwait(false);

        return GeminiToMEAIMapper.CreateMappedChatResponse(
            response,
            createdAt: _timeProvider.GetUtcNow());
    }

    /// <inheritdoc />
    /// <remarks>
    /// Grounding citations are indexed differently here than by <see cref="GetResponseAsync"/>. Gemini
    /// gives a streamed citation offsets into the text of the whole stream rather than of one part, so
    /// each <see cref="TextSpanAnnotatedRegion"/> is attached to an empty <see cref="TextContent"/> of its
    /// own and indexes <see cref="ChatMessage.Text"/> of the aggregated response, not the text of the
    /// content it sits on. A whole response keeps the Gemini convention: its regions sit on the grounded
    /// <see cref="TextContent"/> and index that content's own text.
    /// </remarks>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages);

        var model = ModelIdHelper.GetModelId(options, _metadata);

        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(
            model,
            messages,
            options,
            options?.RawRepresentationFactory?.Invoke(this) as GenerateContentRequest);

        var results = ModelsClient.StreamGenerateContentAsync(model, request, cancellationToken);

        // One state for the whole stream, so that a tool call correlates with the result that arrives in a
        // later chunk and a grounding segment resolves against the text every chunk has produced.
        var state = new CandidateMappingState();

        await foreach (var response in results.ConfigureAwait(false))
        {
            yield return GeminiToMEAIMapper.CreateMappedChatResponseUpdate(
                response,
                state,
                createdAt: _timeProvider.GetUtcNow());
        }
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
    }
}
