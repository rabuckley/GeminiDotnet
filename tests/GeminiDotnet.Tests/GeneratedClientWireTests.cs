using GeminiDotnet.Testing;

namespace GeminiDotnet;

/// <summary>
/// Pins how the generated clients build a request URI, one test per shape of path, through the HTTP boundary.
/// </summary>
public sealed class GeneratedClientWireTests
{
    [Fact]
    public async Task ListAsync_WithQueryParameters_ShouldSendThemEscaped()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson("""{ "models": [] }""");
        var client = CreateClient(handler);

        // Act
        await client.V1Beta.Models.ListAsync(
            pageSize: 10,
            pageToken: "next page/2",
            TestContext.Current.CancellationToken);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/v1beta/models?pageSize=10&pageToken=next%20page%2F2", request.RequestUri?.PathAndQuery);
    }

    [Fact]
    public async Task ListAsync_WithoutQueryParameters_ShouldSendNoQueryString()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson("""{ "models": [] }""");
        var client = CreateClient(handler);

        // Act
        await client.V1Beta.Models.ListAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/v1beta/models", request.RequestUri?.PathAndQuery);
    }

    [Fact]
    public async Task GetAsync_WithAWildcardPath_ShouldKeepItsSlashesAndEscapeEachSegment()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson("""{ "name": "batches/my batch" }""");
        var client = CreateClient(handler);

        // Act
        await client.V1Beta.Batches.GetAsync("batches/my batch", TestContext.Current.CancellationToken);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/v1beta/batches/my%20batch", request.RequestUri?.PathAndQuery);
    }

    [Fact]
    public async Task GetAsync_WithAMultiSegmentWildcardPath_ShouldKeepEverySlashAndEscapeEachSegment()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler()
            .RespondWithJson("""{ "name": "fileSearchStores/my store/documents/my doc" }""");
        var client = CreateClient(handler);

        // Act
        await client.V1Beta.FileSearchStores.Documents.GetAsync(
            "fileSearchStores/my store/documents/my doc",
            TestContext.Current.CancellationToken);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/v1beta/fileSearchStores/my%20store/documents/my%20doc", request.RequestUri?.PathAndQuery);
    }

    private static GeminiClient CreateClient(ScriptedHttpMessageHandler handler)
    {
        return new GeminiClient(handler.CreateHttpClient());
    }
}
