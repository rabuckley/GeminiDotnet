using GeminiDotnet.Testing;
using GeminiDotnet.V1Beta.Files;
using Microsoft.Extensions.AI;
using System.Net;
using System.Text;
using File = GeminiDotnet.V1Beta.Files.File;

#pragma warning disable MEAI001 // Type is for evaluation purposes only

namespace GeminiDotnet.Extensions.AI;

public sealed class GeminiHostedFileClientTests
{
    #region ExtractResourceId

    [Theory]
    [InlineData("https://generativelanguage.googleapis.com/v1beta/files/abc123", "abc123")]
    [InlineData("files/abc123", "abc123")]
    [InlineData("abc123", "abc123")]
    [InlineData("https://example.com/files/xyz-456", "xyz-456")]
    public void ExtractResourceId_ReturnsExpectedId(string input, string expected)
    {
        // Act
        var result = GeminiHostedFileClient.ExtractResourceId(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region MapFileToHostedFileContent

    [Fact]
    public void MapFileToHostedFileContent_MapsAllProperties()
    {
        // Arrange
        var createdAt = new DateTimeOffset(2026, 1, 15, 10, 30, 0, TimeSpan.Zero);

        var file = new File
        {
            Uri = "https://generativelanguage.googleapis.com/v1beta/files/test-id",
            MimeType = "text/csv",
            DisplayName = "sales-data.csv",
            SizeBytes = 4096,
            CreateTime = createdAt,
            Name = "files/test-id",
        };

        // Act
        var result = GeminiHostedFileClient.MapFileToHostedFileContent(file);

        // Assert
        Assert.Equal(file.Uri, result.FileId);
        Assert.Equal(file.MimeType, result.MediaType);
        Assert.Equal(file.DisplayName, result.Name);
        Assert.Equal(file.SizeBytes, result.SizeInBytes);
        Assert.Equal(createdAt, result.CreatedAt);
        Assert.Same(file, result.RawRepresentation);
    }

    [Fact]
    public void MapFileToHostedFileContent_FallsBackToName_WhenUriIsNull()
    {
        // Arrange
        var file = new File { Name = "files/fallback-id" };

        // Act
        var result = GeminiHostedFileClient.MapFileToHostedFileContent(file);

        // Assert
        Assert.Equal("files/fallback-id", result.FileId);
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_OnSuccess()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson("{}");
        using var client = CreateClient(handler);

        // Act
        var result = await client.DeleteAsync("abc123", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result);
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Delete, request.Method);
        Assert.Equal("/v1beta/files/abc123", request.RequestUri?.AbsolutePath);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_On404()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(NotFoundJson, HttpStatusCode.NotFound);
        using var client = CreateClient(handler);

        // Act
        var result = await client.DeleteAsync("nonexistent", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region GetFileInfoAsync

    [Fact]
    public async Task GetFileInfoAsync_ReturnsNull_On404()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(NotFoundJson, HttpStatusCode.NotFound);
        using var client = CreateClient(handler);

        // Act
        var result = await client.GetFileInfoAsync("nonexistent", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetFileInfoAsync_ReturnsMappedContent_OnSuccess()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(
            """
            {
              "name": "files/found",
              "uri": "https://generativelanguage.googleapis.com/v1beta/files/found",
              "mimeType": "application/pdf",
              "displayName": "document.pdf"
            }
            """);
        using var client = CreateClient(handler);

        // Act
        var result = await client.GetFileInfoAsync("found", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("https://generativelanguage.googleapis.com/v1beta/files/found", result.FileId);
        Assert.Equal("application/pdf", result.MediaType);
        Assert.Equal("document.pdf", result.Name);
        Assert.Equal("/v1beta/files/found", Assert.Single(handler.Requests).RequestUri?.AbsolutePath);
    }

    #endregion

    #region ListFilesAsync

    [Fact]
    public async Task ListFilesAsync_RespectsLimit()
    {
        // Arrange — the page holds 5 files, more than the limit of 3 asks for.
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(
            ListFilesJson(nextPageToken: null, "file-0", "file-1", "file-2", "file-3", "file-4"));
        using var client = CreateClient(handler);

        var options = new HostedFileClientOptions { Limit = 3 };

        // Act
        List<HostedFileContent> results = [];
        await foreach (var item in client.ListFilesAsync(options, TestContext.Current.CancellationToken))
        {
            results.Add(item);
        }

        // Assert
        Assert.Equal(3, results.Count);
        Assert.Equal("?pageSize=3", Assert.Single(handler.Requests).RequestUri?.Query);
    }

    [Fact]
    public async Task ListFilesAsync_HandlesPagination()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler()
            .RespondWithJson(ListFilesJson(nextPageToken: "page2", "a", "b"))
            .RespondWithJson(ListFilesJson(nextPageToken: null, "c"));
        using var client = CreateClient(handler);

        // Act
        List<HostedFileContent> results = [];
        await foreach (var item in client.ListFilesAsync(cancellationToken: TestContext.Current.CancellationToken))
        {
            results.Add(item);
        }

        // Assert
        Assert.Equal(
            ["files/a", "files/b", "files/c"],
            results.Select(file => file.FileId));
        Assert.Equal(
            ["?pageSize=100", "?pageSize=100&pageToken=page2"],
            handler.Requests.Select(request => request.RequestUri?.Query));
    }

    #endregion

    #region UploadAsync

    [Fact]
    public async Task UploadAsync_PassesMediaTypeAndFileName()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler()
            .RespondWith(UploadStarted)
            .RespondWithJson(
                """
                {
                  "file": {
                    "name": "files/uploaded",
                    "uri": "https://generativelanguage.googleapis.com/v1beta/files/uploaded",
                    "mimeType": "text/csv",
                    "displayName": "data.csv"
                  }
                }
                """);
        using var client = CreateClient(handler);

        using var stream = new MemoryStream("hello"u8.ToArray());

        // Act
        var result = await client.UploadAsync(
            stream,
            "text/csv",
            "data.csv",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal("https://generativelanguage.googleapis.com/v1beta/files/uploaded", result.FileId);

        var start = handler.Requests[0];
        Assert.Equal("text/csv", start.Headers["X-Goog-Upload-Header-Content-Type"]);
        Assert.Equal("data.csv", start.ReadBodyAsJson()?["file"]?["displayName"]?.GetValue<string>());
    }

    [Fact]
    public async Task UploadAsync_ThrowsForNonSeekableStream()
    {
        // Arrange
        using var client = CreateClient(new ScriptedHttpMessageHandler());
        var nonSeekable = new NonSeekableStream();

        // Act
        Task Act() => client.UploadAsync(nonSeekable, "text/plain", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
    }

    #endregion

    #region DownloadAsync

    [Fact]
    public async Task DownloadAsync_ReturnsStreamWithCorrectMetadata()
    {
        // Arrange
        var handler = RespondToMetadataAndDownload(
            metadata: () => JsonResponse(
                """
                {
                  "name": "files/dl-test",
                  "uri": "https://generativelanguage.googleapis.com/v1beta/files/dl-test",
                  "mimeType": "text/plain",
                  "displayName": "report.txt"
                }
                """),
            download: () => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("file-content", Encoding.UTF8, "text/plain"),
            });
        using var client = CreateClient(handler);

        // Act
        await using var stream = await client.DownloadAsync(
            "dl-test",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal("text/plain", stream.MediaType);
        Assert.Equal("report.txt", stream.FileName);

        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync(TestContext.Current.CancellationToken);
        Assert.Equal("file-content", content);
    }

    [Fact]
    public async Task DownloadAsync_DisposesDownloadResult_WhenGetFileFails()
    {
        // Arrange — the metadata request fails with a 404, but the download succeeds. The download must be
        // disposed to avoid leaking its response.
        var downloaded = new TrackableStream("file-content"u8.ToArray());
        var handler = RespondToMetadataAndDownload(
            metadata: () => JsonResponse(NotFoundJson, HttpStatusCode.NotFound),
            download: () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(downloaded) });
        using var client = CreateClient(handler);

        // Act
        Task Act() => client.DownloadAsync("nonexistent", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<GeminiClientException>(Act);
        Assert.True(downloaded.IsDisposed);
    }

    [Fact]
    public async Task DownloadAsync_ExtractsResourceIdFromFullUri()
    {
        // Arrange
        var handler = RespondToMetadataAndDownload(
            metadata: () => JsonResponse("""{ "name": "files/uri-test" }"""),
            download: () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent([]) });
        using var client = CreateClient(handler);

        // Act
        await using var stream = await client.DownloadAsync(
            "https://generativelanguage.googleapis.com/v1beta/files/uri-test",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(
            ["/v1beta/files/uri-test", "/v1beta/files/uri-test:download"],
            handler.Requests.Select(request => request.RequestUri?.AbsolutePath).Order());
    }

    #endregion

    #region GeminiHostedFileDownloadStream

    [Fact]
    public async Task GeminiHostedFileDownloadStream_Dispose_DisposesUnderlyingResult()
    {
        // Arrange
        var contentStream = new TrackableStream("test"u8.ToArray());
        var download = new MediaDownload(contentStream, "application/octet-stream", contentStream.Length);
        var downloadStream = new GeminiHostedFileDownloadStream(download, "test.bin");

        // Act
        await downloadStream.DisposeAsync();

        // Assert
        Assert.True(contentStream.IsDisposed);
    }

    #endregion

    #region GetService

    [Fact]
    public void GetService_ReturnsMetadata()
    {
        // Arrange
        using var client = CreateClient(new ScriptedHttpMessageHandler());

        // Act
        var metadata = client.GetService(typeof(HostedFileClientMetadata));

        // Assert
        var typed = Assert.IsType<HostedFileClientMetadata>(metadata);
        Assert.Equal("Gemini", typed.ProviderName);
    }

    [Fact]
    public void GetService_ReturnsWrappedGeminiClient()
    {
        // Arrange
        using var geminiClient = new GeminiClient(new ScriptedHttpMessageHandler().CreateHttpClient());
        using var client = new GeminiHostedFileClient(geminiClient);

        // Act
        var result = client.GetService(typeof(IGeminiClient));

        // Assert
        Assert.Same(geminiClient, result);
    }

    [Fact]
    public void GetService_ReturnsSelf_ForMatchingType()
    {
        // Arrange
        using var client = CreateClient(new ScriptedHttpMessageHandler());

        // Act
        var result = client.GetService(typeof(GeminiHostedFileClient));

        // Assert
        Assert.Same(client, result);
    }

    [Fact]
    public void GetService_ReturnsNull_ForUnknownType()
    {
        // Arrange
        using var client = CreateClient(new ScriptedHttpMessageHandler());

        // Act
        var result = client.GetService(typeof(string));

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetService_ReturnsNull_WhenServiceKeyIsProvided()
    {
        // Arrange
        using var client = CreateClient(new ScriptedHttpMessageHandler());

        // Act — keyed services are not supported
        var result = client.GetService(typeof(HostedFileClientMetadata), serviceKey: "some-key");

        // Assert
        Assert.Null(result);
    }

    #endregion

    // -- Test helpers --

    private const string NotFoundJson =
        """{ "error": { "code": 404, "message": "File not found.", "status": "NOT_FOUND" } }""";

    private static GeminiHostedFileClient CreateClient(ScriptedHttpMessageHandler handler)
    {
        return new GeminiHostedFileClient(new GeminiClient(handler.CreateHttpClient()));
    }

    /// <summary>
    /// Scripts the two requests that <see cref="GeminiHostedFileClient.DownloadAsync"/> sends concurrently, each
    /// answered by what it asks for, so the test does not depend on which arrives first.
    /// </summary>
    private static ScriptedHttpMessageHandler RespondToMetadataAndDownload(
        Func<HttpResponseMessage> metadata,
        Func<HttpResponseMessage> download)
    {
        HttpResponseMessage Answer(RecordedRequest request) =>
            request.RequestUri?.AbsolutePath.EndsWith(":download", StringComparison.Ordinal) is true
                ? download()
                : metadata();

        return new ScriptedHttpMessageHandler().RespondWith(Answer).RespondWith(Answer);
    }

    private static HttpResponseMessage JsonResponse(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };
    }

    private static string ListFilesJson(string? nextPageToken, params string[] ids)
    {
        var files = string.Join(", ", ids.Select(id => $$"""{ "name": "files/{{id}}" }"""));
        var token = nextPageToken is null ? "" : $$""", "nextPageToken": "{{nextPageToken}}" """;
        return $$"""{ "files": [{{files}}]{{token}} }""";
    }

    /// <summary>
    /// The API answers the start of a resumable upload with the URL to send the bytes to in a header.
    /// </summary>
    private static HttpResponseMessage UploadStarted()
    {
        var response = JsonResponse("{}");
        response.Headers.Add("X-Goog-Upload-URL", "https://upload.example.com/resume?upload_id=abc123");
        return response;
    }

    /// <summary>
    /// A stream that is not seekable, used to test the seekable-stream requirement.
    /// </summary>
    private sealed class NonSeekableStream : Stream
    {
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
        public override void Flush() { }
        public override int Read(byte[] buffer, int offset, int count) => 0;
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }

    /// <summary>
    /// A <see cref="MemoryStream"/> that tracks whether it has been disposed.
    /// </summary>
    private sealed class TrackableStream(byte[] buffer) : MemoryStream(buffer)
    {
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
