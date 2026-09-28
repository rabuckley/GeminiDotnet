using GeminiDotnet.V1Beta;
using System.Net;
using System.Text;

namespace GeminiDotnet;

public sealed class MediaDownloadTests
{
    [Fact]
    public async Task Download_ShouldExposeTheBodyAndWhatTheResponseDeclared()
    {
        // Arrange
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("file bytes", Encoding.UTF8, "text/plain"),
        });

        // Act
        await using var download = await client.GetEnvironmentFilesHttpContentAsync("env", "dir/file.txt");
        using var reader = new StreamReader(download.Stream);

        // Assert
        Assert.Equal("file bytes", await reader.ReadToEndAsync());
        Assert.Equal("text/plain", download.MimeType);
        Assert.Equal(Encoding.UTF8.GetByteCount("file bytes"), download.Length);
    }

    [Fact]
    public async Task Download_WhenServerReturnsError_ShouldThrowAndReleaseTheResponse()
    {
        // Arrange
        var body = new TrackingContent(
            """{ "error": { "code": 404, "message": "No such file.", "status": "NOT_FOUND" } }""",
            "application/json");
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.NotFound) { Content = body });

        // Act
        var ex = await Assert.ThrowsAsync<GeminiClientException>(
            () => client.GetEnvironmentFilesHttpContentAsync("env", "missing.txt"));

        // Assert
        Assert.Contains("No such file.", ex.Message);
        Assert.True(body.IsDisposed);
    }

    private static EnvironmentsClient CreateClient(HttpResponseMessage response)
    {
        var httpClient = new HttpClient(new StubHandler(response))
        {
            BaseAddress = new Uri("https://generativelanguage.googleapis.com"),
        };
        return new EnvironmentsClient(new GeminiRequester(httpClient, V1BetaJsonContext.Default));
    }

    private sealed class StubHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            => Task.FromResult(response);
    }

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
