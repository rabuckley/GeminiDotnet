using System.Runtime.Intrinsics;
using System.Text;

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
        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey, });
        return StreamingCompletionTestCore(model, client, cancellationToken);
    }

    [Theory]
    [MemberData(nameof(BetaModels))]
    public Task CompleteStreamingAsync_WithValidRequestAndExperimentalModel_ShouldStreamResults(string model)
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey, ApiVersion = GeminiApiVersions.V1Beta });
        return StreamingCompletionTestCore(model, client, cancellationToken);
    }

    private async Task StreamingCompletionTestCore(
        string model,
        GeminiClient client,
        CancellationToken cancellationToken)
    {
        // Arrange
        var chatClient = new GeminiChatClient(client);

        List<ChatMessage> messages =
        [
            new() { Role = ChatRole.User, Text = "Who was the first person to walk on the moon?", }
        ];

        var chatOptions = new ChatOptions { ModelId = model };

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
        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey, });
        var chatClient = new GeminiChatClient(client);

        List<ChatMessage> messages =
        [
            new() { Role = ChatRole.User, Text = "Who was the first person to walk on the moon?", }
        ];

        var chatOptions = new ChatOptions { ModelId = GeminiModels.Gemini2Flash };

        // Act
        var result = await chatClient.CompleteAsync(messages, chatOptions, cancellationToken);

        // Assert
        Assert.NotNull(result);
        var choice = Assert.Single(result.Choices);
        Assert.Contains("Armstrong", choice.Text, StringComparison.OrdinalIgnoreCase);
    }

    public static IEnumerable<TheoryDataRow<string>> StableModels()
    {
        yield return GeminiModels.Gemini1p5Pro;
        yield return GeminiModels.Gemini1p5Flash;
        yield return GeminiModels.Gemini1p5Flash8b;
    }

    public static IEnumerable<TheoryDataRow<string>> BetaModels()
    {
        yield return GeminiModels.Gemini2Flash;
        yield return GeminiModels.Gemini2FlashThinking;
    }
}