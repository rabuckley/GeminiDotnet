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

Use `GetStreamingResponseAsync` to read updates as Gemini generates them.

```cs
var options = new GeminiClientOptions { ApiKey = _apiKey, ModelId = "gemini-3.7-flash" };

IChatClient client = new GeminiChatClient(options);

await foreach (var update in client.GetStreamingResponseAsync("What is AI?"))
{
    Console.Write(update);
}
```

### Function Execution

Call `UseFunctionInvocation()` on the chat client builder to invoke declared .NET functions automatically. [FunctionInvokingChatClient documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.ai.functioninvokingchatclient)

```cs
var geminiClient = new GeminiChatClient(new GeminiClientOptions
{
    ApiKey = _apiKey, ModelId = "gemini-3.7-flash"
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

Add a `HostedCodeInterpreterTool` to let Gemini run Python and use the output in its response.

```cs
var options = new GeminiClientOptions
{
    ApiKey = _apiKey, ModelId = "gemini-3.7-flash"
};

IChatClient geminiClient = new GeminiChatClient(options);

var chatOptions = new ChatOptions { Tools = [new HostedCodeInterpreterTool()] };

var response = await geminiClient.GetResponseAsync(
    [new(ChatRole.User, "What is the sum of the first 42 fibonacci numbers? Generate and run code to do the calculation.")],
    chatOptions,
    cancellationToken);
```

### Remote MCP Servers

Pass the server name and streamable HTTP endpoint to `HostedMcpServerTool`. Gemini discovers and invokes its tools server-side.

```cs
var options = new GeminiClientOptions
{
    ApiKey = _apiKey, ModelId = "gemini-3.7-flash"
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

Gemini requires a unique lowercase snake_case `ServerName` per request and an absolute `ServerAddress` URL.

The adapter throws `GeminiMappingException` unless these requirements hold.

- Leave `AllowedTools` as `null`. Restrict tools on the server, since Gemini offers the model every tool it exposes.
- Set `ApprovalMode` explicitly to `HostedMcpServerToolApprovalMode.NeverRequire`. Gemini invokes tools without an approval callback.
- Use MCP servers with function declarations or on their own. Gemini rejects requests that combine MCP servers with `HostedWebSearchTool`, `HostedCodeInterpreterTool`, or `HostedFileSearchTool`.

The adapter ignores `ServerDescription`.

### Requiring a Tool Call

Include at least one function declaration, such as an `AIFunction`, when using `ChatToolMode.RequireAny` or `RequireSpecific`. Gemini requires a function call in these modes. Hosted tools, including remote MCP tools, cannot satisfy that requirement. The adapter throws `GeminiMappingException` before sending such a request, which would otherwise risk an empty `TOO_MANY_TOOL_CALLS` response after billable tool calls.
