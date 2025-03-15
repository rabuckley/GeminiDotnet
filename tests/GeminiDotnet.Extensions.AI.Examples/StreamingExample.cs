using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI.Examples;

public sealed class StreamingExample
{
    public static async Task ExecuteAsync(IChatClient client, CancellationToken cancellationToken)
    {
        var chatOptions = new ChatOptions();

        IList<ChatMessage> messages =
        [
            new(ChatRole.User, "Explain Wittgenstein's Philosophical Investigations")
        ];

        await foreach (var update in client.GetStreamingResponseAsync(messages, chatOptions, cancellationToken))
        {
            Console.Write(update.Text);
        }
    }
}
