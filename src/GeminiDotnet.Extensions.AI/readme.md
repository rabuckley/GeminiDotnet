# GeminiDotnet.Extensions.AI

This package provides implementations of the [`Microsoft.Extensions.AI.Abstractions`](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai) APIs backed by Google's Gemini API. These provide common abstractions over generative AI models, allowing users to swap out their model provider without rewriting their code. These implementations are backed by the modern and lightweight [`GeminiDotnet`](https://www.nuget.org/packages/GeminiDotnet/) package.

For more information on why you may want to use the [`Microsoft.Extensions.AI.Abstractions`](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai) APIs, you can read its announcement [blog post](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/).

## Getting Started

First, install the NuGet package into your project.

```sh
dotnet add package GeminiDotnet.Extensions.AI
```

Then, you can create and use the `IChatClient` abstraction to interact with Gemini as follows.

```cs
var options = new GeminiClientOptions 
{
    ApiKey = "<your-api-key>"
};

IChatClient chatClient = new GeminiChatClient(options);

var response = await chatClient.GetResponseAsync("What is AI?");

Console.WriteLine(response.Message);
```

Or, to get access to the results as they are streamed back from the model, you can use `GetStreamingResponseAsync`.

```cs
var options = new GeminiClientOptions { ApiKey = "<your-api-key>" };

IChatClient chatClient = new GeminiChatClient(options);

await foreach (var update in chatClient.GetStreamingResponseAsync("What is AI?"))
{
    Console.Write(update);
}
```

