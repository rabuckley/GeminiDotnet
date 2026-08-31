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

        var chatOptions = new ChatOptions { ModelId = TestConfiguration.DefaultModel };

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

    private async Task StreamingCompletionTestCore(
        string model,
        GeminiClientOptions options,
        CancellationToken cancellationToken)
    {
        // Arrange
        using var chatClient = new GeminiChatClient(options);

        List<ChatMessage> messages =
        [
            new(ChatRole.User, "Explain the theory of relativity in simple terms.")
        ];

        var chatOptions = new ChatOptions { ModelId = model };
        var updates = new List<ChatResponseUpdate>();
        var streamed = new StringBuilder(512);

        // Act
        await foreach (var update in chatClient.GetStreamingResponseAsync(messages, chatOptions, cancellationToken))
        {
            updates.Add(update);
            streamed.Append(update.Text);
        }

        _output.WriteLine(streamed.ToString());

        // Assert
        Assert.True(updates.Count > 1, $"Expected the answer to arrive in several updates, got {updates.Count}.");
        Assert.NotEmpty(streamed.ToString());
        Assert.Contains(updates, update => update.FinishReason == ChatFinishReason.Stop);
        Assert.All(updates, update => Assert.NotNull(update.ResponseId));
        Assert.Single(updates.Select(u => u.ResponseId).Distinct());

        // The updates must assemble back into the same answer the caller saw arrive piecewise.
        var response = updates.ToChatResponse();
        Assert.Equal(streamed.ToString(), response.Text);
        Assert.Equal(ChatRole.Assistant, Assert.Single(response.Messages).Role);
        Assert.NotNull(response.ModelId);
    }

    public static IEnumerable<TheoryDataRow<string>> StableModels()
    {
        yield return TestConfiguration.DefaultModel;
    }
}
