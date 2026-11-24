using GeminiDotnet.Testing;
using Microsoft.Extensions.AI;

namespace GeminiDotnet.Extensions.AI;

public sealed class GeminiEmbeddingGeneratorWireTests
{
    [Fact]
    public async Task GenerateAsync_WithARawRepresentationOfAnotherType_ShouldThrowWithoutSending()
    {
        var handler = new ScriptedHttpMessageHandler();
        var generator = new GeminiEmbeddingGenerator(new GeminiClient(handler.CreateHttpClient()));
        var options = new EmbeddingGenerationOptions
        {
            ModelId = "gemini-embedding-001",
            RawRepresentationFactory = _ => new V1Beta.Models.BatchEmbedContentsRequest { Requests = [] },
        };

        Task Act() => generator.GenerateAsync(["Hello, world!"], options, TestContext.Current.CancellationToken);

        var ex = await Assert.ThrowsAsync<GeminiMappingException>(Act);
        Assert.Contains(typeof(V1.Models.BatchEmbedContentsRequest).FullName!, ex.Message);
        Assert.Empty(handler.Requests);
    }
}
