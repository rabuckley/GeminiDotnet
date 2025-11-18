using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

public static class GeminiClientExtensions
{
    /// <summary>
    /// Gets an <see cref="IChatClient"/> using this <see cref="IGeminiClient"/> instance.
    /// </summary>
    public static IChatClient AsChatClient(this IGeminiClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        return new GeminiChatClient(client);
    }

    /// <summary>
    /// Gets an <see cref="IEmbeddingGenerator"/> using this <see cref="IGeminiClient"/> instance.
    /// </summary>
    public static IEmbeddingGenerator AsEmbeddingGenerator(this IGeminiClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        return new GeminiEmbeddingGenerator(client);
    }
}
