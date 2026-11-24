# GeminiDotnet.Extensions.AI

This package implements [`Microsoft.Extensions.AI`](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai) interfaces using `GeminiDotnet.V1`.

If you set `RawRepresentationFactory`, return a `GeminiDotnet.V1.GenerateContentRequest` for chat, a `GeminiDotnet.V1.Models.BatchEmbedContentsRequest` for embeddings, or `null` to use the mapped request. The adapter throws `GeminiMappingException` if the factory returns a V1Beta request.

## Get started

```sh
dotnet add package GeminiDotnet.Extensions.AI
```

Create a `GeminiChatClient` to use Gemini through `IChatClient`.

```cs
var options = new GeminiClientOptions 
{
    ApiKey = "<your-api-key>"
};

IChatClient chatClient = new GeminiChatClient(options);

var response = await chatClient.GetResponseAsync("What is AI?");

Console.WriteLine(response.Message);
```

Use `GetStreamingResponseAsync` to read updates as they arrive.

```cs
var options = new GeminiClientOptions { ApiKey = "<your-api-key>" };

IChatClient chatClient = new GeminiChatClient(options);

await foreach (var update in chatClient.GetStreamingResponseAsync("What is AI?"))
{
    Console.Write(update);
}
```

