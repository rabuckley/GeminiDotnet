using GeminiDotnet.Testing;
using Microsoft.Extensions.AI;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Text.Json;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
using ChatRole = Microsoft.Extensions.AI.ChatRole;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Drives <see cref="GeminiChatClient"/> offline, through the HTTP boundary: what goes on the wire for a
/// given conversation, and what the caller sees for a given API response.
/// </summary>
public sealed class GeminiChatClientWireTests
{
    private const string Model = "gemini-test";

    [Fact]
    public async Task GetResponseAsync_ShouldPostTheMappedConversationToGenerateContent()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(TextResponseJson);
        var client = CreateClient(handler);

        // Act
        await client.GetResponseAsync(WeatherConversation(), WeatherOptions(), TestContext.Current.CancellationToken);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal($"https://generativelanguage.googleapis.com/v1beta/models/{Model}:generateContent", request.RequestUri?.ToString());
        Assert.Equal("application/json", request.ContentType);
        Assert.Equal(JsonText.Normalize(WeatherRequestJson), JsonText.Normalize(request.ReadBodyAsString()));
    }

    [Fact]
    public async Task GetStreamingResponseAsync_ShouldPostTheSameBodyToStreamGenerateContentAsServerSentEvents()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithServerSentEvents(TextResponseJson);
        var client = CreateClient(handler);

        // Act
        await foreach (var _ in client.GetStreamingResponseAsync(
            WeatherConversation(), WeatherOptions(), TestContext.Current.CancellationToken))
        {
        }

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(
            $"https://generativelanguage.googleapis.com/v1beta/models/{Model}:streamGenerateContent?alt=sse",
            request.RequestUri?.ToString());
        Assert.Equal(JsonText.Normalize(WeatherRequestJson), JsonText.Normalize(request.ReadBodyAsString()));
    }

    [Fact]
    public async Task GetResponseAsync_WithNoModelInOptions_ShouldSendTheClientDefaultModel()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(TextResponseJson);
        var client = new GeminiChatClient(new GeminiClient(handler.CreateHttpClient(), modelId: "gemini-default"));

        // Act
        await client.GetResponseAsync("Hello", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.EndsWith("/v1beta/models/gemini-default:generateContent", request.RequestUri?.AbsolutePath);
        Assert.Equal("gemini-default", request.ReadBodyAsJson()?["model"]?.GetValue<string>());
    }

    [Fact]
    public async Task GetResponseAsync_WithARawRepresentation_ShouldSendItsFieldsAlongsideTheMappedConversation()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(TextResponseJson);
        var client = CreateClient(handler);
        var options = new ChatOptions
        {
            ModelId = Model,
            RawRepresentationFactory = _ => new V1Beta.GenerateContentRequest
            {
                Contents = [],
                CachedContent = "cachedContents/weather",
            },
        };

        // Act
        await client.GetResponseAsync("What's the weather in Paris?", options, TestContext.Current.CancellationToken);

        // Assert
        var body = Assert.Single(handler.Requests).ReadBodyAsJson();
        Assert.Equal("cachedContents/weather", body?["cachedContent"]?.GetValue<string>());
        var content = Assert.Single(body?["contents"]?.AsArray() ?? []);
        Assert.Equal("What's the weather in Paris?", content?["parts"]?[0]?["text"]?.GetValue<string>());
    }

    [Fact]
    public async Task GetResponseAsync_WithARawRepresentation_ShouldSendFieldsThatHaveNoMappedSource()
    {
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(TextResponseJson);
        var client = CreateClient(handler);
        var options = new ChatOptions
        {
            ModelId = Model,
            RawRepresentationFactory = _ => new V1Beta.GenerateContentRequest
            {
                Contents = [],
                ContinuationToken = new byte[] { 1, 2, 3 },
                Labels = new Dictionary<string, string> { ["team"] = "weather" },
                ServiceTier = V1Beta.ServiceTier.Flex,
                Store = true,
            },
        };

        await client.GetResponseAsync("What's the weather in Paris?", options, TestContext.Current.CancellationToken);

        var body = Assert.Single(handler.Requests).ReadBodyAsJson();
        Assert.Equal("AQID", body?["continuationToken"]?.GetValue<string>());
        Assert.Equal("weather", body?["labels"]?["team"]?.GetValue<string>());
        Assert.Equal("flex", body?["serviceTier"]?.GetValue<string>());
        Assert.True(body?["store"]?.GetValue<bool>());
        var content = Assert.Single(body?["contents"]?.AsArray() ?? []);
        Assert.Equal("What's the weather in Paris?", content?["parts"]?[0]?["text"]?.GetValue<string>());
    }

    [Fact]
    public async Task GetResponseAsync_WithNoModel_ShouldThrowArgumentExceptionWithoutSending()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler();
        var client = CreateClient(handler);

        // Act
        Task Act() => client.GetResponseAsync([], new ChatOptions(), TestContext.Current.CancellationToken);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(Act);
        Assert.Contains(nameof(ChatOptions.ModelId), ex.Message);
        Assert.Contains(nameof(GeminiClientOptions), ex.Message);
        Assert.Contains(nameof(ChatOptions), ex.Message);
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task GetStreamingResponseAsync_WithNoModel_ShouldThrowArgumentExceptionWithoutSending()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler();
        var client = CreateClient(handler);

        // Act
        async Task Act()
        {
            await foreach (var _ in client.GetStreamingResponseAsync(
                [], new ChatOptions(), TestContext.Current.CancellationToken))
            {
            }
        }

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(Act);
        Assert.Contains(nameof(ChatOptions.ModelId), ex.Message);
        Assert.Contains(nameof(GeminiClientOptions), ex.Message);
        Assert.Contains(nameof(ChatOptions), ex.Message);
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task GetResponseAsync_ShouldMapTheResponseForTheCaller()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(TextResponseJson);
        var client = CreateClient(handler);

        // Act
        var response = await client.GetResponseAsync(
            "What's the weather in Paris?",
            new ChatOptions { ModelId = Model },
            TestContext.Current.CancellationToken);

        // Assert
        var message = Assert.Single(response.Messages);
        Assert.Equal(ChatRole.Assistant, message.Role);
        Assert.Equal("It's 18°C and sunny in Paris.", message.Text);
        Assert.Equal("resp-1", response.ResponseId);
        Assert.Equal("gemini-test-001", response.ModelId);
        Assert.Equal(ChatFinishReason.Stop, response.FinishReason);

        Assert.NotNull(response.Usage);
        Assert.Equal(12, response.Usage.InputTokenCount);
        Assert.Equal(9, response.Usage.OutputTokenCount);
        Assert.Equal(21, response.Usage.TotalTokenCount);
    }

    [Fact]
    public async Task GetResponseAsync_WhenTheModelCallsAFunction_ShouldSurfaceTheCall()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(
            """
            {
              "candidates": [
                {
                  "content": {
                    "role": "model",
                    "parts": [
                      { "functionCall": { "id": "call-1", "name": "get_weather", "args": { "city": "Paris" } } }
                    ]
                  },
                  "finishReason": "STOP"
                }
              ],
              "modelVersion": "gemini-test-001",
              "responseId": "resp-2"
            }
            """);
        var client = CreateClient(handler);

        // Act
        var response = await client.GetResponseAsync(
            "What's the weather in Paris?",
            new ChatOptions { ModelId = Model },
            TestContext.Current.CancellationToken);

        // Assert
        var call = Assert.IsType<FunctionCallContent>(Assert.Single(Assert.Single(response.Messages).Contents));
        Assert.Equal("call-1", call.CallId);
        Assert.Equal("get_weather", call.Name);
        Assert.NotNull(call.Arguments);
        Assert.Equal("Paris", call.Arguments["city"]?.ToString());
    }

    [Fact]
    public async Task GetResponseAsync_WithContinuationFinishReason_ShouldReportLength()
    {
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(ContinuationResponseJson);
        var client = CreateClient(handler);

        var response = await client.GetResponseAsync(
            "What's the weather in Paris?",
            new ChatOptions { ModelId = Model },
            TestContext.Current.CancellationToken);

        Assert.Equal(ChatFinishReason.Length, response.FinishReason);
        Assert.Equal("It's 18°C", response.Text);
    }

    [Fact]
    public async Task GetStreamingResponseAsync_WithContinuationFinishReason_ShouldReportLength()
    {
        var handler = new ScriptedHttpMessageHandler().RespondWithServerSentEvents(ContinuationResponseJson);
        var client = CreateClient(handler);

        List<ChatResponseUpdate> updates = [];
        await foreach (var update in client.GetStreamingResponseAsync(
            "What's the weather in Paris?",
            new ChatOptions { ModelId = Model },
            TestContext.Current.CancellationToken))
        {
            updates.Add(update);
        }

        Assert.Equal(ChatFinishReason.Length, updates.ToChatResponse().FinishReason);
    }

    [Fact]
    public async Task GetResponseAsync_WithBlockedPrompt_ShouldReportContentFilter()
    {
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(BlockedPromptResponseJson);
        var client = CreateClient(handler);

        var response = await client.GetResponseAsync(
            "What's the weather in Paris?",
            new ChatOptions { ModelId = Model },
            TestContext.Current.CancellationToken);

        Assert.Equal(ChatFinishReason.ContentFilter, response.FinishReason);
        Assert.Empty(response.Messages);
    }

    [Fact]
    public async Task GetStreamingResponseAsync_WithBlockedPrompt_ShouldReportContentFilter()
    {
        var handler = new ScriptedHttpMessageHandler().RespondWithServerSentEvents(BlockedPromptResponseJson);
        var client = CreateClient(handler);

        List<ChatResponseUpdate> updates = [];
        await foreach (var update in client.GetStreamingResponseAsync(
            "What's the weather in Paris?",
            new ChatOptions { ModelId = Model },
            TestContext.Current.CancellationToken))
        {
            updates.Add(update);
        }

        Assert.Equal(ChatFinishReason.ContentFilter, updates.ToChatResponse().FinishReason);
    }

    [Fact]
    public async Task GetStreamingResponseAsync_ShouldYieldAnUpdatePerEventThatAssemblesIntoTheResponse()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithServerSentEvents(
            """{"candidates":[{"content":{"role":"model","parts":[{"text":"It's 18°C"}]}}],"modelVersion":"gemini-test-001","responseId":"resp-3"}""",
            """{"candidates":[{"content":{"role":"model","parts":[{"text":" and sunny"}]}}],"modelVersion":"gemini-test-001","responseId":"resp-3"}""",
            """{"candidates":[{"content":{"role":"model","parts":[{"text":" in Paris."}]},"finishReason":"STOP"}],"usageMetadata":{"promptTokenCount":12,"candidatesTokenCount":9,"totalTokenCount":21},"modelVersion":"gemini-test-001","responseId":"resp-3"}""");
        var client = CreateClient(handler);

        // Act
        List<ChatResponseUpdate> updates = [];
        await foreach (var update in client.GetStreamingResponseAsync(
            "What's the weather in Paris?",
            new ChatOptions { ModelId = Model },
            TestContext.Current.CancellationToken))
        {
            updates.Add(update);
        }

        // Assert
        Assert.Equal(["It's 18°C", " and sunny", " in Paris."], updates.Select(update => update.Text));
        Assert.All(updates, update => Assert.Equal("resp-3", update.ResponseId));
        Assert.Equal([null, null, ChatFinishReason.Stop], updates.Select(update => update.FinishReason));

        var response = updates.ToChatResponse();
        Assert.Equal("It's 18°C and sunny in Paris.", Assert.Single(response.Messages).Text);
        Assert.Equal("gemini-test-001", response.ModelId);
        Assert.Equal(ChatFinishReason.Stop, response.FinishReason);
        Assert.NotNull(response.Usage);
        Assert.Equal(21, response.Usage.TotalTokenCount);
    }

    [Fact]
    public async Task GetResponseAsync_WhenTheApiReturnsAnError_ShouldThrowWithTheApiMessage()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(ErrorResponseJson, HttpStatusCode.BadRequest);
        var client = CreateClient(handler);

        // Act
        Task Act() => client.GetResponseAsync(
            "Hello",
            new ChatOptions { ModelId = Model },
            TestContext.Current.CancellationToken);

        // Assert
        var ex = await Assert.ThrowsAsync<GeminiClientException>(Act);
        Assert.Contains("Please use a valid role: user, model.", ex.Message);
    }

    [Fact]
    public async Task GetStreamingResponseAsync_WhenTheApiReturnsAnError_ShouldThrowWithTheApiMessage()
    {
        // Arrange
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(ErrorResponseJson, HttpStatusCode.BadRequest);
        var client = CreateClient(handler);

        // Act
        async Task Act()
        {
            await foreach (var _ in client.GetStreamingResponseAsync(
                "Hello",
                new ChatOptions { ModelId = Model },
                TestContext.Current.CancellationToken))
            {
            }
        }

        // Assert
        var ex = await Assert.ThrowsAsync<GeminiClientException>(Act);
        Assert.Contains("Please use a valid role: user, model.", ex.Message);
    }

    [Fact]
    public async Task GetResponseAsync_WithDataContentResult_ShouldSendInlineDataAndAnEmptyResponse()
    {
        var functionResponse = await SendFunctionResultAsync(
            new FunctionResultContent("call-1", new DataContent(new byte[] { 1, 2, 3 }, "image/png") { Name = "a.png" }));

        Assert.Equal(
            JsonText.Normalize(
                """
                {
                  "id": "call-1",
                  "name": "get_image",
                  "response": {},
                  "parts": [ { "inlineData": { "data": "AQID", "displayName": "a.png", "mimeType": "image/png" } } ]
                }
                """),
            functionResponse);
    }

    [Fact]
    public async Task GetResponseAsync_WithUriContentResult_ShouldSendFileData()
    {
        var functionResponse = await SendFunctionResultAsync(
            new FunctionResultContent("call-1", new UriContent("https://example.com/a.png", "image/png")));

        Assert.Equal(
            JsonText.Normalize(
                """
                {
                  "id": "call-1",
                  "name": "get_image",
                  "response": {},
                  "parts": [ { "fileData": { "fileUri": "https://example.com/a.png", "mimeType": "image/png" } } ]
                }
                """),
            functionResponse);
    }

    [Fact]
    public async Task GetResponseAsync_WithHostedFileContentResult_ShouldSendFileDataWithTheDisplayName()
    {
        var functionResponse = await SendFunctionResultAsync(new FunctionResultContent(
            "call-1",
            new HostedFileContent("https://example.com/files/a") { MediaType = "application/pdf", Name = "a.pdf" }));

        Assert.Equal(
            JsonText.Normalize(
                """
                {
                  "id": "call-1",
                  "name": "get_image",
                  "response": {},
                  "parts": [
                    { "fileData": { "displayName": "a.pdf", "fileUri": "https://example.com/files/a", "mimeType": "application/pdf" } }
                  ]
                }
                """),
            functionResponse);
    }

    [Fact]
    public async Task GetResponseAsync_WithMixedContentsResult_ShouldJoinTextAndKeepMediaInOrder()
    {
        List<AIContent> result =
        [
            new TextContent("first"),
            new DataContent(new byte[] { 1, 2, 3 }, "image/png"),
            new DataContent("second"u8.ToArray(), "text/csv"),
            new UriContent("https://example.com/a.png", "image/png"),
        ];

        var functionResponse = await SendFunctionResultAsync(new FunctionResultContent("call-1", result));

        Assert.Equal(
            JsonText.Normalize(
                """
                {
                  "id": "call-1",
                  "name": "get_image",
                  "response": { "result": "first\nsecond" },
                  "parts": [
                    { "inlineData": { "data": "AQID", "mimeType": "image/png" } },
                    { "fileData": { "fileUri": "https://example.com/a.png", "mimeType": "image/png" } }
                  ]
                }
                """),
            functionResponse);
    }

    [Theory]
    [InlineData("text/plain")]
    [InlineData("text/plain; charset=utf-8")]
    [InlineData("text/csv")]
    public async Task GetResponseAsync_WithTextDataContentResult_ShouldDecodeItIntoTheResponse(string mediaType)
    {
        var functionResponse = await SendFunctionResultAsync(
            new FunctionResultContent("call-1", new DataContent(Encoding.UTF8.GetBytes("18°C"), mediaType)));

        Assert.Equal(
            JsonText.Normalize("""{ "id": "call-1", "name": "get_image", "response": { "result": "18°C" } }"""),
            functionResponse);
    }

    [Fact]
    public async Task GetResponseAsync_WithTextContentResult_ShouldSendTheSameShapeAsAString()
    {
        var fromContent = await SendFunctionResultAsync(new FunctionResultContent("call-1", new TextContent("sunny")));
        var fromString = await SendFunctionResultAsync(new FunctionResultContent("call-1", "sunny"));

        Assert.Equal(fromString, fromContent);
    }

    [Fact]
    public async Task GetResponseAsync_WithEmptyContentsResult_ShouldSendTheSameShapeAsNull()
    {
        var fromContents = await SendFunctionResultAsync(new FunctionResultContent("call-1", new List<AIContent>()));
        var fromNull = await SendFunctionResultAsync(new FunctionResultContent("call-1", null));

        Assert.Equal(fromNull, fromContents);
    }

    [Fact]
    public async Task GetResponseAsync_WithFailedMediaOnlyResult_ShouldKeepTheErrorKey()
    {
        var functionResponse = await SendFunctionResultAsync(
            new FunctionResultContent("call-1", new DataContent(new byte[] { 1, 2, 3 }, "image/png"))
            {
                Exception = new InvalidOperationException(),
            });

        Assert.Equal(
            JsonText.Normalize(
                """
                {
                  "id": "call-1",
                  "name": "get_image",
                  "response": { "error": null },
                  "parts": [ { "inlineData": { "data": "AQID", "mimeType": "image/png" } } ]
                }
                """),
            functionResponse);
    }

    [Fact]
    public async Task GetResponseAsync_WithFailedTextResult_ShouldSendTheTextAsTheError()
    {
        var functionResponse = await SendFunctionResultAsync(
            new FunctionResultContent("call-1", new TextContent("failed")) { Exception = new InvalidOperationException() });

        Assert.Equal(
            JsonText.Normalize("""{ "id": "call-1", "name": "get_image", "response": { "error": "failed" } }"""),
            functionResponse);
    }

    [Fact]
    public async Task GetResponseAsync_WithPocoResult_ShouldSerializeItIntoTheResponse()
    {
        var functionResponse = await SendFunctionResultAsync(
            new FunctionResultContent("call-1", new WireWeather("Paris", new DateOnly(2000, 10, 1))));

        Assert.Equal(
            JsonText.Normalize(
                """
                {
                  "id": "call-1",
                  "name": "get_image",
                  "response": { "result": { "city": "Paris", "date": "2000-10-01" } }
                }
                """),
            functionResponse);
    }

    [Fact]
    public async Task GetResponseAsync_WithPocoFunctionCallArgument_ShouldSerializeItIntoTheArgs()
    {
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(TextResponseJson);
        var client = CreateClient(handler);
        List<ChatMessage> messages =
        [
            new(ChatRole.User, "Hi"),
            new(ChatRole.Assistant,
            [
                new FunctionCallContent(
                    "call-1",
                    "get_weather",
                    new Dictionary<string, object?> { ["request"] = new WireWeather("Paris", new DateOnly(2000, 10, 1)) }),
            ]),
        ];

        await client.GetResponseAsync(messages, new ChatOptions { ModelId = Model }, TestContext.Current.CancellationToken);

        using var body = JsonDocument.Parse(Assert.Single(handler.Requests).ReadBodyAsString());
        var args = body.RootElement.GetProperty("contents")[1].GetProperty("parts")[0]
            .GetProperty("functionCall").GetProperty("args");
        Assert.Equal(
            JsonText.Normalize("""{ "request": { "city": "Paris", "date": "2000-10-01" } }"""),
            JsonText.Normalize(args.GetRawText()));
    }

    [Fact]
    public async Task GetResponseAsync_WhenAFunctionReturnsDataContent_ShouldSendInlineData()
    {
        static DataContent GetImage() => new(new byte[] { 1, 2, 3 }, "image/png");

        var function = AIFunctionFactory.Create(GetImage, "get_image");

        var functionResponse = await InvokeFunctionAsync(function);

        Assert.Equal(
            JsonText.Normalize(
                """
                {
                  "id": "call-1",
                  "name": "get_image",
                  "response": {},
                  "parts": [ { "inlineData": { "data": "AQID", "mimeType": "image/png" } } ]
                }
                """),
            functionResponse);
    }

    [Fact]
    public async Task GetResponseAsync_WhenAnAIFunctionReturnsContentsAsObject_ShouldSplitThem()
    {
        var function = new ContentsFunction(
        [
            new TextContent("caption"),
            new DataContent(new byte[] { 1, 2, 3 }, "image/png"),
        ]);

        var functionResponse = await InvokeFunctionAsync(function);

        Assert.Equal(
            JsonText.Normalize(
                """
                {
                  "id": "call-1",
                  "name": "get_image",
                  "response": { "result": "caption" },
                  "parts": [ { "inlineData": { "data": "AQID", "mimeType": "image/png" } } ]
                }
                """),
            functionResponse);
    }

    private static async Task<string> SendFunctionResultAsync(FunctionResultContent result)
    {
        var handler = new ScriptedHttpMessageHandler().RespondWithJson(TextResponseJson);
        var client = CreateClient(handler);
        List<ChatMessage> messages =
        [
            new(ChatRole.User, "Get the image."),
            new(ChatRole.Assistant, [new FunctionCallContent("call-1", "get_image")]),
            new(ChatRole.Tool, [result]),
        ];

        await client.GetResponseAsync(messages, new ChatOptions { ModelId = Model }, TestContext.Current.CancellationToken);

        return ReadLastFunctionResponse(Assert.Single(handler.Requests));
    }

    private static async Task<string> InvokeFunctionAsync(AIFunction function)
    {
        var handler = new ScriptedHttpMessageHandler()
            .RespondWithJson(GetImageCallResponseJson)
            .RespondWithJson(TextResponseJson);
        var client = new ChatClientBuilder(CreateClient(handler)).UseFunctionInvocation().Build();

        await client.GetResponseAsync(
            "Get the image.",
            new ChatOptions { ModelId = Model, Tools = [function] },
            TestContext.Current.CancellationToken);

        return ReadLastFunctionResponse(handler.Requests[^1]);
    }

    private static string ReadLastFunctionResponse(RecordedRequest request)
    {
        using var body = JsonDocument.Parse(request.ReadBodyAsString());
        var lastContent = body.RootElement.GetProperty("contents").EnumerateArray().Last();
        return JsonText.Normalize(lastContent.GetProperty("parts")[0].GetProperty("functionResponse").GetRawText());
    }

    private sealed record WireWeather(string City, DateOnly Date);

    private sealed class ContentsFunction : AIFunction
    {
        private readonly AIContent[] _contents;

        public ContentsFunction(AIContent[] contents)
        {
            _contents = contents;
        }

        public override string Name => "get_image";

        protected override ValueTask<object?> InvokeCoreAsync(
            AIFunctionArguments arguments,
            CancellationToken cancellationToken)
        {
            return new ValueTask<object?>(_contents);
        }
    }

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string GetImageCallResponseJson =
        """
        {
          "candidates": [
            {
              "content": {
                "role": "model",
                "parts": [ { "functionCall": { "id": "call-1", "name": "get_image", "args": {} } } ]
              },
              "finishReason": "STOP"
            }
          ],
          "modelVersion": "gemini-test-001",
          "responseId": "resp-6"
        }
        """;

    private static GeminiChatClient CreateClient(ScriptedHttpMessageHandler handler)
    {
        return new GeminiChatClient(new GeminiClient(handler.CreateHttpClient()));
    }

    private static List<ChatMessage> WeatherConversation()
    {
        return
        [
            new(ChatRole.System, "You are terse."),
            new(ChatRole.User, "What's the weather in Paris?"),
            new(ChatRole.Assistant,
            [
                new FunctionCallContent("call-1", "get_weather", new Dictionary<string, object?> { ["city"] = "Paris" }),
            ]),
            new(ChatRole.Tool, [new FunctionResultContent("call-1", "18°C and sunny")]),
        ];
    }

    private static ChatOptions WeatherOptions()
    {
        return new ChatOptions
        {
            ModelId = Model,
            Temperature = 0.5f,
            TopP = 0.9f,
            TopK = 40,
            MaxOutputTokens = 256,
            Seed = 7,
            StopSequences = ["END"],
            Tools = [AIFunctionFactory.Create((string city) => "", "get_weather", "Gets the weather for a city.")],
            ToolMode = ChatToolMode.Auto,
        };
    }

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string WeatherRequestJson =
        """
        {
          "model": "gemini-test",
          "systemInstruction": { "parts": [ { "text": "You are terse." } ] },
          "contents": [
            { "role": "user", "parts": [ { "text": "What's the weather in Paris?" } ] },
            {
              "role": "model",
              "parts": [ { "functionCall": { "id": "call-1", "name": "get_weather", "args": { "city": "Paris" } } } ]
            },
            {
              "role": "model",
              "parts": [
                { "functionResponse": { "id": "call-1", "name": "get_weather", "response": { "result": "18°C and sunny" } } }
              ]
            }
          ],
          "generationConfig": {
            "temperature": 0.5,
            "topP": 0.9,
            "topK": 40,
            "maxOutputTokens": 256,
            "seed": 7,
            "stopSequences": [ "END" ]
          },
          "tools": [
            {
              "functionDeclarations": [
                {
                  "name": "get_weather",
                  "description": "Gets the weather for a city.",
                  "parametersJsonSchema": {
                    "type": "object",
                    "properties": { "city": { "type": "string" } },
                    "required": [ "city" ]
                  },
                  "responseJsonSchema": { "type": "string" }
                }
              ]
            }
          ],
          "toolConfig": { "functionCallingConfig": { "mode": "AUTO" } }
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string TextResponseJson =
        """
        {
          "candidates": [
            {
              "content": { "role": "model", "parts": [ { "text": "It's 18°C and sunny in Paris." } ] },
              "finishReason": "STOP"
            }
          ],
          "usageMetadata": { "promptTokenCount": 12, "candidatesTokenCount": 9, "totalTokenCount": 21 },
          "modelVersion": "gemini-test-001",
          "responseId": "resp-1"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string ContinuationResponseJson =
        """
        {
          "candidates": [
            {
              "content": { "role": "model", "parts": [ { "text": "It's 18°C" } ] },
              "finishReason": "CONTINUATION",
              "continuationToken": "AQID"
            }
          ],
          "modelVersion": "gemini-test-001",
          "responseId": "resp-4"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string BlockedPromptResponseJson =
        """
        {
          "promptFeedback": { "blockReason": "OTHER" },
          "modelVersion": "gemini-test-001",
          "responseId": "resp-5"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string ErrorResponseJson =
        """
        { "error": { "code": 400, "message": "Please use a valid role: user, model.", "status": "INVALID_ARGUMENT" } }
        """;
}
