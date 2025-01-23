# GeminiDotnet.Extensions.AI

This package provides an implementation of [`Microsoft.Extensions.AI.IChatClient`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.ai.ichatclient) for interacting with [Google Gemini models](https://deepmind.google/technologies/gemini/).

For more information on why you may want to use the `IChatClient` abstraction, you can read the announcement [blog post](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/).

## Getting Started

First, install the NuGet package into your project.

```sh
dotnet add package GeminiDotnet.Extensions.AI
```

Then, you can create and use the `IChatClient` abstraction to interact with Gemini as follows.

```cs
using GeminiDotnet;
using GeminiDotnet.Extensions.AI;

var options = new GeminiClientOptions 
{
    ApiKey = "<your-api-key>"
};

var client = new GeminiClient(options);
IChatClient chatClient = new GeminiChatClient(client);

var response = await chatClient.CompleteAsync("What is AI?"); 

Console.WriteLine(response.Message);
```
