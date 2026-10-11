using GeminiDotnet.Testing;
using Microsoft.Extensions.AI;
using System.Diagnostics.CodeAnalysis;
using System.Net;
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
    private const string ErrorResponseJson =
        """
        { "error": { "code": 400, "message": "Please use a valid role: user, model.", "status": "INVALID_ARGUMENT" } }
        """;
}
