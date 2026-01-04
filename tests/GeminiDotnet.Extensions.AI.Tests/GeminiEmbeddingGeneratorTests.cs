using GeminiDotnet.Testing;
using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

[IntegrationTest]
public sealed class GeminiEmbeddingGeneratorTests
{
    private readonly ITestOutputHelper _output;

    public GeminiEmbeddingGeneratorTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData("text-embedding-004")]
    [InlineData("gemini-embedding-001")]
    public async Task GenerateAsync_ShouldReturnEmbeddings(string model)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions
        {
            ApiKey = TestConfiguration.GetApiKey(), DefaultEmbeddingDimensions = 768
        };

        var client = new GeminiEmbeddingGenerator(options);

        var embeddingOptions = new EmbeddingGenerationOptions { ModelId = model };

        // Act
        var embeddings = await client.GenerateAsync(["Hello, world!"], embeddingOptions, cancellationToken);

        // Assert
        Assert.NotNull(embeddings);
        Assert.NotEmpty(embeddings);
        Assert.Equal(options.DefaultEmbeddingDimensions, embeddings.First().Vector.Span.Length);
    }

    [Fact]
    public async Task GenerateAsync_WithoutDimensions_ShouldReturnEmbeddingsOfDefaultSize()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var options = new GeminiClientOptions { ApiKey = TestConfiguration.GetApiKey() };

        var client = new GeminiEmbeddingGenerator(options);
        var embeddingOptions = new EmbeddingGenerationOptions { ModelId = "gemini-embedding-001" };

        // Act
        var embeddings = await client.GenerateAsync(["Hello, world!"], embeddingOptions, cancellationToken);

        // Assert
        Assert.NotNull(embeddings);
        Assert.NotEmpty(embeddings);
        Assert.Equal(3072, embeddings.First().Vector.Span.Length); // Default size for gemini-embedding-001
    }

    [Fact]
    public async Task GenerateAsync_WithMultipleInputs_ShouldReturnEmbeddings()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var options = new GeminiClientOptions { ApiKey = TestConfiguration.GetApiKey() };
        var client = new GeminiEmbeddingGenerator(options);
        const int dimensions = 2048;

        var embeddingOptions = new EmbeddingGenerationOptions
        {
            ModelId = "gemini-embedding-001", Dimensions = dimensions
        };

        var inputs = new[] { "Input one", "Input two", "Input three" };

        // Act
        var embeddings = await client.GenerateAsync(inputs, embeddingOptions, cancellationToken);

        // Assert
        Assert.NotNull(embeddings);
        Assert.Equal(inputs.Length, embeddings.Count);

        Assert.All(embeddings, embedding =>
        {
            Assert.Equal(dimensions, embedding.Vector.Span.Length);
        });
    }
}
