using GeminiDotnet.Testing;
using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.FileSearchStores;
using Microsoft.Extensions.AI;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace GeminiDotnet.Extensions.AI;

[IntegrationTest]
public sealed class GeminiChatClientTests
{
    private readonly ITestOutputHelper _output;
    private readonly string _apiKey;
    private const string Model = TestConfiguration.DefaultModel;

    public GeminiChatClientTests(ITestOutputHelper output)
    {
        _output = output;
        _apiKey = TestConfiguration.GetApiKey();
    }

    [Fact]
    public async Task GetStreamingResponseAsync_WithParameterlessFunction()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var geminiClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey, ModelId = Model,
        });

        const string weather = "It's raining.";
        var calls = 0;

        [Description("Gets the current weather")]
        string GetCurrentWeather()
        {
            calls++;
            return weather;
        }

        IChatClient client = new ChatClientBuilder(geminiClient)
            .UseFunctionInvocation()
            .Build();

        var messages = new List<ChatMessage>
        {
            new(ChatRole.User, "Should I wear a rain coat? Get the current weather if needed.")
        };

        var options = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(GetCurrentWeather, nameof(GetCurrentWeather))]
        };

        // Act
        var updates = new List<ChatResponseUpdate>();

        await foreach (var update in client.GetStreamingResponseAsync(messages, options, cancellationToken))
        {
            updates.Add(update);
            _output.Write(update.Text);
        }

        // Assert
        // The model's wording is its own business; what this test owns is that the tool was offered,
        // called, and its result fed back into the same exchange.
        Assert.Equal(1, calls);
        AssertFunctionRoundTrip(updates, nameof(GetCurrentWeather), weather);
        Assert.NotEmpty(updates.ToChatResponse().Text);

        // A follow-up turn must accept the assistant and tool messages the first turn produced.
        messages.AddRange(updates.ToChatResponse().Messages);
        messages.Add(new ChatMessage(ChatRole.User, "Thanks, I'll wear a rain coat."));

        var followUp = new List<ChatResponseUpdate>();

        await foreach (var update in client.GetStreamingResponseAsync(messages, options, cancellationToken))
        {
            followUp.Add(update);
            _output.Write(update.Text);
        }

        Assert.NotEmpty(followUp.ToChatResponse().Text);
    }

    [Fact]
    public async Task GetStreamingResponseAsync_WithFunctionWithParameters()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var geminiClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey, ModelId = Model,
        });

        var arguments = new List<(string Location, DateOnly Date)>();

        [Description("Gets the current weather")]
        string GetCurrentWeather(string location, DateOnly date)
        {
            arguments.Add((location, date));
            return $"It's raining in {location}.";
        }

        IChatClient client = new ChatClientBuilder(geminiClient)
            .UseFunctionInvocation()
            .Build();

        var messages = new List<ChatMessage>
        {
            new(ChatRole.User,
                "Should I wear a rain coat in London tomorrow (1st Oct, 2000)? Get the current weather if needed using YYYY-MM-DD format.")
        };

        var options = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(GetCurrentWeather, nameof(GetCurrentWeather))]
        };

        // Act
        var updates = new List<ChatResponseUpdate>();

        await foreach (var update in client.GetStreamingResponseAsync(messages, options, cancellationToken))
        {
            updates.Add(update);
            _output.Write(update.Text);
        }

        // Assert
        var (location, date) = Assert.Single(arguments);
        Assert.Equal("London", location, StringComparer.OrdinalIgnoreCase);
        Assert.Equal(new DateOnly(2000, 10, 1), date);
        AssertFunctionRoundTrip(updates, nameof(GetCurrentWeather), "It's raining in London.");
        Assert.NotEmpty(updates.ToChatResponse().Text);
    }

    /// <summary>
    /// Asserts that the streamed updates carry a call to <paramref name="functionName"/> and the
    /// matching result, correlated by call id — the round trip the function-invoking client is
    /// responsible for, independent of how the model words its final answer.
    /// </summary>
    private static void AssertFunctionRoundTrip(
        IEnumerable<ChatResponseUpdate> updates,
        string functionName,
        string expectedResult)
    {
        var contents = updates.SelectMany(u => u.Contents).ToList();

        var call = Assert.Single(contents.OfType<FunctionCallContent>(), c => c.Name == functionName);
        var result = Assert.Single(contents.OfType<FunctionResultContent>(), r => r.CallId == call.CallId);

        Assert.Null(result.Exception);
        // The function-invoking client hands the return value back as its serialized JSON form.
        Assert.Equal(expectedResult, result.Result?.ToString());
    }

    /// <summary>
    /// https://github.com/rabuckley/GeminiDotnet/issues/7
    /// </summary>
    [Fact]
    public void ToChatResponse_WithToolCall_RegressionTest()
    {
        // Arrange
        var callId = Guid.NewGuid().ToString();
        const string name = "GetCapitalCity";
        var arguments = new Dictionary<string, object?> { { "country", "France" } };

        List<ChatResponseUpdate> updates =
        [
            new(ChatRole.Assistant, [new FunctionCallContent(callId, name, arguments)]),
            new(ChatRole.Tool, [new FunctionResultContent(callId, "Paris")]),
            new(ChatRole.Assistant, [new TextContent("Paris is the capital of France.")]),
        ];

        // Act
        var response = updates.ToChatResponse();

        // Assert
        Assert.Equal(3, response.Messages.Count);
    }

    [Fact]
    public async Task InstructionAndSystemMessage()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        var geminiClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey, ModelId = Model,
        });

        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, "You are a helpful assistant that translates text."),
            new(ChatRole.User, "Translate the following text to French: 'Hello, how are you?'"),
        };

        var options = new ChatOptions { Instructions = "Please provide a concise translation.", };

        var response = geminiClient.GetStreamingResponseAsync(messages, options, cancellationToken);

        var sb = new StringBuilder();

        await foreach (var update in response)
        {
            foreach (var content in update.Contents)
            {
                sb.Append(content);
                _output.Write(content.ToString() ?? "<null>");
            }
        }
    }

    [Fact]
    public async Task FunctionCallingExample()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        IChatClient geminiClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey,
            ModelId = Model,
        });

        [Description("Gets the current weather")]
        static string GetCurrentWeather(string location, DateOnly date)
        {
            return $"It's raining in {location} on {date}.";
        }

        IChatClient client = new ChatClientBuilder(geminiClient)
            .UseFunctionInvocation()
            .Build();

        List<ChatMessage> messages =
        [
            new(ChatRole.User,
                "Should I wear a rain coat in London tomorrow (1st Oct, 2000)? Get the current weather using the function if needed.")
        ];

        var options = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(GetCurrentWeather, nameof(GetCurrentWeather))]
        };

        var response = await client.GetResponseAsync(messages, options, cancellationToken);

        messages.AddRange(response.Messages);
        messages.Add(new ChatMessage(ChatRole.User, "Thanks!"));

        var response2 = await client.GetResponseAsync(messages, options, cancellationToken);

        messages.AddRange(response2.Messages);

        Assert.All(
            messages.Where(m => m.Contents.Any(c => c is TextReasoningContent)),
            content => Assert.All(content.Contents.OfType<TextReasoningContent>(),
                reasoningContent => Assert.NotNull(reasoningContent.ProtectedData)));
    }

    record WeatherInfo(string Location, DateOnly Date, string Summary);

    [Fact]
    public async Task FunctionCalling_WithObjectReturnType()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        IChatClient geminiClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey, ModelId = Model,
        });

        [Description("Gets the current weather")]
        static WeatherInfo GetCurrentWeather(string location, DateOnly date)
        {
            return new WeatherInfo(location, date, $"It's raining in {location} on {date}.");
        }

        IChatClient client = new ChatClientBuilder(geminiClient)
            .UseFunctionInvocation()
            .Build();

        List<ChatMessage> messages =
        [
            new(ChatRole.User,
                "Get the current weather in London tomorrow (2000-10-01) using the function.")
        ];

        var options = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(GetCurrentWeather, nameof(GetCurrentWeather))]
        };

        var response = await client.GetResponseAsync(messages, options, cancellationToken);

        var functionCall = response.Messages
            .SelectMany(m => m.Contents)
            .OfType<FunctionCallContent>()
            .FirstOrDefault();

        Assert.NotNull(functionCall);
        Assert.Equal(nameof(GetCurrentWeather), functionCall.Name);
        Assert.NotNull(functionCall.Arguments);
        Assert.Equal("London", functionCall.Arguments["location"]?.ToString());
        Assert.Equal(DateOnly.Parse("2000-10-01"), DateOnly.Parse(functionCall.Arguments["date"]!.ToString()!));
    }

    [Fact]
    public async Task GetResponseAsync_WithHostedFileSearchTool_ShouldGroundTheAnswerInTheStore()
    {
        // Arrange — no store fixture exists, so this test owns the whole lifecycle.
        var cancellationToken = TestContext.Current.CancellationToken;

        var client = new GeminiClient(new GeminiClientOptions { ApiKey = _apiKey, ModelId = Model });
        var stores = client.V1Beta.FileSearchStores;

        // A fact the model cannot know, so an answer containing it can only have come from the store.
        const string fact = "The Aldbourne Reading Room was founded in 1873 by Hester Vane.";

        var store = await stores.CreateFileSearchStoreAsync(
            new FileSearchStore { DisplayName = $"gemini-dotnet-test-{Guid.NewGuid():N}" },
            cancellationToken);

        Assert.NotNull(store.Name);
        var storeId = store.Name["fileSearchStores/".Length..];
        string? uploadedFileId = null;

        try
        {
            var fileName = await UploadDocumentAsync(client, fact, cancellationToken);
            uploadedFileId = fileName["files/".Length..];

            await ImportDocumentAsync(client, storeId, fileName, cancellationToken);

            IChatClient chatClient = new GeminiChatClient(new GeminiClientOptions
            {
                ApiKey = _apiKey, ModelId = Model,
            });

            var options = new ChatOptions
            {
                Tools =
                [
                    new HostedFileSearchTool { Inputs = [new HostedVectorStoreContent(store.Name)] },
                ],
            };

            // Act
            var response = await chatClient.GetResponseAsync(
                "Who founded the Aldbourne Reading Room, and in what year?",
                options,
                cancellationToken);

            // Assert
            _output.WriteLine(response.Text);

            Assert.Contains("Hester Vane", response.Text, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("1873", response.Text, StringComparison.Ordinal);

            var citations = response.Messages
                .SelectMany(m => m.Contents)
                .OfType<TextContent>()
                .SelectMany(t => t.Annotations ?? [])
                .OfType<CitationAnnotation>()
                .ToList();

            Assert.Contains(citations, c => c.ToolName == GeminiToolNames.FileSearch);
        }
        finally
        {
            // Nested so a failure deleting the store cannot leave the uploaded file behind in the
            // shared project, where nothing else would ever notice it.
            try
            {
                await stores.DeleteFileSearchStoreAsync(storeId, force: true, CancellationToken.None);
            }
            finally
            {
                if (uploadedFileId is not null)
                {
                    // Importing copies the file into the store, so the upload is the test's to clean up.
                    await client.V1Beta.Files.DeleteFileAsync(uploadedFileId, CancellationToken.None);
                }
            }
        }
    }


    [Fact]
    public async Task GetResponseAsync_WithHostedMcpServerTool_ShouldAnswerFromTheRemoteServer()
    {
        // Arrange — Google's public demo MCP server, which exposes weather tools. Gemini connects to it
        // and runs the tools server-side, so nothing here handles a tool call.
        var cancellationToken = TestContext.Current.CancellationToken;

        IChatClient chatClient = new GeminiChatClient(new GeminiClientOptions
        {
            ApiKey = _apiKey, ModelId = Model,
        });

        var options = new ChatOptions
        {
            Tools =
            [
                new HostedMcpServerTool("weather", "https://gemini-api-demos.uc.r.appspot.com/mcp")
                {
                    ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire,
                },
            ],
        };

        // Act
        var response = await chatClient.GetResponseAsync(
            "Will it rain in London tomorrow? Use the weather tools.",
            options,
            cancellationToken);

        // Assert — the demo server answers the call itself with "not implemented", so the model's prose says
        // nothing about whether Gemini reached it. The tool-use prompt tokens do: Gemini only bills them once
        // it has connected to the server, pulled the tool schemas and run a tool round-trip.
        _output.WriteLine(response.Text);

        Assert.NotEmpty(response.Text);

        var toolUseTokens = response.Usage?.AdditionalCounts?
            .GetValueOrDefault(GeminiAdditionalCounts.ToolUsePromptTokenCount);

        Assert.NotNull(toolUseTokens);
        Assert.True(toolUseTokens > 0, $"Expected the MCP server to be invoked, got {toolUseTokens} tool-use tokens.");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public Task GetResponseAsync_WithHostedCodeInterpreterTool_ShouldAcceptTheResponseAsHistory(
        bool persistHistoryAsJson)
    {
        return AssertCodeExecutionIsAcceptedAsHistoryAsync(
            persistHistoryAsJson,
            (client, messages, options, cancellationToken) =>
                client.GetResponseAsync(messages, options, cancellationToken));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public Task GetStreamingResponseAsync_WithHostedCodeInterpreterTool_ShouldAcceptTheResponseAsHistory(
        bool persistHistoryAsJson)
    {
        // A streamed turn delivers the executableCode and codeExecutionResult parts in separate chunks, so
        // aggregating the stream has to leave both echoable.
        return AssertCodeExecutionIsAcceptedAsHistoryAsync(
            persistHistoryAsJson,
            (client, messages, options, cancellationToken) => client
                .GetStreamingResponseAsync(messages, options, cancellationToken)
                .ToChatResponseAsync(cancellationToken));
    }

    private async Task AssertCodeExecutionIsAcceptedAsHistoryAsync(
        bool persistHistoryAsJson,
        Func<IChatClient, IList<ChatMessage>, ChatOptions, CancellationToken, Task<ChatResponse>> getResponseAsync)
    {
        // Arrange — Gemini needs the executableCode and codeExecutionResult parts echoed back on the next
        // turn. With the parts still on RawRepresentation they are echoed verbatim; persisted as JSON they
        // are rebuilt from Inputs, Outputs and the additional properties.
        var cancellationToken = TestContext.Current.CancellationToken;

        var clientOptions = new GeminiClientOptions { ApiKey = _apiKey, ModelId = Model };
        using var requests = new RequestRecordingHandler();
        using var httpClient = new HttpClient(requests) { BaseAddress = clientOptions.Endpoint };
        httpClient.DefaultRequestHeaders.Add("x-goog-api-key", clientOptions.ApiKey);

        IChatClient client = new GeminiChatClient(new GeminiClient(httpClient, clientOptions.ModelId));

        var options = new ChatOptions { Tools = [new HostedCodeInterpreterTool()] };

        var messages = new List<ChatMessage>
        {
            new(ChatRole.User,
                "Run Python code to compute the sum of the integers from 1 to 1000, then reply with only the number."),
        };

        var first = await getResponseAsync(client, messages, options, cancellationToken);
        _output.WriteLine(first.Text);

        Assert.Contains(first.Messages.SelectMany(m => m.Contents), c => c is CodeInterpreterToolCallContent);

        messages.AddMessages(first);

        if (persistHistoryAsJson)
        {
            var json = JsonSerializer.Serialize(messages, GeminiJsonUtilities.DefaultOptions);
            messages = JsonSerializer.Deserialize<List<ChatMessage>>(json, GeminiJsonUtilities.DefaultOptions)!;

            Assert.All(messages.SelectMany(m => m.Contents), c => Assert.Null(c.RawRepresentation));
        }

        messages.Add(new ChatMessage(ChatRole.User,
            "Divide the number you just computed by 2 and reply with only the result, no code and no commas."));

        // Act
        var second = await getResponseAsync(client, messages, options, cancellationToken);
        _output.WriteLine(second.Text);

        // Assert — the first reply already states the number, so a right answer alone would not show the
        // code parts were sent. The second request has to carry them, and Gemini has to accept it.
        var echoed = JsonSerializer.Deserialize<GenerateContentRequest>(requests.Bodies[^1])!;
        var echoedParts = echoed.Contents.SelectMany(c => c.Parts ?? []).ToList();

        Assert.Contains(echoedParts, p => p.ExecutableCode is not null);
        Assert.Contains(echoedParts, p => p.CodeExecutionResult is not null);
        Assert.Contains("250250", second.Text.Replace(",", "").Replace(" ", ""));
    }

    private sealed class RequestRecordingHandler : DelegatingHandler
    {
        public RequestRecordingHandler() : base(new HttpClientHandler())
        {
        }

        public List<string> Bodies { get; } = [];

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Content is not null)
            {
                Bodies.Add(await request.Content.ReadAsStringAsync(cancellationToken));
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }

    private static async Task<string> UploadDocumentAsync(
        GeminiClient client,
        string text,
        CancellationToken cancellationToken)
    {
        var bytes = Encoding.UTF8.GetBytes(text);

        var file = await client.V1Beta.Files.UploadFileAsync(
            new MemoryStream(bytes),
            bytes.Length,
            new UploadFileOptions { DisplayName = "aldbourne-reading-room", MimeType = "text/plain" },
            cancellationToken);

        Assert.NotNull(file.Name);

        return file.Name;
    }

    private static async Task ImportDocumentAsync(
        GeminiClient client,
        string storeId,
        string fileName,
        CancellationToken cancellationToken)
    {
        var operation = await client.V1Beta.FileSearchStores.ImportFileAsync(
            storeId,
            new ImportFileRequest { FileName = fileName },
            cancellationToken);

        Assert.NotNull(operation.Name);
        var operationId = operation.Name[(operation.Name.LastIndexOf('/') + 1)..];

        var deadline = DateTimeOffset.UtcNow.AddMinutes(2);

        while (operation.Done is not true)
        {
            Assert.True(DateTimeOffset.UtcNow < deadline, "Timed out importing the file into the store.");

            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

            var polled = await client.V1Beta.FileSearchStores.GetOperationByFileSearchStoreAndOperationAsync(
                storeId,
                operationId,
                cancellationToken);

            operation = operation with { Done = polled.Done, Error = polled.Error };
        }

        Assert.Null(operation.Error);
    }
}
