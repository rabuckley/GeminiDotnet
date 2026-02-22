using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.Files;
using System.Net;
using System.Text;
using System.Text.Json;
using File = GeminiDotnet.V1Beta.Files.File;

namespace GeminiDotnet;

public sealed class FilesClientUploadTests
{
    [Fact]
    public async Task UploadFileAsync_ShouldFollowResumableUploadProtocol()
    {
        // Arrange
        const string uploadUrl = "https://upload.example.com/resume?upload_id=abc123";
        const string displayName = "sales-q4.csv";
        const string mimeType = "text/csv";
        const string expectedFileName = "files/generated-id";
        const string expectedUri = "https://generativelanguage.googleapis.com/v1beta/files/generated-id";
        var fileContent = "name,revenue\nWidget A,1000\nWidget B,2000"u8.ToArray();

        var requestLog = new List<HttpRequestMessage>();

        var handler = new MockUploadHandler(
            uploadUrl,
            expectedFileName,
            expectedUri,
            displayName,
            requestLog);

        using var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://generativelanguage.googleapis.com") };
        var requester = new GeminiRequester(httpClient, V1BetaJsonContext.Default);
        var client = new FilesClient(requester);

        using var stream = new MemoryStream(fileContent);

        // Act
        var result = await client.UploadFileAsync(
            stream,
            fileContent.Length,
            new UploadFileOptions { DisplayName = displayName, MimeType = mimeType });

        // Assert — verify the returned file metadata
        Assert.Equal(expectedFileName, result.Name);
        Assert.Equal(expectedUri, result.Uri);
        Assert.Equal(displayName, result.DisplayName);

        // Assert — verify the two-step protocol was followed
        Assert.Equal(2, requestLog.Count);

        // Step 1: Initiate
        var initRequest = requestLog[0];
        Assert.Equal(HttpMethod.Post, initRequest.Method);
        Assert.Contains("/upload/v1beta/files", initRequest.RequestUri!.PathAndQuery);
        Assert.Equal("resumable", GetSingleHeaderValue(initRequest, "X-Goog-Upload-Protocol"));
        Assert.Equal("start", GetSingleHeaderValue(initRequest, "X-Goog-Upload-Command"));
        Assert.Equal(fileContent.Length.ToString(), GetSingleHeaderValue(initRequest, "X-Goog-Upload-Header-Content-Length"));
        Assert.Equal(mimeType, GetSingleHeaderValue(initRequest, "X-Goog-Upload-Header-Content-Type"));

        // Step 2: Upload + finalize
        var uploadRequest = requestLog[1];
        Assert.Equal(HttpMethod.Post, uploadRequest.Method);
        Assert.Equal(uploadUrl, uploadRequest.RequestUri!.ToString());
        Assert.Equal("upload, finalize", GetSingleHeaderValue(uploadRequest, "X-Goog-Upload-Command"));
        Assert.Equal("0", GetSingleHeaderValue(uploadRequest, "X-Goog-Upload-Offset"));

        // Verify the actual file bytes were sent in the upload request
        Assert.Equal(fileContent, handler.CapturedUploadBody);
    }

    [Fact]
    public async Task UploadFileAsync_WithNoOptions_ShouldUseDefaultMimeType()
    {
        // Arrange
        const string uploadUrl = "https://upload.example.com/resume?upload_id=default";
        var fileContent = new byte[] { 0x00, 0x01, 0x02 };

        var requestLog = new List<HttpRequestMessage>();

        var handler = new MockUploadHandler(
            uploadUrl,
            expectedFileName: "files/bin-file",
            expectedUri: "https://generativelanguage.googleapis.com/v1beta/files/bin-file",
            expectedDisplayName: null,
            requestLog);

        using var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://generativelanguage.googleapis.com") };
        var requester = new GeminiRequester(httpClient, V1BetaJsonContext.Default);
        var client = new FilesClient(requester);

        using var stream = new MemoryStream(fileContent);

        // Act
        var result = await client.UploadFileAsync(stream, fileContent.Length);

        // Assert — the default MIME type should be used
        var initRequest = requestLog[0];
        Assert.Equal("application/octet-stream", GetSingleHeaderValue(initRequest, "X-Goog-Upload-Header-Content-Type"));
    }

