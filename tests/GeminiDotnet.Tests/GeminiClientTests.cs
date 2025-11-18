using GeminiDotnet.Testing;
using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.CachedContents;
using GeminiDotnet.V1Beta.Models;
using System.Net;
using System.Text;

namespace GeminiDotnet;

[IntegrationTest]
public sealed class GeminiClientTests
{
    private readonly ITestOutputHelper _output;
    private readonly string _apiKey;

    public GeminiClientTests(ITestOutputHelper output)
    {
        _apiKey = TestConfiguration.GetApiKey();
        _output = output;
    }

    private static GenerateContentRequest WhoWasTheFirstPersonToWalkOnTheMoonRequest(string model) => new()
    {
        Model = model,
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

        var options = new GeminiClientOptions { ApiKey = _apiKey, };
        var client = new GeminiClient(options).V1Beta.Models;

        var request = WhoWasTheFirstPersonToWalkOnTheMoonRequest(model);

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

        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey }).V1Beta.Models;

        const string model = "gemini-2.5-flash";

        var request = new GenerateContentRequest
        {
            Model = model,
            SystemInstruction = new Content { Parts = [new Part { Text = "You are Neko the cat. Respond like one." }] },
            Contents =
            [
                new() { Role = ChatRoles.User, Parts = [new() { Text = "Hello cat!" }] },
                new() { Role = ChatRoles.Model, Parts = [new() { Text = "Meow!" }] },
                new() { Role = ChatRoles.User, Parts = [new() { Text = "What is your name? What do like to drink?" }] },
            ]
        };

        // Act
        var result = await client.GenerateContentAsync("gemini-2.5-flash", request, cancellationToken);

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

        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey }).V1Beta.Models;
        var request = WhoWasTheFirstPersonToWalkOnTheMoonRequest(model);

        var sb = new StringBuilder();
        var count = 0;

        // Act
        await foreach (var result in client.StreamGenerateContentAsync(model, request, cancellationToken))
        {
            var response = result.Candidates.Single().Content.Parts.Single();
            Assert.NotNull(response.Text);
            sb.Append(response.Text);
            count++;
        }

        var resultText = sb.ToString();
        _output.WriteLine(resultText);

        // Assert
        Assert.True(count > 1);
        Assert.Contains("Armstrong", resultText, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("text-embedding-004")]
    [InlineData("gemini-embedding-001")]
    public async Task EmbedContentAsync_WithTextContent_ShouldReturnEmbeddings(string model)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey }).V1Beta.Models;

        var request = new EmbedContentRequest
        {
            Model = model,
            Content = new Content { Parts = [new Part { Text = "The quick brown fox jumps over the lazy dog." }] }
        };

        // Act
        var result = await client.EmbedContentAsync(model, request, cancellationToken);

        // Assert
        Assert.NotNull(result.Embedding);
        Assert.NotEqual(0, result.Embedding.Values.Length);
    }

    [Fact]
    public async Task GenerateContentAsync_WithCodeExecution_ShouldReturnExecutedCode()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(options).V1Beta.Models;

        const string model = "gemini-2.5-flash";

        var request = new GenerateContentRequest
        {
            Model = model,
            Tools = [new Tool { CodeExecution = new CodeExecution() }],
            Contents =
            [
                new Content
                {
                    Role = ChatRoles.User,
                    Parts =
                    [
                        new Part { Text = "Can you print Hello, World! using Python? Generate and run the program." }
                    ]
                }
            ]
        };

        // Act
        var result = await client.GenerateContentAsync(model, request, cancellationToken);

        // Assert
        var candidate = result.Candidates.Single();
        var explanation = candidate.Content.Parts.First(p => p.Text is not null).Text!;
        _output.WriteLine(explanation);
        var codePart = candidate.Content.Parts.First(p => p.ExecutableCode is not null).ExecutableCode!;

        _output.WriteLine(codePart.Language.ToString());
        _output.WriteLine(codePart.Code);

        Assert.Contains("Hello, World!", codePart.Code);
        Assert.Equal(ExecutableCodeLanguage.Python, codePart.Language);
        var resultPart = candidate.Content.Parts.First(p => p.CodeExecutionResult is not null).CodeExecutionResult!;

        _output.WriteLine(resultPart.Output);
        Assert.Contains("Hello, World!", resultPart.Output);
    }

    [Fact]
    public async Task GenerateContent_WithThinkingModel()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(options).V1Beta.Models;

        var model = "gemini-2.5-pro";

        var request = new GenerateContentRequest
        {
            Model = model,
            GenerationConfiguration = new GenerationConfiguration
            {
                ThinkingConfiguration = new ThinkingConfiguration { IncludeThoughts = true },
            },
            Contents =
            [
                new Content { Role = ChatRoles.User, Parts = [new Part { Text = "Explain the prisoner's dilemma" }] }
            ]
        };

        var sb = new StringBuilder();

        GenerateContentResponse? response = null;

        // Act
        await foreach (var result in client.StreamGenerateContentAsync(model, request, cancellationToken))
        {
            response = result;
            var part = result.Candidates.Single().Content.Parts.Single();

            Assert.NotNull(part.Text);

            if (part.Thought is true)
            {
                sb.Append($"Thought: {part.Text}");
            }
            else
            {
                sb.Append(part.Text);
            }
        }

        var resultText = sb.ToString();
        _output.WriteLine(resultText);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.UsageMetadata.ThoughtsTokenCount > 0);
        Assert.Contains("prisoner", resultText, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GenerateContentAsync_WithBadRequest_ShouldThrowGeminiClientException()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(options).V1Beta.Models;

        const string model = "gemini-2.5-flash";

        var request = new GenerateContentRequest
        {
            Model = model,
            Contents = [new Content { Role = "Fred", Parts = [new Part { Text = "What is the meaning of life?" }] }]
        };

        // Act
        async Task Act() => await client.GenerateContentAsync(model, request, cancellationToken);

        // Assert
        var ex = await Assert.ThrowsAsync<GeminiClientException>(Act);
        Assert.Equal(HttpStatusCode.BadRequest, ex.Response.StatusCode);
        Assert.Equal("INVALID_ARGUMENT", ex.Response.Status);
    }

    [Fact]
    public async Task GenerateContentAsync_WithSearchTool_ShouldReturnSearchResults()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(options).V1Beta.Models;

        const string model = "gemini-2.5-flash";

        var request = new GenerateContentRequest
        {
            Model = model,
            Tools = [new Tool { GoogleSearch = new GoogleSearch() }],
            Contents =
            [
                new Content
                {
                    Role = ChatRoles.User,
                    Parts = [new Part { Text = "When is the next total solar eclipse in the United States?" }]
                }
            ]
        };

        // Act
        var result = await client.GenerateContentAsync(model, request, cancellationToken);

        // Assert
        Assert.NotNull(result);
        var candidate = Assert.Single(result.Candidates);
        Assert.NotNull(candidate.GroundingMetadata);
        Assert.NotNull(candidate.GroundingMetadata.WebSearchQueries);
        Assert.NotNull(candidate.GroundingMetadata.GroundingChunks);

        foreach (var search in candidate.GroundingMetadata.WebSearchQueries)
        {
            _output.WriteLine($"Searched for: '{search}'");
        }

        foreach (var chunk in candidate.GroundingMetadata.GroundingChunks)
        {
            _output.WriteLine($"{chunk.Web!.Title}: {chunk.Web!.Uri}");
        }

        Assert.NotNull(candidate.GroundingMetadata.SearchEntryPoint?.RenderedContent);
        _output.WriteLine(candidate.GroundingMetadata.SearchEntryPoint.RenderedContent);
    }

    [Fact]
    public async Task Github_22()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(options).V1Beta.Models;

        const string model = "gemini-2.5-flash";

        GenerateContentRequest request = new()
        {
            Model = model,
            Contents =
            [
                new Content
                {
                    Parts =
                    [
                        new Part
                        {
                            FileData = new FileData { FileUri = "https://www.youtube.com/watch?v=JxlB5kYz990" }
                        },
                        new Part { Text = "Write a summary of the video." }
                    ]
                }
            ],
            Tools = [new Tool { GoogleSearch = new GoogleSearch() }]
        };

        await foreach (var update in client.StreamGenerateContentAsync(model, request, cancellationToken))
        {
            var response = update.Candidates.Single().Content.Parts.Single();

            if (response.Text is not null)
            {
                _output.Write(response.Text);
            }
        }

        // Assert
        // Passed.
    }

    [Fact]
    public async Task GenerateContent_WithThinkingBudget0_ShouldHaveNoThinkingTokenUsage()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(options).V1Beta.Models;

        const string model = "gemini-2.5-pro";

        var request = new GenerateContentRequest
        {
            Model = model,
            GenerationConfiguration = new GenerationConfiguration
            {
                ThinkingConfiguration = new ThinkingConfiguration { ThinkingBudget = 0 }
            },
            Contents =
            [
                new Content { Role = ChatRoles.User, Parts = [new Part { Text = "Explain the prisoner's dilemma" }] }
            ]
        };

        // Act
        var response = await client.GenerateContentAsync(
            model,
            request,
            cancellationToken);

        // Assert
        Assert.Equal(0, response.UsageMetadata.ThoughtsTokenCount);
    }

    [Fact]
    public async Task GenerateContentAsync_WithUrlContext()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(options).V1Beta.Models;

        const string url = "https://en.wikipedia.org/wiki/Artificial_intelligence";

        const string model = "gemini-2.5-flash";

        var request = new GenerateContentRequest
        {
            Model = model,
            Tools = [new Tool { UrlContext = new UrlContext() }],
            Contents =
            [
                new Content
                {
                    Role = ChatRoles.User,
                    Parts = [new Part { Text = $"Summarize the content from the URL {url}." }]
                }
            ],
        };

        // Act
        var response = await client.GenerateContentAsync(
            "gemini-2.5-flash",
            request,
            cancellationToken);

        // Assert
        var candidate = Assert.Single(response.Candidates);
        var metadata = candidate.UrlContextMetadata?.UrlMetadata;
        Assert.NotNull(metadata);
        var urlContext = Assert.Single(metadata);
        Assert.Equal(url, urlContext.RetrievedUrl);
        Assert.Equal(UrlMetadataUrlRetrievalStatus.Success, urlContext.UrlRetrievalStatus);
    }

    public static IEnumerable<TheoryDataRow<string>> StableModels()
    {
        yield return "gemini-3-pro-preview";
    }
}
