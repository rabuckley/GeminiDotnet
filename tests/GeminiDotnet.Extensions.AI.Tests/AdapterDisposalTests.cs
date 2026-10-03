using Microsoft.Extensions.AI;
using System.Net;
using System.Text;

#pragma warning disable MEAI001 // Type is for evaluation purposes only

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Each adapter disposes the <see cref="GeminiClient"/> it creates from options, and never one it was given.
/// </summary>
public sealed class AdapterDisposalTests
{
    public static TheoryData<string> Adapters() =>
        [nameof(GeminiChatClient), nameof(GeminiEmbeddingGenerator), nameof(GeminiHostedFileClient)];

    [Theory]
    [MemberData(nameof(Adapters))]
    public async Task Dispose_WhenCreatedFromOptions_ShouldDisposeTheClientItCreated(string adapterName)
    {
        // Arrange
        var adapter = CreateFromOptions(adapterName, new GeminiClientOptions { ApiKey = "unused" });
        var client = GetGeminiClient(adapter);

        // Act
        adapter.Dispose();

        // Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(
            () => client.V1Beta.Files.GetAsync("files/file", TestContext.Current.CancellationToken));
    }

    [Theory]
    [MemberData(nameof(Adapters))]
    public async Task Dispose_WhenGivenAClient_ShouldLeaveItUsable(string adapterName)
    {
        // Arrange
        using var httpClient = new HttpClient(new StubHandler())
        {
            BaseAddress = new Uri("https://generativelanguage.googleapis.com"),
        };
        using var client = new GeminiClient(httpClient);
        var adapter = CreateFromClient(adapterName, client);

        // Act
        adapter.Dispose();

        // Assert
        var file = await client.V1Beta.Files.GetAsync("files/file", TestContext.Current.CancellationToken);
        Assert.Equal("files/file", file.Name);
    }

    private static IDisposable CreateFromOptions(string adapterName, GeminiClientOptions options) => adapterName switch
    {
        nameof(GeminiChatClient) => new GeminiChatClient(options),
        nameof(GeminiEmbeddingGenerator) => new GeminiEmbeddingGenerator(options),
        nameof(GeminiHostedFileClient) => new GeminiHostedFileClient(options),
        _ => throw new ArgumentOutOfRangeException(nameof(adapterName), adapterName, null),
    };

    private static IDisposable CreateFromClient(string adapterName, IGeminiClient client) => adapterName switch
    {
        nameof(GeminiChatClient) => new GeminiChatClient(client),
        nameof(GeminiEmbeddingGenerator) => new GeminiEmbeddingGenerator(client),
        nameof(GeminiHostedFileClient) => new GeminiHostedFileClient(client),
        _ => throw new ArgumentOutOfRangeException(nameof(adapterName), adapterName, null),
    };

    private static IGeminiClient GetGeminiClient(IDisposable adapter) => adapter switch
    {
        IChatClient chatClient => chatClient.GetService<IGeminiClient>()!,
        IEmbeddingGenerator generator => generator.GetService<IGeminiClient>()!,
        IHostedFileClient fileClient => fileClient.GetService<IGeminiClient>()!,
        _ => throw new ArgumentOutOfRangeException(nameof(adapter)),
    };

    private sealed class StubHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{ "name": "files/file" }""", Encoding.UTF8, "application/json"),
            });
    }
}
