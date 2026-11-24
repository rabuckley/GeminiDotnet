# GeminiDotnet

GeminiDotnet is a .NET client for Google's Gemini API. It supports Native AOT through System.Text.Json source generation.

- Use [`GeminiDotnet.Extensions.AI`](./src/GeminiDotnet.Extensions.AI) with [`Microsoft.Extensions.AI`](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai) if you want to swap model providers through a common API.
- Use [`GeminiDotnet`](./src/GeminiDotnet) for direct access to the Gemini API.

Google also publishes a [C# SDK with Microsoft.Extensions.AI support](https://github.com/googleapis/dotnet-genai).

### Versions

| Package | Latest | Downloads |
| --- | --- | --- |
| GeminiDotnet | [![NuGet Version](https://img.shields.io/nuget/v/GeminiDotnet)](https://www.nuget.org/packages/GeminiDotnet) | [![NuGet Downloads](https://img.shields.io/nuget/dt/GeminiDotnet)](https://www.nuget.org/packages/GeminiDotnet)|
| GeminiDotnet.Extensions.AI | [![NuGet Version](https://img.shields.io/nuget/v/GeminiDotnet.Extensions.AI)](https://www.nuget.org/packages/GeminiDotnet.Extensions.AI) | [![NuGet Downloads](https://img.shields.io/nuget/dt/GeminiDotnet.Extensions.AI)](https://www.nuget.org/packages/GeminiDotnet.Extensions.AI)

### Versioning policy

Both packages follow [Semantic Versioning](https://semver.org/) with [Cargo’s 0.x convention](https://doc.rust-lang.org/cargo/reference/semver.html). In 0.x, minor releases may break the public API, and patches may add compatible features and fixes. See the [V1Beta exception](#v1beta-stability).

Removing an adapter feature is a breaking change even if Google withdraws it.

### V1Beta stability

`GeminiDotnet.V1Beta` can break in any release, including a patch release. To opt in to the experimental API across a project, add `GEMINI001` to the project's `NoWarn` property.

```xml
<NoWarn>$(NoWarn);GEMINI001</NoWarn>
```

`GeminiDotnet.Extensions.AI` uses `GeminiDotnet.V1`. Use `GeminiClient.V1Beta` directly for beta-only features such as context caching and audio transcription.

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
