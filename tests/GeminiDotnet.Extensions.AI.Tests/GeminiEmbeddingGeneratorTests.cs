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
    public async Task GenerateAsync_ShouldDoReturnEmbeddings(string model)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var options = new GeminiClientOptions { ApiKey = TestConfiguration.GetApiKey() };
        var client = new GeminiEmbeddingGenerator(options);

        var embeddingOptions = new EmbeddingGenerationOptions { ModelId = model };

        // Act
        var embeddings = await client.GenerateAsync(["Hello, world!"], embeddingOptions, cancellationToken);

        // Assert
        Assert.NotNull(embeddings);
        Assert.NotEmpty(embeddings);
        Assert.Equal(768, embeddings.First().Vector.Span.Length);
    }
}
