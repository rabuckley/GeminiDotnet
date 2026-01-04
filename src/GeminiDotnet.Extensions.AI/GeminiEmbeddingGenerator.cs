using GeminiDotnet.V1Beta.Models;
using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// An <see cref="IEmbeddingGenerator{TInput, TEmbedding}"/> implementation for the Gemini AI service.
/// </summary>
public sealed class GeminiEmbeddingGenerator : IEmbeddingGenerator<string, Embedding<float>>
{
    private readonly IGeminiClient _client;
    private readonly EmbeddingGeneratorMetadata _metadata;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiEmbeddingGenerator"/> class.
    /// </summary>
    /// <param name="options">The options to use for the client.</param>
    public GeminiEmbeddingGenerator(GeminiClientOptions options) : this(new GeminiClient(options))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiEmbeddingGenerator"/> class.
    /// </summary>
    /// <param name="client">The <see cref="IGeminiClient"/> to use.</param>
    public GeminiEmbeddingGenerator(IGeminiClient client) : this(client, TimeProvider.System)
    {
    }

    internal GeminiEmbeddingGenerator(IGeminiClient client, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(timeProvider);

        _client = client;
        _timeProvider = timeProvider;

        _metadata = new EmbeddingGeneratorMetadata(
            providerName: "Gemini",
            providerUri: client.Endpoint,
            defaultModelId: client.Options.ModelId,
            defaultModelDimensions: client.Options.DefaultEmbeddingDimensions);
    }

    public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(
        IEnumerable<string> values,
        EmbeddingGenerationOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(values);

        var modelId = ModelIdHelper.GetModelId(options, _metadata);

        var request = MEAIToGeminiMapper.CreateMappedEmbeddingRequest(
            modelId,
            values,
            options,
            options?.RawRepresentationFactory?.Invoke(this) as EmbedContentRequest);

        var response = await _client.V1Beta.Models
            .EmbedContentAsync(modelId, request, cancellationToken)
            .ConfigureAwait(false);

        return GeminiToMEAIMapper.CreateMappedGeneratedEmbeddings(
            response,
            modelId,
            createdAt: _timeProvider.GetUtcNow());
    }


    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);

        if (serviceType == typeof(IGeminiClient))
        {
            return _client;
        }

        if (serviceType == typeof(EmbeddingGeneratorMetadata))
        {
            return _metadata;
        }

        return null;
    }

    public void Dispose()
    {
    }
}
