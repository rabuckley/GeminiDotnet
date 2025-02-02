using System.Runtime.CompilerServices;

using Microsoft.Extensions.AI;

using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// An <see cref="IChatClient"/> implementation for the Gemini AI service.
/// </summary>
public sealed class GeminiChatClient : IChatClient
{
    private readonly GeminiClient _client;
    private readonly TimeProvider _timeProvider;

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

        Metadata = new ChatClientMetadata("Gemini", client.Endpoint);
    }

    /// <inheritdoc />
    public ChatClientMetadata Metadata { get; }

    /// <inheritdoc />
    public async Task<ChatCompletion> CompleteAsync(
        IList<ChatMessage> chatMessages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chatMessages);

        if (options?.ModelId is null)
        {
            throw new ArgumentException($"The {nameof(options.ModelId)} property must be set", nameof(options));
        }

        var request = ExtensionsAIToGeminiMapper.CreateMappedTextGenerationRequest(chatMessages);
        var response = await _client.GenerateContentAsync(options.ModelId, request, cancellationToken);
        return GeminiToExtensionsAIMapper.CreateMappedChatCompletion(response, _timeProvider.GetUtcNow());
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<StreamingChatCompletionUpdate> CompleteStreamingAsync(
        IList<ChatMessage> chatMessages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chatMessages);

        if (options?.ModelId is null)
        {
            throw new ArgumentException($"The {nameof(options.ModelId)} property must be set", nameof(options));
        }

        var request = ExtensionsAIToGeminiMapper.CreateMappedTextGenerationRequest(chatMessages);

        await foreach (var response in _client.GenerateContentStreamingAsync(options.ModelId, request, cancellationToken))
        {
            yield return GeminiToExtensionsAIMapper.CreateMappedStreamingChatCompletionUpdate(
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

        return null;
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
}