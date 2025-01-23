using System.Text;

using GeminiDotnet.ContentGeneration;
using GeminiDotnet.ContentGeneration.FunctionCalling;
using GeminiDotnet.Embeddings;

namespace GeminiDotnet;

public sealed class GeminiClientTests
{
    private readonly ITestOutputHelper _output;
    private readonly string _apiKey;

    public GeminiClientTests(ITestOutputHelper output)
    {
        _apiKey = TestConfiguration.GetApiKey();
        _output = output;
    }

    private static GenerateContentRequest WhoWasTheFirstPersonToWalkOnTheMoonRequest => new()
    {
        Contents =
        [
            new ChatMessage
            {
                Role = ChatRole.User,
                Parts =
                [
                    new TextContentPart { Text = "Who was the first person to walk on the moon?" }
                ]
            }
        ]
    };

    [Theory]
    [MemberData(nameof(AllModels))]
    public async Task GenerateContentAsync_WithValidRequest_ShouldStreamResults(GeminiModel model)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var httpClient = new HttpClient { BaseAddress = new Uri("https://generativelanguage.googleapis.com") };
        var options = new GeminiClientOptions { ApiKey = _apiKey, };
        var client = new GeminiClient(httpClient, options);
        var request = WhoWasTheFirstPersonToWalkOnTheMoonRequest;

        // Act
        var result = await client.GenerateContentAsync(model, request, cancellationToken);

        // Assert
        var response = result.Candidates.Single().Content.Parts.Single();
        Assert.IsType<TextContentPart>(response);
        var resultText = ((TextContentPart)response).Text;
        _output.WriteLine(resultText);
        Assert.Contains("Armstrong", resultText, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [MemberData(nameof(AllModels))]
    public async Task GetTextGenerationResultsAsync_WithValidRequest_ShouldStreamResults(GeminiModel model)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var httpClient = new HttpClient { BaseAddress = new Uri("https://generativelanguage.googleapis.com") };
        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(httpClient, options);
        var request = WhoWasTheFirstPersonToWalkOnTheMoonRequest;

        var sb = new StringBuilder();

        // Act
        await foreach (var result in client.GenerateContentStreamingAsync(model, request, cancellationToken))
        {
            var response = result.Candidates.Single().Content.Parts.Single();
            Assert.IsType<TextContentPart>(response);
            sb.AppendLine(((TextContentPart)response).Text);
        }

        var resultText = sb.ToString();
        _output.WriteLine(resultText);

        // Assert
        Assert.Contains("Armstrong", resultText, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task EmbedContentAsync_WithTextContent_ShouldReturnEmbeddings()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey });

        var request = new EmbeddingRequest
        {
            Content = new EmbeddingContent
            {
                Parts =
                [
                    new TextContentPart { Text = "The quick brown fox jumps over the lazy dog." }
                ]
            }
        };

        // Act
        var result = await client.EmbedContentAsync(GeminiModel.TextEmbedding004, request, cancellationToken);

        // Assert
        Assert.NotNull(result.Embedding);
        Assert.NotEmpty(result.Embedding.Values);
    }

    [Fact]
    public async Task GenerateContentAsync_WithCodeExecution_ShouldReturnExecutedCode()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var httpClient = new HttpClient { BaseAddress = new Uri("https://generativelanguage.googleapis.com") };
        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(httpClient, options);

        var request = new GenerateContentRequest
        {
            Tools = [new Tool { CodeExecution = new() }],
            Contents =
            [
                new ChatMessage
                {
                    Role = ChatRole.User,
                    Parts =
                    [
                        new TextContentPart { Text = "Can you print Hello, World! using Python?" }
                    ]
                }
            ]
        };

        // Act
        var result = await client.GenerateContentAsync(GeminiModel.Gemini1p5Flash, request, cancellationToken);

        // Assert
        var candidate = result.Candidates.Single();
        var explanation = candidate.Content.Parts.OfType<TextContentPart>().First().Text;
        _output.WriteLine(explanation);
        var codePart = candidate.Content.Parts.OfType<ExecutableCodeContentPart>().First();

        _output.WriteLine(codePart.Language);
        _output.WriteLine(codePart.Code);

        Assert.Contains("Hello, World!", codePart.Code);
        Assert.Equal("PYTHON", codePart.Language);
        var resultPart = candidate.Content.Parts.OfType<CodeExecutionResultContentPart>().First();

        _output.WriteLine(resultPart.Output);
        Assert.Contains("Hello, World!", resultPart.Output);
    }

    public static IEnumerable<TheoryDataRow<GeminiModel>> AllModels()
    {
        foreach (GeminiModel model in GeminiModel.ChatModels) yield return model;
    }
}