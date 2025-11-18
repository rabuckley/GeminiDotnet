using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// An <see cref="IChatClient"/> implementation for the Gemini AI service.
/// </summary>
public sealed class GeminiEmbeddingGenerator : IEmbeddingGenerator<string, Embedding<float>>
{
    private readonly IGeminiClient _client;
    private readonly EmbeddingGeneratorMetadata _metadata;

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
    public GeminiEmbeddingGenerator(IGeminiClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        _client = client;

        _metadata = new EmbeddingGeneratorMetadata(
            providerName: "Gemini",
            providerUri: client.Endpoint,
            defaultModelId: client.Options.ModelId,
            defaultModelDimensions: 768);
    }

    public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(
        IEnumerable<string> values,
        EmbeddingGenerationOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(values);

        var modelId = ModelIdHelper.GetModelId(options, _metadata);
        var request = MEAIToGeminiMapper.CreateMappedEmbeddingRequest(modelId, values, options);
        var response = await _client.V1Beta.Models.EmbedContentAsync(modelId, request, cancellationToken).ConfigureAwait(false);
        return GeminiToMEAIMapper.CreateMappedGeneratedEmbeddings(response, options);
    }


    public object? GetService(Type serviceType, object? serviceKey = null)
    {
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

    public void Dispose()
    {
    }
}
