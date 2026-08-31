using GeminiDotnet.Testing;
using GeminiDotnet.V1Beta;
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

        const string model = TestConfiguration.DefaultModel;

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
        var result = await client.GenerateContentAsync(model, request, cancellationToken);

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

        const string model = TestConfiguration.DefaultModel;

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
    public async Task StreamGenerateContentAsync_WithIncludeThoughts_ShouldStreamThoughtParts()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(options).V1Beta.Models;

        var model = TestConfiguration.DefaultModel;

        var request = new GenerateContentRequest
        {
            Model = model,
            GenerationConfiguration = new GenerationConfiguration
            {
                ThinkingConfiguration = new ThinkingConfiguration
                {
                    IncludeThoughts = true,
                    // IncludeThoughts alone is not enough: at the default thinking level the flash-lite
                    // tier answers without thinking, so the response carries no thought parts and no
                    // thoughts token count. Asking for a level that thinks is what makes them appear.
                    ThinkingLevel = ThinkingConfigThinkingLevel.High,
                },
            },
            Contents =
            [
                new Content { Role = ChatRoles.User, Parts = [new Part { Text = "Explain the prisoner's dilemma" }] }
            ]
        };

        var thoughts = new StringBuilder();
        var answer = new StringBuilder();
        UsageMetadata? usage = null;

        // Act
        await foreach (var chunk in client.StreamGenerateContentAsync(model, request, cancellationToken))
        {
            usage = chunk.UsageMetadata ?? usage;

            foreach (var part in StreamedParts(chunk))
            {
                (part.Thought is true ? thoughts : answer).Append(part.Text);
            }
        }

        _output.WriteLine("Thoughts:");
        _output.WriteLine(thoughts.ToString());
        _output.WriteLine("Answer:");
        _output.WriteLine(answer.ToString());

        // Assert
        Assert.NotEmpty(thoughts.ToString());
        Assert.NotEmpty(answer.ToString());
        Assert.NotNull(usage);
        Assert.True(usage.ThoughtsTokenCount > 0, $"Expected thought tokens, got {usage.ThoughtsTokenCount}.");
    }

    [Fact]
    public async Task GenerateContentAsync_WithBadRequest_ShouldThrowGeminiClientException()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(options).V1Beta.Models;

        const string model = TestConfiguration.DefaultModel;

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

        const string model = TestConfiguration.DefaultModel;

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

        const string model = TestConfiguration.DefaultModel;

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
            foreach (var part in StreamedParts(update))
            {
                if (part.Text is not null)
                {
                    _output.Write(part.Text);
                }
            }
        }

        // Assert
        // Passed.
    }

    [Fact]
    public async Task GenerateContentAsync_WithUrlContext()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions { ApiKey = _apiKey };
        var client = new GeminiClient(options).V1Beta.Models;

        const string url = "https://en.wikipedia.org/wiki/Artificial_intelligence";

        const string model = TestConfiguration.DefaultModel;

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
            model,
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

    /// <summary>
    /// Flattens the parts of a streamed chunk. A chunk is free to carry no candidates, a candidate
    /// with no content, or content with no parts — a trailing usage-only chunk does exactly that —
    /// so nothing here may assume any of them are present.
    /// </summary>
    private static IEnumerable<Part> StreamedParts(GenerateContentResponse chunk)
    {
        if (chunk.Candidates is null)
        {
            yield break;
        }

        foreach (var candidate in chunk.Candidates)
        {
            foreach (var part in candidate.Content?.Parts ?? [])
            {
                yield return part;
            }
        }
    }

    public static IEnumerable<TheoryDataRow<string>> StableModels()
    {
        yield return TestConfiguration.DefaultModel;
    }
}
