# GeminiDotnet

GeminiDotnet is a lightweight but fully-featured library for interacting with Google's Gemini API in modern dotnet, with only System.\* dependencies.

There are two packages which can be used. The recommended entry-point is [`GeminiDotnet.Extensions.AI`](./src/GeminiDotnet.Extensions.AI) which provides implementations of the [`Microsoft.Extensions.AI.Abstractions`](https://www.nuget.org/packages/Microsoft.Extensions.AI.Abstractions/) interfaces. These provide common abstractions over generative AI models, allowing users to swap out their model provider without rewriting their code. Alternatively, if you'd like a lightweight and direct mapping straight to the Google Gemini API, you can use [`GeminiDotnet`](./src/GeminiDotnet) directly.

See the separate readmes for

- [`GeminiDotnet`](./src/GeminiDotnet) for direct interaction with Gemini API
- [`GeminiDotnet.Extensions.AI`](./src/GeminiDotnet.Extensions.AI) for use with [`Microsoft.Extensions.AI.Abstractions`](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/) (recommended).

### Versions

| Package | Latest |
| --- | --- |
| GeminiDotnet | [![NuGet Version](https://img.shields.io/nuget/v/GeminiDotnet)](https://www.nuget.org/packages/GeminiDotnet) |
| GeminiDotnet.Extensions.AI | [![NuGet Version](https://img.shields.io/nuget/v/GeminiDotnet.Extensions.AI)](https://www.nuget.org/packages/GeminiDotnet.Extensions.AI) | 

