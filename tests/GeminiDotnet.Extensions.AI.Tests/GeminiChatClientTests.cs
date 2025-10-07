using GeminiDotnet.Testing;
using Microsoft.Extensions.AI;
using System.Text;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
using ChatRole = Microsoft.Extensions.AI.ChatRole;

namespace GeminiDotnet.Extensions.AI;

[IntegrationTest]
public sealed class GeminiChatClientTests
{
    private readonly ITestOutputHelper _output;
    private readonly string _apiKey;

    public GeminiChatClientTests(ITestOutputHelper output)
    {
        _output = output;
        _apiKey = TestConfiguration.GetApiKey();
    }

    [Fact]
    public async Task GetResponseAsync_WithNoModel_ShouldThrowArgumentException()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var options = new GeminiClientOptions { ApiKey = "" };
        var client = new GeminiChatClient(options);

        // Act
        Task Act() => client.GetResponseAsync(new List<ChatMessage>(), new ChatOptions(), cancellationToken);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(Act);
        Assert.Contains(nameof(ChatOptions.ModelId), ex.Message);
        Assert.Contains(nameof(GeminiClientOptions), ex.Message);
        Assert.Contains(nameof(ChatOptions), ex.Message);
    }

    [Fact]
    public async Task GetStreamingResponseAsync_WithNoModel_ShouldThrowArgumentException()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var options = new GeminiClientOptions { ApiKey = "" };
        var client = new GeminiChatClient(options);

        // Act
        async Task Act()
        {
            List<ChatMessage> messages = [];
            var chatOptions = new ChatOptions();

            await foreach (var _ in client.GetStreamingResponseAsync(messages, chatOptions, cancellationToken))
            {
            }
        }

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(Act);
        Assert.Contains(nameof(ChatOptions.ModelId), ex.Message);
        Assert.Contains(nameof(GeminiClientOptions), ex.Message);
        Assert.Contains(nameof(ChatOptions), ex.Message);
    }

    [Fact]
    public async Task GetResponseAsync_WithSystemRole()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var client =
            new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey });
        var chatClient = new GeminiChatClient(client);

        List<ChatMessage> messages =
        [
            new(ChatRole.System, "You are Neko the cat. Respond like one."),
            new(ChatRole.User, "Hello cat!"),
            new(ChatRole.Assistant, "Meow!"),
            new(ChatRole.User, "What is your name? What do like to drink?")
        ];

        var chatOptions = new ChatOptions { ModelId = GeminiModels.Gemini2Flash };

        // Act
        var result = await chatClient.GetResponseAsync(messages, chatOptions, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Neko", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [MemberData(nameof(StableModels))]
    public async Task GetResponseAsyncTest(string model)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey, });
        var chatClient = new GeminiChatClient(client);

        List<ChatMessage> messages =
        [
            new(ChatRole.User, "Who was the first person to walk on the moon?")
        ];

        var chatOptions = new ChatOptions { ModelId = model };

        // Act
        var result = await chatClient.GetResponseAsync(messages, chatOptions, cancellationToken);

        // Assert
        Assert.NotNull(result);
        var choice = Assert.Single(result.Messages);
        Assert.Contains("Armstrong", choice.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [MemberData(nameof(StableModels))]
    public Task GetStreamingResponseAsync_WithValidRequest_ShouldStreamResults(string model)
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var options = new GeminiClientOptions { ApiKey = _apiKey };
        return StreamingCompletionTestCore(model, options, cancellationToken);
    }

    [Theory]
    [MemberData(nameof(ExperimentalModels))]
    public Task GetStreamingResponseAsync_WithValidRequestAndExperimentalModel_ShouldStreamResults(string model)
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var options = new GeminiClientOptions { ApiKey = _apiKey };
        return StreamingCompletionTestCore(model, options, cancellationToken);
    }

    private async Task StreamingCompletionTestCore(
        string model,
        GeminiClientOptions options,
        CancellationToken cancellationToken)
    {
        // Arrange
        using var chatClient = new GeminiChatClient(options);

        List<ChatMessage> messages =
        [
            new(ChatRole.User, "Who was the first person to walk on the moon?")
        ];

        var chatOptions = new ChatOptions { ModelId = model };
        var sb = new StringBuilder(512);
        var count = 0;

        // Act
        await foreach (var update in chatClient.GetStreamingResponseAsync(messages, chatOptions, cancellationToken))
        {
            Assert.NotNull(update.Text);
            sb.Append(update.Text);
            count++;
        }

        var result = sb.ToString();
        _output.WriteLine(result);

        // Assert
        Assert.True(count > 1);
        Assert.Contains("Armstrong", result, StringComparison.OrdinalIgnoreCase);
    }

    public static IEnumerable<TheoryDataRow<string>> StableModels()
    {
        yield return GeminiModels.Gemini2Flash;
    }

    public static IEnumerable<TheoryDataRow<string>> ExperimentalModels()
    {
        yield return GeminiModels.Experimental.Gemini2FlashThinking;
    }
}
