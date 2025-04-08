using Microsoft.Extensions.AI;
using System.Runtime.CompilerServices;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// An <see cref="IChatClient"/> implementation for the Gemini AI service.
/// </summary>
public sealed class GeminiChatClient : IChatClient
{
    private readonly GeminiClient _client;
    private readonly TimeProvider _timeProvider;
    private readonly ChatClientMetadata _metadata;

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
    public GeminiChatClient(GeminiClient client) : this(client, TimeProvider.System)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiChatClient"/> class.
    /// </summary>
    /// <param name="client">The <see cref="GeminiClient"/> to use.</param>
    /// <param name="timeProvider">The <see cref="TimeProvider"/> to use.</param>
    internal GeminiChatClient(GeminiClient client, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(timeProvider);

        _client = client;
        _timeProvider = timeProvider;

        _metadata = new ChatClientMetadata("Gemini", providerUri: client.Endpoint, defaultModelId: client.Options.ModelId);
    }


    /// <inheritdoc />
    public async Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages);

        var model = ModelIdHelper.GetModelId(options, _metadata);
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(messages, options);
        var response = await _client.GenerateContentAsync(model, request, cancellationToken).ConfigureAwait(false);
        return GeminiToMEAIMapper.CreateMappedChatResponse(response, _timeProvider.GetUtcNow());
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages);

        var model = ModelIdHelper.GetModelId(options, _metadata);
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest(messages, options);

        await foreach (var response in _client.GenerateContentStreamingAsync(model, request, cancellationToken).ConfigureAwait(false))
        {
            yield return GeminiToMEAIMapper.CreateMappedChatResponseUpdate(
                response,
                createdAt: _timeProvider.GetUtcNow());
        }
    }

    /// <inheritdoc />
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        if (serviceType == typeof(GeminiClient))
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
