using System.Text;

using GeminiDotnet.ContentGeneration;

using Microsoft.Extensions.AI;

using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
using ChatRole = Microsoft.Extensions.AI.ChatRole;

namespace GeminiDotnet.Extensions.AI;

public sealed class GeminiChatClientTests
{
    private readonly ITestOutputHelper _output;
    private readonly string _apiKey;

    public GeminiChatClientTests(ITestOutputHelper output)
    {
        _output = output;
        _apiKey = TestConfiguration.GetApiKey();
    }

    [Theory]
    [MemberData(nameof(AllModels))]
    public async Task CompleteAsyncTest(GeminiModel model)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var httpClient = new HttpClient { BaseAddress = new Uri("https://generativelanguage.googleapis.com") };
        var options = new GeminiClientOptions { ApiKey = _apiKey, };
        var client = new GeminiClient(httpClient, options);
        var chatClient = new GeminiChatClient(client);

        List<ChatMessage> messages =
        [
            new() { Role = ChatRole.User, Text = "Who was the first person to walk on the moon?", }
        ];

        var chatOptions = new ChatOptions { ModelId = model.ToString() };

        // Act
        var result = await chatClient.CompleteAsync(messages, chatOptions, cancellationToken);

        // Assert
        Assert.NotNull(result);
        var choice = Assert.Single(result.Choices);
        Assert.Contains("Armstrong", choice.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [MemberData(nameof(AllModels))]
    public async Task CompleteStreamingAsync_WithValidRequest_ShouldStreamResults(GeminiModel model)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var httpClient = new HttpClient { BaseAddress = new Uri("https://generativelanguage.googleapis.com") };
        var options = new GeminiClientOptions { ApiKey = _apiKey, };
        var client = new GeminiClient(httpClient, options);
        var chatClient = new GeminiChatClient(client);

        List<ChatMessage> messages =
        [
            new() { Role = ChatRole.User, Text = "Who was the first person to walk on the moon?", }
        ];

        var chatOptions = new ChatOptions { ModelId = model.ToString() };

        var sb = new StringBuilder(256);

        // Act
        await foreach (var update in chatClient.CompleteStreamingAsync(messages, chatOptions, cancellationToken))
        {
            Assert.NotNull(update.Text);
            sb.Append(update.Text);
        }

        var result = sb.ToString();
        _output.WriteLine(result);

        // Assert
        Assert.Contains("Armstrong", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CompleteAsync_WithConfigurationTest()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var httpClient = new HttpClient { BaseAddress = new Uri("https://generativelanguage.googleapis.com") };
        var options = new GeminiClientOptions { ApiKey = _apiKey, };
        var client = new GeminiClient(httpClient, options);
        var chatClient = new GeminiChatClient(client);

        List<ChatMessage> messages =
        [
            new() { Role = ChatRole.User, Text = "Who was the first person to walk on the moon?", }
        ];

        var chatOptions = new ChatOptions { ModelId = GeminiModel.Gemini2Flash.ToString() };

        // Act
        var result = await chatClient.CompleteAsync(messages, chatOptions, cancellationToken);

        // Assert
        Assert.NotNull(result);
        var choice = Assert.Single(result.Choices);
        Assert.Contains("Armstrong", choice.Text, StringComparison.OrdinalIgnoreCase);
    }

    public static IEnumerable<TheoryDataRow<GeminiModel>> AllModels()
    {
        return GeminiModel.ChatModels.Select(model => new TheoryDataRow<GeminiModel>(model));
    }
}