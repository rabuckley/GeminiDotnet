using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GeminiDotnet.Extensions.AI.Examples;

public sealed class LoggingExample
{
    public static async Task ExecuteAsync(IChatClient geminiClient, CancellationToken cancellationToken)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddConsole();
        });

        IChatClient client = new ChatClientBuilder(geminiClient)
            .UseLogging(loggerFactory,
                client => client.JsonSerializerOptions = GeminiJsonUtilities.DefaultOptions)
            .Build();

        var chatOptions = new ChatOptions { Tools = [new CodeInterpreterTool()] };

        IList<ChatMessage> messages =
        [
            new(ChatRole.User,
                "What is the sum of the first 42 numbers of the fibonacci sequence? Write and execute a program to figure it out.")
        ];

        var response = await client.GetResponseAsync(messages, chatOptions, cancellationToken);

        Console.WriteLine(response.Text);
    }
}
