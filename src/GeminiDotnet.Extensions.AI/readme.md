# GeminiDotnet.Extensions.AI

This package provides an implementation of [`Microsoft.Extensions.AI.IChatClient`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.ai.ichatclient) for interacting with [Google Gemini models](https://deepmind.google/technologies/gemini/) using the modern and lightweight [`GeminiDotnet`](https://www.nuget.org/packages/GeminiDotnet/) package.

Note: This package is marked as "preview" because `Microsoft.Extensions.AI.Abstractions` is in preview. It will become stable once `Microsoft.Extensions.AI` is.

For more information on why you may want to use the `IChatClient` abstraction, you can read the announcement [blog post](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/).

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

