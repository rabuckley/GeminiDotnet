# GeminiDotnet

GeminiDotnet is a lightweight yet fully-featured library for interacting with Google's Gemini API in modern .NET. GeminiDotnet is performant and Native AOT compatible, using System.Text.Json source-generation for JSON serialization, and has minimal dependencies.

This respository contains two packages which users can choose from. The recommended entry-point is [`GeminiDotnet.Extensions.AI`](./src/GeminiDotnet.Extensions.AI) which provides implementations of the [`Microsoft.Extensions.AI.Abstractions`](https://www.nuget.org/packages/Microsoft.Extensions.AI.Abstractions/) APIs. These provide common abstractions over generative AI models, allowing users to swap out their model provider without rewriting their code. Alternatively, if you'd like a direct, lightweight mapping to the Google Gemini API, you can use [`GeminiDotnet`](./src/GeminiDotnet) directly.

- [`GeminiDotnet`](./src/GeminiDotnet) for direct interaction with Gemini API
- [`GeminiDotnet.Extensions.AI`](./src/GeminiDotnet.Extensions.AI) for use with [`Microsoft.Extensions.AI.Abstractions`](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/) (recommended).

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

### Streaming Text Generation

To get incremental updates while the model continues to output its response, you can use the streaming overloads.

```cs
var options = new GeminiClientOptions { ApiKey = _apiKey, ModelId = GeminiModels.Gemini2Flash };

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
    ApiKey = _apiKey, ModelId = GeminiModels.Gemini2Flash, ApiVersion = GeminiApiVersions.V1Beta
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
    ApiKey = _apiKey, ModelId = GeminiModels.Gemini2Flash, ApiVersion = GeminiApiVersions.V1Beta
};

IChatClient geminiClient = new GeminiChatClient(options);

var chatOptions = new ChatOptions { Tools = [new CodeInterpreterTool()] };

var response = await geminiClient.GetResponseAsync(
    [new(ChatRole.User, "What is the sum of the first 42 fibonacci numbers? Generate and run code to do the calculation.")],
    chatOptions,
    cancellationToken);
```

