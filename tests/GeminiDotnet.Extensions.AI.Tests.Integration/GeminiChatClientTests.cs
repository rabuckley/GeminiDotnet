using GeminiDotnet.Testing;
using Microsoft.Extensions.AI;
using System.ComponentModel;
using System.Text;

namespace GeminiDotnet.Extensions.AI;

[IntegrationTest]
public sealed class GeminiChatClientTests
{
    private readonly ITestOutputHelper _output;
    private readonly string _apiKey;
    private const string Model = "gemini-2.5-flash-lite";

    public GeminiChatClientTests(ITestOutputHelper output)
    {
        _output = output;
        _apiKey = TestConfiguration.GetApiKey();
    }

    [Fact]
    public async Task GetStreamingResponseAsync_WithParameterlessFunction()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var geminiClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey, ModelId = Model,
        });

        [Description("Gets the current weather")]
        static string GetCurrentWeather() => "It's raining.";

        IChatClient client = new ChatClientBuilder(geminiClient)
            .UseFunctionInvocation()
            .Build();

        var messages = new List<ChatMessage>
        {
            new(ChatRole.User, "Should I wear a rain coat? Get the current weather if needed.")
        };

        var options = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(GetCurrentWeather, nameof(GetCurrentWeather))]
        };

        // Act
        var response = client.GetStreamingResponseAsync(
            messages,
            options,
            cancellationToken);

        var sb = new StringBuilder();

        await foreach (var update in response)
        {
            sb.Append(update.Text);
            _output.Write(update.Text);
        }

        var output = sb.ToString();

        // Assert
        Assert.Contains("yes", output, StringComparison.OrdinalIgnoreCase);

        var message = new ChatMessage(ChatRole.User, "Thanks, I'll wear a rain coat.");
        messages.Add(message);
        _output.WriteLine(message.Text!);

        var response2 = client.GetStreamingResponseAsync(messages, options, cancellationToken);

        await foreach (var update in response2)
        {
            foreach (var content in update.Contents)
            {
                _output.Write(content.ToString() ?? "<null>");
            }
        }
    }

    [Fact]
    public async Task GetStreamingResponseAsync_WithFunctionWithParameters()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var geminiClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey, ModelId = Model,
        });

        [Description("Gets the current weather")]
        static string GetCurrentWeather(string location, DateOnly date)
        {
            Assert.Equal("London", location, StringComparer.OrdinalIgnoreCase);
            Assert.Equal(new DateOnly(2000, 10, 1), date);
            return $"It's raining in {location}.";
        }

        IChatClient client = new ChatClientBuilder(geminiClient)
            .UseFunctionInvocation()
            .Build();

        var messages = new List<ChatMessage>
        {
            new(ChatRole.User,
                "Should I wear a rain coat in London tomorrow (1st Oct, 2000)? Get the current weather if needed using YYYY-MM-DD format.")
        };

        var options = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(GetCurrentWeather, nameof(GetCurrentWeather))]
        };

        // Act
        var response = client.GetStreamingResponseAsync(
            messages,
            options,
            cancellationToken);

        var sb = new StringBuilder();

        await foreach (var update in response)
        {
            foreach (var content in update.Contents)
            {
                if (content is not TextContent tc)
                {
                    continue;
                }

                sb.Append(tc.Text);
                _output.Write(tc.Text);
            }
        }

        var output = sb.ToString();

        // Assert
        Assert.Contains("yes", output, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// https://github.com/rabuckley/GeminiDotnet/issues/7
    /// </summary>
    [Fact]
    public void ToChatResponse_WithToolCall_RegressionTest()
    {
        // Arrange
        var callId = Guid.NewGuid().ToString();
        const string name = "GetCapitalCity";
        var arguments = new Dictionary<string, object?> { { "country", "France" } };

        List<ChatResponseUpdate> updates =
        [
            new(ChatRole.Assistant, [new FunctionCallContent(callId, name, arguments)]),
            new(ChatRole.Tool, [new FunctionResultContent(callId, "Paris")]),
            new(ChatRole.Assistant, [new TextContent("Paris is the capital of France.")]),
        ];

        // Act
        var response = updates.ToChatResponse();

        // Assert
        Assert.Equal(3, response.Messages.Count);
    }

    [Fact]
    public async Task InstructionAndSystemMessage()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        var geminiClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey, ModelId = "gemini-2.5-flash",
        });

        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, "You are a helpful assistant that translates text."),
            new(ChatRole.User, "Translate the following text to French: 'Hello, how are you?'"),
        };

        var options = new ChatOptions { Instructions = "Please provide a concise translation.", };

        var response = geminiClient.GetStreamingResponseAsync(messages, options, cancellationToken);

        var sb = new StringBuilder();

        await foreach (var update in response)
        {
            foreach (var content in update.Contents)
            {
                sb.Append(content);
                _output.Write(content.ToString() ?? "<null>");
            }
        }
    }

    [Fact]
    public async Task FunctionCallingExample()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        IChatClient geminiClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey,
            ModelId = "gemini-2.5-flash",
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
                "Should I wear a rain coat in London tomorrow (1st Oct, 2000)? Get the current weather using the function if needed.")
        ];

        var options = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(GetCurrentWeather, nameof(GetCurrentWeather))]
        };

        var response = await client.GetResponseAsync(messages, options, cancellationToken);

        messages.AddRange(response.Messages);
        messages.Add(new ChatMessage(ChatRole.User, "Thanks!"));

        var response2 = await client.GetResponseAsync(messages, options, cancellationToken);

        messages.AddRange(response2.Messages);

        Assert.All(
            messages.Where(m => m.Contents.Any(c => c is TextReasoningContent)),
            content => Assert.All(content.Contents.OfType<TextReasoningContent>(),
                reasoningContent => Assert.NotNull(reasoningContent.ProtectedData)));
    }

    record WeatherInfo(string Location, DateOnly Date, string Summary);

    [Fact]
    public async Task FunctionCalling_WithObjectReturnType()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        IChatClient geminiClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey, ModelId = "gemini-2.5-flash",
        });

        [Description("Gets the current weather")]
        static WeatherInfo GetCurrentWeather(string location, DateOnly date)
        {
            return new WeatherInfo(location, date, $"It's raining in {location} on {date}.");
        }

        IChatClient client = new ChatClientBuilder(geminiClient)
            .UseFunctionInvocation()
            .Build();

        List<ChatMessage> messages =
        [
            new(ChatRole.User,
                "Get the current weather in London tomorrow (2000-10-01) using the function.")
        ];

        var options = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(GetCurrentWeather, nameof(GetCurrentWeather))]
        };

        var response = await client.GetResponseAsync(messages, options, cancellationToken);

        var functionCall = response.Messages
            .SelectMany(m => m.Contents)
            .OfType<FunctionCallContent>()
            .FirstOrDefault();

        Assert.NotNull(functionCall);
        Assert.Equal(nameof(GetCurrentWeather), functionCall.Name);
        Assert.NotNull(functionCall.Arguments);
        Assert.Equal("London", functionCall.Arguments["location"]?.ToString());
        Assert.Equal(DateOnly.Parse("2000-10-01"), DateOnly.Parse(functionCall.Arguments["date"]!.ToString()!));
    }
}
