using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI.Examples;

public sealed class YouTubeExample
{
    public static async Task ExecuteAsync(IChatClient geminiClient, CancellationToken cancellationToken)
    {
        var chatOptions = new ChatOptions { ModelId = GeminiModels.Gemini2Flash, };

        IList<ChatMessage> messages =
        [
            new(ChatRole.User, [
                new TextContent(
                    "Summarize the following YouTube video. Pay particular attention to the parts about the future of software development."),
                new UriContent("https://www.youtube.com/watch?v=En5cSXgGvZM", "video/mp4"),
            ])
        ];

        await foreach (var update in geminiClient.GetStreamingResponseAsync(messages, chatOptions, cancellationToken))
        {
            Console.Write(update.Text);
        }
    }
}
