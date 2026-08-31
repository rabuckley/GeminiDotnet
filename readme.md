# GeminiDotnet

GeminiDotnet is a lightweight yet fully-featured library for interacting with Google's Gemini API in modern .NET. GeminiDotnet is performant and Native AOT compatible, using System.Text.Json source-generation for JSON serialization, and has minimal dependencies.

This respository contains two packages which users can choose from. The recommended entry-point is [`GeminiDotnet.Extensions.AI`](./src/GeminiDotnet.Extensions.AI) which provides implementations of the [`Microsoft.Extensions.AI.Abstractions`](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai) APIs. These provide common abstractions over generative AI models, allowing users to swap out their model provider without rewriting their code. Alternatively, if you'd like a direct, lightweight mapping to the Google Gemini API, you can use [`GeminiDotnet`](./src/GeminiDotnet) directly.

- [`GeminiDotnet`](./src/GeminiDotnet) for direct interaction with Gemini API
- [`GeminiDotnet.Extensions.AI`](./src/GeminiDotnet.Extensions.AI) for use with [`Microsoft.Extensions.AI`](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai) (recommended).

> [!NOTE]
> Since writing this library, Google have released [first-party support for C#](https://github.com/googleapis/dotnet-genai), which you may prefer. As of [Google.GenAI](https://www.nuget.org/packages/Google.GenAI) v0.11.0, this now includes Microsoft.Extensions.AI support too.

### Versions

| Package | Latest | Downloads |
| --- | --- | --- |
| GeminiDotnet | [![NuGet Version](https://img.shields.io/nuget/v/GeminiDotnet)](https://www.nuget.org/packages/GeminiDotnet) | [![NuGet Downloads](https://img.shields.io/nuget/dt/GeminiDotnet)](https://www.nuget.org/packages/GeminiDotnet)|
| GeminiDotnet.Extensions.AI | [![NuGet Version](https://img.shields.io/nuget/v/GeminiDotnet.Extensions.AI)](https://www.nuget.org/packages/GeminiDotnet.Extensions.AI) | [![NuGet Downloads](https://img.shields.io/nuget/dt/GeminiDotnet.Extensions.AI)](https://www.nuget.org/packages/GeminiDotnet.Extensions.AI)

## Examples

The following examples use the [`GeminiDotnet.Extensions.AI`](https://www.nuget.org/packages/GeminiDotnet.Extensions.AI) package.

- [Streaming Text Generation](#streaming-text-generation)
- [Function Execution](#function-execution)
- [Code Execution](#code-execution)
- [Remote MCP Servers](#remote-mcp-servers)
- [Requiring a Tool Call](#requiring-a-tool-call)

### Streaming Text Generation

To get incremental updates while the model continues to output its response, you can use the streaming overloads.

```cs
var options = new GeminiClientOptions { ApiKey = _apiKey, ModelId = "gemini-2.5-flash" };

IChatClient client = new GeminiChatClient(options);

await foreach (var update in client.GetStreamingResponseAsync("What is AI?"))
{
    Console.Write(update);
}
```

### Function Execution

Using [`Microsoft.Extensions.AI.FunctionInvokingChatClient`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.ai.functioninvokingchatclient) to handle automatic function invocation, it is simple to wire up function calling to arbitrary .NET functions.

```cs
var geminiClient = new GeminiChatClient(new GeminiClientOptions
{
    ApiKey = _apiKey, ModelId = "gemini-2.5-flash"
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
    new(ChatRole.User, "Should I wear a rain coat in London tomorrow (1st Oct, 2000)? Get the current weather if needed.")
];

var options = new ChatOptions
{
    Tools = [AIFunctionFactory.Create(GetCurrentWeather, nameof(GetCurrentWeather))]
};

var response = await client.GetResponseAsync(messages, options, cancellationToken);
```

### Code Execution

The Gemini API provides a code execution feature that enables the model to generate and run Python code and learn iteratively from the results until it arrives at a final output. You can enable and use this as follows.

```cs
var options = new GeminiClientOptions
{
    ApiKey = _apiKey, ModelId = "gemini-2.5-flash"
};

IChatClient geminiClient = new GeminiChatClient(options);

var chatOptions = new ChatOptions { Tools = [new HostedCodeInterpreterTool()] };

var response = await geminiClient.GetResponseAsync(
    [new(ChatRole.User, "What is the sum of the first 42 fibonacci numbers? Generate and run code to do the calculation.")],
    chatOptions,
    cancellationToken);
```

### Remote MCP Servers

Gemini can connect to a remote MCP server itself, discover its tools and call them server-side. Map a
`HostedMcpServerTool` onto that with the server's name and its streamable HTTP endpoint.

```cs
var options = new GeminiClientOptions
{
    ApiKey = _apiKey, ModelId = "gemini-2.5-flash"
};

IChatClient geminiClient = new GeminiChatClient(options);

var chatOptions = new ChatOptions
{
    Tools =
    [
        new HostedMcpServerTool("weather", "https://example.com/mcp")
        {
            ApprovalMode = HostedMcpServerToolApprovalMode.NeverRequire,
        },
    ]
};

var response = await geminiClient.GetResponseAsync(
    [new(ChatRole.User, "Will it rain in London tomorrow?")],
    chatOptions,
    cancellationToken);
```

Gemini rejects a request whose MCP servers break its own naming and addressing rules:

- `ServerName` must be lowercase snake_case (`weather`, `rain_forecast`), and must be unique across the
  servers in one request.
- `ServerAddress` must be an absolute URL.

The rest of `HostedMcpServerTool` is what Gemini cannot honour. Rather than dropping a restriction the
caller asked for, the mapping throws a `GeminiMappingException`:

- `AllowedTools` must be `null`. Gemini accepts an allow-list and then ignores it, so the model would still
  be offered every tool the server exposes. Restrict the tools on the server instead.
- `ApprovalMode` must be set to `HostedMcpServerToolApprovalMode.NeverRequire`. Gemini runs the tools
  server-side with no approval hook, so that is the only mode it can honour. The default `null` is rejected
  too: M.E.AI documents it as a value some providers treat as `AlwaysRequire`, and the OpenAI client does
  exactly that, so reading it as `NeverRequire` here would quietly turn "unspecified" into consent.
- An MCP server cannot be combined with `HostedWebSearchTool`, `HostedCodeInterpreterTool` or
  `HostedFileSearchTool` in the same request, because Gemini rejects that combination. `AIFunction` tools
  can be used alongside it.

`ServerDescription` has no Gemini counterpart and is ignored.

### Requiring a Tool Call

`ChatOptions.ToolMode = ChatToolMode.RequireAny` (or `RequireSpecific`) makes Gemini require a *function*
call, so the request must also declare at least one `AIFunction`. Asked to require one when only hosted
tools are present, the model loops until it hits the tool-call cap and returns an empty response with
`finishReason: TOO_MANY_TOOL_CALLS`, having billed every round-trip it made along the way. An MCP server is
no exception, because Gemini runs its tools server-side and no client-visible call ever satisfies the mode.
This library therefore throws a `GeminiMappingException` instead of sending such a request.
