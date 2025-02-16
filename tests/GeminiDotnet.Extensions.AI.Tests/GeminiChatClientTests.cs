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
    public async Task CompleteAsync_WithSystemRole()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey, ApiVersion = GeminiApiVersions.V1Beta });
        var chatClient = new GeminiChatClient(client);

        List<ChatMessage> messages =
        [
            new() { Role = ChatRole.System, Text = "You are Neko the cat. Respond like one." },
            new() { Role = ChatRole.User, Text = "Hello cat!" },
            new() { Role = ChatRole.Assistant, Text = "Meow!" },
            new() { Role = ChatRole.User, Text = "What is your name? What do like to drink?" }
        ];

        var chatOptions = new ChatOptions { ModelId = GeminiModels.Gemini2Flash };

        // Act
        var result = await chatClient.CompleteAsync(messages, chatOptions, cancellationToken);

        // Assert
        Assert.NotNull(result);
        var choice = Assert.Single(result.Choices);
        Assert.Contains("Neko", choice.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [MemberData(nameof(StableModels))]
    public async Task CompleteAsyncTest(string model)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey, });
        var chatClient = new GeminiChatClient(client);

        List<ChatMessage> messages =
        [
            new() { Role = ChatRole.User, Text = "Who was the first person to walk on the moon?", }
        ];

        var chatOptions = new ChatOptions { ModelId = model };

        // Act
        var result = await chatClient.CompleteAsync(messages, chatOptions, cancellationToken);

        // Assert
        Assert.NotNull(result);
        var choice = Assert.Single(result.Choices);
        Assert.Contains("Armstrong", choice.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [MemberData(nameof(StableModels))]
    public Task CompleteStreamingAsync_WithValidRequest_ShouldStreamResults(string model)
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var options = new GeminiClientOptions { ApiKey = _apiKey };
        return StreamingCompletionTestCore(model, options, cancellationToken);
    }

    [Theory]
    [MemberData(nameof(ExperimentalModels))]
    public Task CompleteStreamingAsync_WithValidRequestAndExperimentalModel_ShouldStreamResults(string model)
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var options = new GeminiClientOptions { ApiKey = _apiKey, ApiVersion = GeminiApiVersions.V1Beta };
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
            new() { Role = ChatRole.User, Text = "Who was the first person to walk on the moon?", }
        ];

        var chatOptions = new ChatOptions { ModelId = model };
        var sb = new StringBuilder(512);

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

    public static IEnumerable<TheoryDataRow<string>> StableModels()
    {
        // Subset of stable models for testing
        yield return GeminiModels.Gemini1p5Flash;
        yield return GeminiModels.Gemini2Flash;
    }

    public static IEnumerable<TheoryDataRow<string>> ExperimentalModels()
    {
        yield return GeminiModels.Experimental.Gemini2FlashThinking;
    }
}
