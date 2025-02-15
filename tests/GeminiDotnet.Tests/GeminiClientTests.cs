using GeminiDotnet.ContentGeneration;
using GeminiDotnet.ContentGeneration.FunctionCalling;
using GeminiDotnet.Embeddings;
using System.Text;

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
            new Content 
            {
                Role = ChatRoles.User,
                Parts =
                [
                    new Part { Text = "Who was the first person to walk on the moon?" }
                ]
            }
        ]
    };

    [Theory]
    [MemberData(nameof(StableModels))]
    public async Task GenerateContentAsync_WithValidRequest_ShouldGetResults(string model)
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
        Assert.NotNull(response.Text);
        var resultText = response.Text;
        _output.WriteLine(resultText);
        Assert.Contains("Armstrong", resultText, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GenerateContent_WithSystemInstruction_ShouldGetResults()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey, ApiVersion = GeminiApiVersions.V1Beta });

        var request = new GenerateContentRequest
        {
            SystemInstruction = new Content 
            {
                Parts = [ new Part { Text = "You are Neko the cat. Respond like one." } ]
            },
            Contents =
            [
                new() { Role = ChatRoles.User, Parts = [new() { Text = "Hello cat!" }] },
                new() { Role = ChatRoles.Model, Parts = [new() { Text = "Meow!" }] },
                new() { Role = ChatRoles.User, Parts = [new() { Text = "What is your name? What do like to drink?" }] },
            ]
        };

        // Act
        var result = await client.GenerateContentAsync(GeminiModels.Gemini2Flash, request, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        var candidate = Assert.Single(result.Candidates);
        var choice = Assert.Single(candidate.Content.Parts);
        Assert.Contains("Neko", choice.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [MemberData(nameof(StableModels))]
    public async Task GenerateContentStreamingAsync_WithValidRequest_ShouldStreamResults(string model)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey });
        var request = WhoWasTheFirstPersonToWalkOnTheMoonRequest;

        var sb = new StringBuilder();

        // Act

        try
        {
            await foreach (var result in client.GenerateContentStreamingAsync(model, request, cancellationToken))
            {
                var response = result.Candidates.Single().Content.Parts.Single();
                Assert.NotNull(response.Text);
                sb.Append(response.Text);
            }
        }
        catch (HttpRequestException ex)
        {
            _output.WriteLine(ex.Message);
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
                    new Part { Text = "The quick brown fox jumps over the lazy dog." }
                ]
            }
        };

        // Act
        var result = await client.EmbedContentAsync(GeminiModels.TextEmbedding004, request, cancellationToken);

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
        var options = new GeminiClientOptions { ApiKey = _apiKey, ApiVersion = GeminiApiVersions.V1Beta };
        var client = new GeminiClient(httpClient, options);

        var request = new GenerateContentRequest
        {
            Tools = [new Tool { CodeExecution = new CodeExecution() }],
            Contents =
            [
                new Content 
                {
                    Role = ChatRoles.User,
                    Parts =
                    [
                        new Part { Text = "Can you print Hello, World! using Python?" }
                    ]
                }
            ]
        };

        // Act
        var result = await client.GenerateContentAsync(GeminiModels.Gemini2Flash, request, cancellationToken);

        // Assert
        var candidate = result.Candidates.Single();
        var explanation = candidate.Content.Parts.First(p => p.Text is not null).Text!;
        _output.WriteLine(explanation);
        var codePart = candidate.Content.Parts.First(p => p.ExecutableCode is not null).ExecutableCode!;

        _output.WriteLine(codePart.Language);
        _output.WriteLine(codePart.Code);

        Assert.Contains("Hello, World!", codePart.Code);
        Assert.Equal("PYTHON", codePart.Language);
        var resultPart = candidate.Content.Parts.First(p => p.CodeExecutionResult is not null).CodeExecutionResult!;

        _output.WriteLine(resultPart.Output);
        Assert.Contains("Hello, World!", resultPart.Output);
    }

    [Fact]
    public async Task Thinking_WithWhat_ShouldDoWhat()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var httpClient = new HttpClient { BaseAddress = new Uri("https://generativelanguage.googleapis.com") };
        var options = new GeminiClientOptions { ApiKey = _apiKey, ApiVersion = GeminiApiVersions.V1Alpha };
        var client = new GeminiClient(httpClient, options);

        var request = new GenerateContentRequest
        {
            GenerationConfiguration = new GenerationConfiguration
            {
                ThinkingConfiguration = new ThinkingConfiguration { IncludeThoughts = true },
            },
            Contents =
            [
                new Content 
                {
                    Role = ChatRoles.User,
                    Parts =
                    [
                        new Part { Text = "Explain the prisoner's dilemma" }
                    ]
                }
            ]
        };

        var sb = new StringBuilder();

        // Act
        await foreach (var result in client.GenerateContentStreamingAsync(
                           GeminiModels.Experimental.Gemini2FlashThinking,
                           request,
                           cancellationToken))
        {
            var response = result.Candidates.Single().Content.Parts.Single();
            Assert.NotNull(response.Text);
            sb.Append(response.Text);
        }

        var resultText = sb.ToString();
        _output.WriteLine(resultText);

        // Assert
        Assert.Contains("prisoner", resultText, StringComparison.OrdinalIgnoreCase);
    }

    public static IEnumerable<TheoryDataRow<string>> StableModels()
    {
        yield return GeminiModels.Gemini1p5Flash8b;
    }
}