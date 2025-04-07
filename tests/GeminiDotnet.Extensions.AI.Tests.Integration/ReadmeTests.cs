using GeminiDotnet.Extensions.AI.Contents;
using GeminiDotnet.Testing;
using Microsoft.Extensions.AI;
using System.ComponentModel;

namespace GeminiDotnet.Extensions.AI;

#pragma warning disable xUnit1051 // Use TestContext.Current.CancellationToken

[IntegrationTest]
public sealed class ReadmeTests
{
    private readonly string _apiKey = TestConfiguration.GetApiKey();
    private readonly ITestOutputHelper _output;

    public ReadmeTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task GetResponseStreamingExample()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions { ApiKey = _apiKey, ModelId = GeminiModels.Gemini2Flash };

        IChatClient client = new GeminiChatClient(options);

        await foreach (var update in client.GetStreamingResponseAsync("What is AI?"))
        {
            Console.Write(update);
        }
    }

    [Fact]
    public async Task FunctionCallingExample()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        IChatClient geminiClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey,
            ModelId = GeminiModels.Gemini2Flash,
            ApiVersion = GeminiApiVersions.V1Beta
        });

        [Description("Gets the current weather")]
        static string GetCurrentWeather(string location, DateOnly date)
        {
            return $"It's raining in {location} on {date}.";
        }

        IChatClient client = new ChatClientBuilder(geminiClient)
            .UseFunctionInvocation()
            .Build();

        List<ChatMessage> messages =
        [
            new(ChatRole.User,
                "Should I wear a rain coat in London tomorrow (1st Oct, 2000)? Get the current weather if needed.")
        ];

        var options = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(GetCurrentWeather, nameof(GetCurrentWeather))]
        };

        var response = await client.GetResponseAsync(messages, options, cancellationToken);
    }

    [Fact]
    public async Task CodeExecutionExample()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions
        {
            ApiKey = _apiKey,
            ModelId = GeminiModels.Gemini2Flash,
            ApiVersion = GeminiApiVersions.V1Beta
        };

        IChatClient client = new GeminiChatClient(options);

        var chatOptions = new ChatOptions { Tools = [new HostedCodeInterpreterTool()] };

        var response = await client.GetResponseAsync(
            [new(ChatRole.User, "What is the sum of the first 42 fibonacci numbers? Generate and run code to do the calculation.")],
            chatOptions,
            cancellationToken);
        
        Assert.NotEmpty(response.Messages);
        var message = response.Messages[0];

        _ = Assert.Single(message.Contents.OfType<ExecutableCodeContent>());
        _ = Assert.Single(message.Contents.OfType<CodeExecutionContent>());

        foreach (var content in message.Contents)
        {
            if (content is TextContent textContent)
            {
                _output.WriteLine(textContent.Text);
            }
        }
    }
}
