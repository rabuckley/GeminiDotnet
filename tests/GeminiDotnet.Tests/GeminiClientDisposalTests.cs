using System.Net;

namespace GeminiDotnet;

public sealed class GeminiClientDisposalTests
{
    [Fact]
    public async Task Dispose_WhenCreatedFromOptions_ShouldDisposeItsHttpClient()
    {
        // Arrange
        var client = new GeminiClient(new GeminiClientOptions { ApiKey = "unused" });

        // Act
        client.Dispose();

        // Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(
            () => client.V1Beta.Files.GetAsync("files/file", TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Dispose_WhenGivenAnHttpClient_ShouldLeaveItUsable()
    {
        // Arrange
        using var httpClient = new HttpClient(new StubHandler())
        {
            BaseAddress = new Uri("https://generativelanguage.googleapis.com"),
        };
        var client = new GeminiClient(httpClient);

        // Act
        client.Dispose();

        // Assert
        using var response = await httpClient.GetAsync("v1beta/files/file", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
    }
}
