using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace GeminiDotnet.Extensions.AI.Examples;

public sealed partial class FunctionCallingExample
{
    // To support the parameters of the `GetCurrentWeather` function.
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(DateOnly))]
    private sealed partial class JsonContext : JsonSerializerContext;

    public static async Task ExecuteAsync(IChatClient geminiClient, CancellationToken cancellationToken)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddConsole();
        });

        IChatClient client = new ChatClientBuilder(geminiClient)
            .UseFunctionInvocation(loggerFactory)
            .Build();

        [Description("Gets the current weather")]
        static string GetCurrentWeather(string location, DateOnly date)
        {
            return $"It's raining in {location} on {date}.";
        }

        List<ChatMessage> messages =
        [
            new(ChatRole.User,
                "Should I wear a rain coat in London tomorrow (1st Oct, 2000)? Get the current weather if needed.")
        ];

        var options = new ChatOptions
        {
            Tools =
            [
                AIFunctionFactory.Create(
                    method: GetCurrentWeather,
                    name: nameof(GetCurrentWeather),
                    serializerOptions: JsonContext.Default.Options)
            ]
        };

        var response = await client.GetResponseAsync(messages, options, cancellationToken);

        Console.WriteLine(response);
    }
}