    [Fact]
    public async Task UploadFileAsync_WhenInitiationMissesUploadUrl_ShouldThrow()
    {
        // Arrange — handler that doesn't return the X-Goog-Upload-URL header
        var handler = new DelegatingHandlerFunc((request, ct) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json"),
            };
            // Intentionally omit X-Goog-Upload-URL header
            return Task.FromResult(response);
        });

        using var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://generativelanguage.googleapis.com") };
        var requester = new GeminiRequester(httpClient, V1BetaJsonContext.Default);
        var client = new FilesClient(requester);

        using var stream = new MemoryStream([0x00]);

        // Act
        Func<Task> act = () => client.UploadFileAsync(stream, 1);

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(act);
        Assert.Contains("X-Goog-Upload-URL", ex.Message);
    }

    [Fact]
    public async Task UploadFileAsync_WhenServerReturnsError_ShouldThrowGeminiClientException()
    {
        // Arrange — handler that returns a 400 error with a Gemini error body
        var handler = new DelegatingHandlerFunc((request, ct) =>
        {
            var errorBody = """
                {
                    "error": {
                        "code": 400,
                        "message": "Invalid file content.",
                        "status": "INVALID_ARGUMENT"
                    }
                }
                """;

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(errorBody, Encoding.UTF8, "application/json"),
            };
            return Task.FromResult(response);
        });

        using var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://generativelanguage.googleapis.com") };
        var requester = new GeminiRequester(httpClient, V1BetaJsonContext.Default);
        var client = new FilesClient(requester);

        using var stream = new MemoryStream([0x00]);

        // Act
        Func<Task> act = () => client.UploadFileAsync(stream, 1);

        // Assert
        var ex = await Assert.ThrowsAsync<GeminiClientException>(act);
        Assert.Contains("Invalid file content.", ex.Message);
    }

    private static string GetSingleHeaderValue(HttpRequestMessage request, string headerName)
    {
        Assert.True(request.Headers.TryGetValues(headerName, out var values),
            $"Expected header '{headerName}' to be present");
        return Assert.Single(values);
    }

    /// <summary>
    /// A mock handler that simulates the two-step Gemini resumable upload protocol.
    /// </summary>
    private sealed class MockUploadHandler(
        string uploadUrl,
        string expectedFileName,
        string expectedUri,
        string? expectedDisplayName,
        List<HttpRequestMessage> requestLog) : HttpMessageHandler
    {
        public byte[]? CapturedUploadBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            requestLog.Add(request);

            // Step 1: Initiation request → return upload URL in header
            if (request.RequestUri!.PathAndQuery.Contains("/upload/v1beta/files")
                && request.Headers.TryGetValues("X-Goog-Upload-Command", out var commands)
                && commands.First() == "start")
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{}", Encoding.UTF8, "application/json"),
                };
                response.Headers.Add("X-Goog-Upload-URL", uploadUrl);
                return response;
            }

            // Step 2: Upload + finalize → return file metadata
            if (request.RequestUri.ToString() == uploadUrl)
            {
                CapturedUploadBody = await request.Content!.ReadAsByteArrayAsync(cancellationToken);

                // The real Gemini API wraps the file in a CreateFileResponse envelope.
                var fileResponse = new CreateFileResponse
                {
                    File = new File
                    {
                        Name = expectedFileName,
                        Uri = expectedUri,
                        DisplayName = expectedDisplayName,
                        State = FileState.Active,
                    },
                };

                var json = JsonSerializer.Serialize(fileResponse, V1BetaJsonContext.Default.CreateFileResponse);
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json"),
                };
                return response;
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }

    /// <summary>
    /// Simple delegating handler that forwards to a func.
    /// </summary>
    private sealed class DelegatingHandlerFunc(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            => handler(request, cancellationToken);
    }
}
