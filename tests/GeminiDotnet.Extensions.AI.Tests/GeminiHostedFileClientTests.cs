using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.Files;
using Microsoft.Extensions.AI;
using System.Net;
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
        var filesClient = new StubFilesClient();
        var client = CreateClient(filesClient);

        // Act
        var result = await client.DeleteAsync("abc123");

        // Assert
        Assert.True(result);
        Assert.Equal("abc123", filesClient.LastDeletedFile);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_OnGeminiClientException()
    {
        // Arrange
        var filesClient = new StubFilesClient
        {
            ThrowOnDelete = true,
        };
        var client = CreateClient(filesClient);

        // Act
        var result = await client.DeleteAsync("nonexistent");

        // Assert
        Assert.False(result);
    }

    #endregion

    #region GetFileInfoAsync

    [Fact]
    public async Task GetFileInfoAsync_ReturnsNull_On404()
    {
        // Arrange
        var filesClient = new StubFilesClient
        {
            ThrowNotFoundOnGet = true,
        };
        var client = CreateClient(filesClient);

        // Act
        var result = await client.GetFileInfoAsync("nonexistent");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetFileInfoAsync_ReturnsMappedContent_OnSuccess()
    {
        // Arrange
        var file = new File
        {
            Uri = "https://generativelanguage.googleapis.com/v1beta/files/found",
            MimeType = "application/pdf",
            DisplayName = "document.pdf",
        };
        var filesClient = new StubFilesClient { FileToReturn = file };
        var client = CreateClient(filesClient);

        // Act
        var result = await client.GetFileInfoAsync("found");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(file.Uri, result.FileId);
        Assert.Equal(file.MimeType, result.MediaType);
    }

    #endregion

    #region ListFilesAsync

    [Fact]
    public async Task ListFilesAsync_RespectsLimit()
    {
        // Arrange — stub returns 5 files, but we request a limit of 3.
        var files = Enumerable.Range(0, 5).Select(i => new File
        {
            Uri = $"https://generativelanguage.googleapis.com/v1beta/files/file-{i}",
            Name = $"files/file-{i}",
        }).ToList();

        var filesClient = new StubFilesClient
        {
            FilesToList = files,
        };
        var client = CreateClient(filesClient);

        var options = new HostedFileClientOptions { Limit = 3 };

        // Act
        var results = new List<HostedFileContent>();
        await foreach (var item in client.ListFilesAsync(options))
        {
            results.Add(item);
        }

        // Assert
        Assert.Equal(3, results.Count);
    }

    [Fact]
    public async Task ListFilesAsync_HandlesPagination()
    {
        // Arrange — stub will return 2 pages of 2 files each.
        var page1Files = new List<File>
        {
            new() { Uri = "https://example.com/files/a", Name = "files/a" },
            new() { Uri = "https://example.com/files/b", Name = "files/b" },
        };
        var page2Files = new List<File>
        {
            new() { Uri = "https://example.com/files/c", Name = "files/c" },
        };

        var filesClient = new StubFilesClient
        {
            PaginatedResults =
            [
                new ListFilesResponse { Files = page1Files, NextPageToken = "page2" },
                new ListFilesResponse { Files = page2Files, NextPageToken = null },
            ],
        };
        var client = CreateClient(filesClient);

        // Act
        var results = new List<HostedFileContent>();
        await foreach (var item in client.ListFilesAsync())
        {
            results.Add(item);
        }

        // Assert
        Assert.Equal(3, results.Count);
    }

    #endregion

    #region UploadAsync

    [Fact]
    public async Task UploadAsync_PassesMediaTypeAndFileName()
    {
        // Arrange
        var uploadedFile = new File
        {
            Uri = "https://generativelanguage.googleapis.com/v1beta/files/uploaded",
            MimeType = "text/csv",
            DisplayName = "data.csv",
            Name = "files/uploaded",
        };

        var filesClient = new StubFilesClient { FileToReturn = uploadedFile };
        var client = CreateClient(filesClient);

        using var stream = new MemoryStream("hello"u8.ToArray());

        // Act
        var result = await client.UploadAsync(stream, "text/csv", "data.csv");

        // Assert
        Assert.Equal(uploadedFile.Uri, result.FileId);
        Assert.Equal("text/csv", filesClient.LastUploadOptions?.MimeType);
        Assert.Equal("data.csv", filesClient.LastUploadOptions?.DisplayName);
    }

    [Fact]
    public async Task UploadAsync_ThrowsForNonSeekableStream()
    {
        // Arrange
        var client = CreateClient(new StubFilesClient());
        var nonSeekable = new NonSeekableStream();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => client.UploadAsync(nonSeekable, "text/plain"));
    }

    #endregion

    #region DownloadAsync

    [Fact]
    public async Task DownloadAsync_ReturnsStreamWithCorrectMetadata()
    {
        // Arrange
        var file = new File
        {
            Uri = "https://generativelanguage.googleapis.com/v1beta/files/dl-test",
            MimeType = "text/plain",
            DisplayName = "report.txt",
            Name = "files/dl-test",
        };

        var filesClient = new StubFilesClient { FileToReturn = file };
        var client = CreateClient(filesClient);

        // Act
        await using var stream = await client.DownloadAsync("dl-test");

        // Assert
        Assert.Equal("text/plain", stream.MediaType);
        Assert.Equal("report.txt", stream.FileName);

        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();
        Assert.Equal("file-content", content);
    }

    [Fact]
    public async Task DownloadAsync_DisposesDownloadResult_WhenGetFileFails()
    {
        // Arrange — GetFileAsync returns a faulted task (404), but
        // DownloadFileStreamAsync succeeds. The download result must be disposed
        // to avoid leaking the underlying HttpResponseMessage.
        var filesClient = new StubFilesClient
        {
            FaultGetFileAsync = true,
        };
        var client = CreateClient(filesClient);

        // Act & Assert
        await Assert.ThrowsAsync<GeminiClientException>(() => client.DownloadAsync("nonexistent"));

        // The download response should have been disposed by the catch block.
        Assert.True(filesClient.LastDownloadResponse?.IsDisposed ?? false);
    }

    [Fact]
    public async Task DownloadAsync_ExtractsResourceIdFromFullUri()
    {
        // Arrange
        var filesClient = new StubFilesClient();
        var client = CreateClient(filesClient);

        // Act
        await using var stream = await client.DownloadAsync(
            "https://generativelanguage.googleapis.com/v1beta/files/uri-test");

        // Assert — the stub records the resource ID passed to GetFileAsync
        Assert.Equal("uri-test", filesClient.LastGetFileId);
    }

    #endregion

    #region Dispose

    [Fact]
    public void Dispose_DisposesOwnedClient()
    {
        // Arrange — use a disposable stub to verify Dispose is called
        var disposableClient = new DisposableStubGeminiClient();
        var client = new GeminiHostedFileClient(disposableClient);

        // Act — disposing the adapter should NOT dispose the client it doesn't own
        client.Dispose();

        // Assert
        Assert.False(disposableClient.IsDisposed);
    }

    #endregion

    #region GeminiHostedFileDownloadStream

    [Fact]
    public async Task GeminiHostedFileDownloadStream_Dispose_DisposesUnderlyingResult()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent("test"u8.ToArray()),
        };
        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

        var result = new FileDownloadResult(response);
        var contentStream = await result.GetContentStreamAsync();
        var downloadStream = new GeminiHostedFileDownloadStream(result, contentStream, "test.bin");

        // Act
        await downloadStream.DisposeAsync();

        // Assert — reading from the content stream after disposal should throw
        var readFromDisposed = () => response.Content.ReadAsStream();
        Assert.Throws<ObjectDisposedException>(readFromDisposed);
    }

    #endregion

    #region GetService

    [Fact]
    public void GetService_ReturnsMetadata()
    {
        // Arrange
        var client = CreateClient(new StubFilesClient());

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
        var geminiClient = new StubGeminiClient();
        var client = new GeminiHostedFileClient(geminiClient);

        // Act
        var result = client.GetService(typeof(IGeminiClient));

        // Assert
        Assert.Same(geminiClient, result);
    }

    [Fact]
    public void GetService_ReturnsSelf_ForMatchingType()
    {
        // Arrange
        var client = CreateClient(new StubFilesClient());

        // Act
        var result = client.GetService(typeof(GeminiHostedFileClient));

        // Assert
        Assert.Same(client, result);
    }

    [Fact]
    public void GetService_ReturnsNull_ForUnknownType()
    {
        // Arrange
        var client = CreateClient(new StubFilesClient());

        // Act
        var result = client.GetService(typeof(string));

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetService_ReturnsNull_WhenServiceKeyIsProvided()
    {
        // Arrange
        var client = CreateClient(new StubFilesClient());

        // Act — keyed services are not supported
        var result = client.GetService(typeof(HostedFileClientMetadata), serviceKey: "some-key");

        // Assert
        Assert.Null(result);
    }

    #endregion

    // -- Test helpers --

    private static GeminiHostedFileClient CreateClient(StubFilesClient filesClient)
    {
        var geminiClient = new StubGeminiClient { FilesClientOverride = filesClient };
        return new GeminiHostedFileClient(geminiClient);
    }

    /// <summary>
    /// A minimal <see cref="IGeminiClient"/> stub that delegates to a configurable <see cref="IFilesClient"/>.
    /// </summary>
    private sealed class StubGeminiClient : IGeminiClient
    {
        public StubFilesClient? FilesClientOverride { get; init; }

        public IGeminiClientOptions Options => throw new NotImplementedException();
        public Uri? Endpoint => new("https://generativelanguage.googleapis.com");
        public V1.IGeminiV1Client V1 => throw new NotImplementedException();
        public IGeminiV1BetaClient V1Beta => new StubV1BetaClient(FilesClientOverride ?? new StubFilesClient());
    }

    private sealed class StubV1BetaClient : IGeminiV1BetaClient
    {
        private readonly IFilesClient _files;

        public StubV1BetaClient(IFilesClient files)
        {
            _files = files;
        }

        public IBatchesClient Batches => throw new NotImplementedException();
        public ICachedContentsClient CachedContents => throw new NotImplementedException();
        public ICorporaClient Corpora => throw new NotImplementedException();
        public IDynamicClient Dynamic => throw new NotImplementedException();
        public IFilesClient Files => _files;
        public IFileSearchStoresClient FileSearchStores => throw new NotImplementedException();
        public IFilesRegisterClient FilesRegister => throw new NotImplementedException();
        public IGeneratedFilesClient GeneratedFiles => throw new NotImplementedException();
        public IModelsClient Models => throw new NotImplementedException();
        public ITunedModelsClient TunedModels => throw new NotImplementedException();
    }

    /// <summary>
    /// Configurable stub for <see cref="IFilesClient"/> that records calls and returns pre-set values.
    /// </summary>
    private sealed class StubFilesClient : IFilesClient
    {
        public File? FileToReturn { get; set; }
        public List<File>? FilesToList { get; set; }
        public List<ListFilesResponse>? PaginatedResults { get; set; }
        public bool ThrowOnDelete { get; set; }
        public bool ThrowNotFoundOnGet { get; set; }
        public bool FaultGetFileAsync { get; set; }
        public string? LastDeletedFile { get; set; }
        public string? LastGetFileId { get; set; }
        public UploadFileOptions? LastUploadOptions { get; set; }
        public TrackableHttpResponseMessage? LastDownloadResponse { get; set; }

        private int _paginationIndex;

        public Task<ListFilesResponse> ListFilesAsync(int? pageSize = null, string? pageToken = null, CancellationToken cancellationToken = default)
        {
            if (PaginatedResults is not null)
            {
                var response = _paginationIndex < PaginatedResults.Count
                    ? PaginatedResults[_paginationIndex++]
                    : new ListFilesResponse();
                return Task.FromResult(response);
            }

            // Simple single-page response from FilesToList.
            return Task.FromResult(new ListFilesResponse
            {
                Files = FilesToList,
            });
        }

        public Task<CreateFileResponse> CreateFileAsync(CreateFileRequest request, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<File> GetFileAsync(string file, CancellationToken cancellationToken = default)
        {
            LastGetFileId = file;

            if (ThrowNotFoundOnGet)
            {
                ThrowGeminiException(HttpStatusCode.NotFound);
            }

            if (FaultGetFileAsync)
            {
                // Return a faulted task instead of throwing synchronously, so that
                // callers who already started concurrent tasks can observe the fault
                // via Task.WhenAll.
                return Task.FromException<File>(CreateGeminiException(HttpStatusCode.NotFound));
            }

            return Task.FromResult(FileToReturn ?? new File { Name = $"files/{file}" });
        }

        public Task<Empty> DeleteFileAsync(string file, CancellationToken cancellationToken = default)
        {
            if (ThrowOnDelete)
            {
                ThrowGeminiException(HttpStatusCode.NotFound);
            }

            LastDeletedFile = file;
            return Task.FromResult(new Empty());
        }

        public Task<DownloadFileResponse> DownloadFileAsync(string file, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<FileDownloadResult> DownloadFileStreamAsync(string file, CancellationToken cancellationToken = default)
        {
            // Return a result wrapping a simple memory-backed response, using a
            // trackable message so tests can verify disposal.
            var response = new TrackableHttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent("file-content"u8.ToArray()),
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
            LastDownloadResponse = response;
            return Task.FromResult(new FileDownloadResult(response));
        }

        public Task<File> UploadFileAsync(Stream content, long contentLength, UploadFileOptions? options = null, CancellationToken cancellationToken = default)
        {
            LastUploadOptions = options;
            return Task.FromResult(FileToReturn ?? new File { Name = "files/uploaded" });
        }
    }

    /// <summary>
    /// Throws a <see cref="GeminiClientException"/> with the given status code.
    /// Calls the internal <see cref="GeminiClientException.Throw"/> method directly
    /// via <c>InternalsVisibleTo</c>.
    /// </summary>
    private static void ThrowGeminiException(HttpStatusCode statusCode)
    {
        GeminiClientException.Throw(CreateErrorDetails(statusCode));
    }

    /// <summary>
    /// Creates a <see cref="GeminiClientException"/> that can be used with
    /// <see cref="Task.FromException{TResult}"/> instead of throwing synchronously.
    /// </summary>
    private static GeminiClientException CreateGeminiException(HttpStatusCode statusCode)
    {
        try
        {
            GeminiClientException.Throw(CreateErrorDetails(statusCode));
            return null!; // Unreachable
        }
        catch (GeminiClientException ex)
        {
            return ex;
        }
    }

    private static ErrorDetails CreateErrorDetails(HttpStatusCode statusCode) => new()
    {
        StatusCode = statusCode,
        Message = $"Error {(int)statusCode}",
        Status = statusCode.ToString(),
    };

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
    /// An <see cref="HttpResponseMessage"/> subclass that tracks whether it has been disposed.
    /// </summary>
    private sealed class TrackableHttpResponseMessage : HttpResponseMessage
    {
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// A disposable <see cref="IGeminiClient"/> stub for testing ownership semantics.
    /// </summary>
    private sealed class DisposableStubGeminiClient : IGeminiClient, IDisposable
    {
        public bool IsDisposed { get; private set; }

        public IGeminiClientOptions Options => throw new NotImplementedException();
        public Uri? Endpoint => new("https://generativelanguage.googleapis.com");
        public V1.IGeminiV1Client V1 => throw new NotImplementedException();
        public IGeminiV1BetaClient V1Beta => throw new NotImplementedException();

        public void Dispose() => IsDisposed = true;
    }
}
