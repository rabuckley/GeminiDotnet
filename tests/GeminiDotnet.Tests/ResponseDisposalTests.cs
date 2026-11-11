using GeminiDotnet.Testing;
using GeminiDotnet.V1Beta;
using System.Net;
using System.Text;

namespace GeminiDotnet;

public sealed class ResponseDisposalTests
{
    private const string ErrorJson =
        """{ "error": { "code": 400, "message": "Bad request.", "status": "INVALID_ARGUMENT" } }""";

    private static readonly GenerateContentRequest Request = new()
    {
        Contents = [new Content { Role = "user", Parts = [new Part { Text = "Hello" }] }],
    };

    [Fact]
    public async Task StreamGenerateContent_WhenConsumerStopsEarly_ShouldReleaseTheResponse()
    {
        // Arrange
        var body = new TrackingContent(ServerSentEvents("""{ "responseId": "1" }""", """{ "responseId": "2" }"""), "text/event-stream");
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK) { Content = body });

        // Act
        await foreach (var _ in client.StreamGenerateContentAsync("models/m", Request, TestContext.Current.CancellationToken))
        {
            break;
        }

        // Assert
        Assert.True(body.IsDisposed);
    }

    [Fact]
    public async Task StreamGenerateContent_WhenConsumerThrows_ShouldReleaseTheResponse()
    {
        // Arrange
        var body = new TrackingContent(ServerSentEvents("""{ "responseId": "1" }"""), "text/event-stream");
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK) { Content = body });

        // Act
        async Task Act()
        {
            await foreach (var _ in client.StreamGenerateContentAsync("models/m", Request, TestContext.Current.CancellationToken))
            {
                throw new InvalidOperationException("Consumer failed.");
            }
        }

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(Act);
        Assert.True(body.IsDisposed);
    }

    [Fact]
    public async Task StreamGenerateContent_WhenServerReturnsError_ShouldThrowAndReleaseTheResponse()
    {
        // Arrange
        var body = new TrackingContent(ErrorJson, "application/json");
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = body });

        // Act
        async Task Act()
        {
            await foreach (var _ in client.StreamGenerateContentAsync("models/m", Request, TestContext.Current.CancellationToken))
            {
            }
        }

        // Assert
        var ex = await Assert.ThrowsAsync<GeminiClientException>(Act);
        Assert.Contains("Bad request.", ex.Message);
        Assert.True(body.IsDisposed);
    }

    [Fact]
    public async Task GenerateContent_WhenServerReturnsError_ShouldThrowAndReleaseTheResponse()
    {
        // Arrange
        var body = new TrackingContent(ErrorJson, "application/json");
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = body });

        // Act
        async Task Act() => await client.GenerateContentAsync("models/m", Request, TestContext.Current.CancellationToken);

        // Assert
        var ex = await Assert.ThrowsAsync<GeminiClientException>(Act);
        Assert.Contains("Bad request.", ex.Message);
        Assert.True(body.IsDisposed);
    }

    [Fact]
    public async Task GenerateContent_WhenItSucceeds_ShouldReturnTheResultAndReleaseTheResponse()
    {
        // Arrange
        var body = new TrackingContent("""{ "responseId": "1" }""", "application/json");
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK) { Content = body });

        // Act
        var response = await client.GenerateContentAsync("models/m", Request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal("1", response.ResponseId);
        Assert.True(body.IsDisposed);
    }

    private static IModelsClient CreateClient(HttpResponseMessage response)
    {
        var handler = new ScriptedHttpMessageHandler().RespondWith(() => response);
        return new GeminiClient(handler.CreateHttpClient()).V1Beta.Models;
    }

    private static string ServerSentEvents(params string[] payloads)
        => string.Concat(payloads.Select(payload => $"data: {payload}\r\n\r\n"));

    private sealed class TrackingContent(string content, string mediaType)
        : StringContent(content, Encoding.UTF8, mediaType)
    {
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
