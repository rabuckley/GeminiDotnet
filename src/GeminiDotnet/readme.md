# GeminiDotnet

This package provides the tools to interact with the [Google Gemini models](https://deepmind.google/technologies/gemini/) in .NET.

This package is lightweight, with no dependencies, and uses direct mappings to the Gemini API.

For an API you can use with other models, and for a more user-friendly API, you should consider interacting with this library through the [`GeminiDotnet.Extensions.AI`](https://www.nuget.org/packages/GeminiDotnet.Extensions.AI) library, which exposes an [`Microsoft.Extensions.AI.IChatClient`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.ai.ichatclient) implementation. For more information about why this may be preferred, you can read the `Microsoft.Extensions.AI` announcement [blog post](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/).

## Getting Started

If you want to use the minimal API, you can install this package into your project as follows.

```sh
dotnet add package GeminiDotnet
```

Then, you can create and use the `GeminiClient` to interact with Gemini as follows.

```cs
using GeminiDotnet;

var options = new GeminiClientOptions
{
    ApiKey = "<your-api-key>"
};

var client = new GeminiClient(options);

var request = new GenerateContentRequest
{
    Contents = 
    [
        new ChatMessage 
        { 
            Role = ChatRole.User,
            Parts = [new Part { Text = "Who was the first person to walk on the moon?" }]
        }
    ]
};

await foreach (var result in client.GenerateContentStreamingAsync("gemini-2.0-flash", request, cancellationToken))
{
    // Use the result as it is returned.
}
```
