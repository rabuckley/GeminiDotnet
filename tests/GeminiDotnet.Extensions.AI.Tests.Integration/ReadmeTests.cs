using GeminiDotnet.Testing;
using Microsoft.Extensions.AI;
using System.ComponentModel;

#pragma warning disable xUnit1051 // Use TestContext.Current.CancellationToken

namespace GeminiDotnet.Extensions.AI;

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

        var options = new GeminiClientOptions { ApiKey = _apiKey, ModelId = TestConfiguration.DefaultModel };

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
            ModelId = TestConfiguration.DefaultModel,
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
            ModelId = TestConfiguration.DefaultModel,
        };

        IChatClient client = new GeminiChatClient(options);

        var chatOptions = new ChatOptions { Tools = [new HostedCodeInterpreterTool()] };

        var response = await client.GetResponseAsync(
            [new(ChatRole.User, "What is the sum of the first 42 fibonacci numbers? Generate and run code to do the calculation.")],
            chatOptions,
            cancellationToken);
        
        Assert.NotEmpty(response.Messages);
        var contents = response.Messages.SelectMany(m => m.Contents).ToList();

        // The model decides how many programs to run, so only the pairing is ours to assert: every
        // executed program comes back as one call and the one result carrying its call id.
        var calls = contents.OfType<CodeInterpreterToolCallContent>().ToList();
        var results = contents.OfType<CodeInterpreterToolResultContent>().ToList();

        Assert.NotEmpty(calls);
        Assert.Equal(calls.Count, results.Count);

        foreach (var call in calls)
        {
            var input = Assert.Single(call.Inputs!.OfType<DataContent>());
            Assert.False(input.Data.IsEmpty);

            var result = Assert.Single(results, r => r.CallId == call.CallId);
            Assert.NotEmpty(result.Outputs!);
        }

        foreach (var content in contents.OfType<TextContent>())
        {
            _output.WriteLine(content.Text);
        }
    }
}
