using GeminiDotnet.Testing;
using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.Media;
using System.Net;
using System.Text;

namespace GeminiDotnet;

public sealed class FilesClientUploadTests
{
    private const string UploadUrl = "https://upload.example.com/resume?upload_id=abc123";

    [Fact]
    public async Task UploadFileAsync_ShouldFollowResumableUploadProtocol()
    {
        // Arrange
        const string displayName = "sales-q4.csv";
        const string mimeType = "text/csv";
        var fileContent = "name,revenue\nWidget A,1000\nWidget B,2000"u8.ToArray();

        var handler = new ScriptedHttpMessageHandler()
            .RespondWith(UploadStarted)
            .RespondWithJson(
                """
                {
                  "file": {
                    "name": "files/generated-id",
                    "uri": "https://generativelanguage.googleapis.com/v1beta/files/generated-id",
                    "displayName": "sales-q4.csv",
                    "state": "ACTIVE"
                  }
                }
                """);
        var client = CreateClient(handler);

        using var stream = new MemoryStream(fileContent);

        // Act
        var result = await client.UploadFileAsync(
            stream,
            fileContent.Length,
            new UploadFileOptions { DisplayName = displayName, MimeType = mimeType },
            TestContext.Current.CancellationToken);

        // Assert — verify the returned file metadata
        Assert.Equal("files/generated-id", result.Name);
        Assert.Equal("https://generativelanguage.googleapis.com/v1beta/files/generated-id", result.Uri);
        Assert.Equal(displayName, result.DisplayName);

        // Assert — verify the two-step protocol was followed
        Assert.Equal(2, handler.Requests.Count);

        // Step 1: Initiate
        var initRequest = handler.Requests[0];
        Assert.Equal(HttpMethod.Post, initRequest.Method);
        Assert.Contains("/upload/v1beta/files", initRequest.RequestUri!.PathAndQuery);
        Assert.Equal("resumable", initRequest.Headers["X-Goog-Upload-Protocol"]);
        Assert.Equal("start", initRequest.Headers["X-Goog-Upload-Command"]);
        Assert.Equal(fileContent.Length.ToString(), initRequest.Headers["X-Goog-Upload-Header-Content-Length"]);
        Assert.Equal(mimeType, initRequest.Headers["X-Goog-Upload-Header-Content-Type"]);

        // Step 2: Upload + finalize
        var uploadRequest = handler.Requests[1];
        Assert.Equal(HttpMethod.Post, uploadRequest.Method);
        Assert.Equal(UploadUrl, uploadRequest.RequestUri!.ToString());
        Assert.Equal("upload, finalize", uploadRequest.Headers["X-Goog-Upload-Command"]);
        Assert.Equal("0", uploadRequest.Headers["X-Goog-Upload-Offset"]);

        // Verify the actual file bytes were sent in the upload request
        Assert.Equal(fileContent, uploadRequest.ReadBodyAsBytes());
    }

    [Fact]
    public async Task UploadFileAsync_ShouldLeaveTheCallersStreamOpen()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler()
            .RespondWith(UploadStarted)
            .RespondWithJson("""{ "file": { "name": "files/open-file" } }""");
        var client = CreateClient(handler);

        using var stream = new MemoryStream([0x00, 0x01]);

        // Act
        await client.UploadFileAsync(stream, stream.Length, cancellationToken: TestContext.Current.CancellationToken);

        // Assert — the caller can rewind and reuse the stream, e.g. to retry
        stream.Position = 0;
        Assert.Equal(0x00, stream.ReadByte());
    }

    [Fact]
    public async Task UploadFileAsync_WithNoOptions_ShouldUseDefaultMimeType()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler()
            .RespondWith(UploadStarted)
            .RespondWithJson("""{ "file": { "name": "files/bin-file" } }""");
        var client = CreateClient(handler);

        using var stream = new MemoryStream([0x00, 0x01, 0x02]);

        // Act
        await client.UploadFileAsync(stream, stream.Length, cancellationToken: TestContext.Current.CancellationToken);

        // Assert — the default MIME type should be used
        Assert.Equal("application/octet-stream", handler.Requests[0].Headers["X-Goog-Upload-Header-Content-Type"]);
    }

    [Fact]
    public async Task UploadFileAsync_WhenInitiationMissesUploadUrl_ShouldThrow()
    {
        // Arrange — the start response omits the X-Goog-Upload-URL header
        var handler = new ScriptedHttpMessageHandler().RespondWithJson("{}");
        var client = CreateClient(handler);

        using var stream = new MemoryStream([0x00]);

        // Act
        Task Act() => client.UploadFileAsync(stream, 1, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(Act);
        Assert.Contains("X-Goog-Upload-URL", ex.Message);
    }

    [Fact]
    public async Task UploadFileAsync_WhenServerReturnsError_ShouldThrowGeminiClientException()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(
            """
            {
                "error": {
                    "code": 400,
                    "message": "Invalid file content.",
                    "status": "INVALID_ARGUMENT"
                }
            }
            """,
            HttpStatusCode.BadRequest);
        var client = CreateClient(handler);

        using var stream = new MemoryStream([0x00]);

        // Act
        Task Act() => client.UploadFileAsync(stream, 1, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        var ex = await Assert.ThrowsAsync<GeminiClientException>(Act);
        Assert.Contains("Invalid file content.", ex.Message);
    }

    private static IFilesClient CreateClient(ScriptedHttpMessageHandler handler)
    {
        return new GeminiClient(handler.CreateHttpClient()).V1Beta.Files;
    }

    /// <summary>
    /// The API answers the start of a resumable upload with the URL to send the bytes to in a header.
    /// </summary>
    private static HttpResponseMessage UploadStarted()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}", Encoding.UTF8, "application/json"),
        };
        response.Headers.Add("X-Goog-Upload-URL", UploadUrl);
        return response;
    }
}
