using Microsoft.Extensions.AI;
using System;

namespace GeminiDotnet.Extensions.AI;

public static class GeminiClientExtensions
{
    /// <summary>
    /// Gets an <see cref="IChatClient"/> using this <see cref="GeminiClient"/> instance.
    /// </summary>
    public static IChatClient AsChatClient(this GeminiClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        return new GeminiChatClient(client);
    }

    /// <summary>
    /// Gets an <see cref="IEmbeddingGenerator"/> using this <see cref="GeminiClient"/> instance.
    /// </summary>
    public static IEmbeddingGenerator AsEmbeddingGenerator(this GeminiClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        return new GeminiEmbeddingGenerator(client);
    }
}
