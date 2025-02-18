# GeminiDotnet

GeminiDotnet is a lightweight yet fully-featured library for interacting with Google's Gemini API in modern .NET. GeminiDotnet is performant and Native AOT compatible, using System.Text.Json source-generation for JSON serialization, and has minimal dependencies.

This respository contains two packages which users can choose from. The recommended entry-point is [`GeminiDotnet.Extensions.AI`](./src/GeminiDotnet.Extensions.AI) which provides implementations of the [`Microsoft.Extensions.AI.Abstractions`](https://www.nuget.org/packages/Microsoft.Extensions.AI.Abstractions/) APIs. These provide common abstractions over generative AI models, allowing users to swap out their model provider without rewriting their code. Alternatively, if you'd like a direct, lightweight mapping to the Google Gemini API, you can use [`GeminiDotnet`](./src/GeminiDotnet) directly.

See the separate readmes for

- [`GeminiDotnet`](./src/GeminiDotnet) for direct interaction with Gemini API
- [`GeminiDotnet.Extensions.AI`](./src/GeminiDotnet.Extensions.AI) for use with [`Microsoft.Extensions.AI.Abstractions`](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/) (recommended).

### Versions

| Package | Latest | Downloads |
| --- | --- | --- |
| GeminiDotnet | [![NuGet Version](https://img.shields.io/nuget/v/GeminiDotnet)](https://www.nuget.org/packages/GeminiDotnet) | [![NuGet Downloads](https://img.shields.io/nuget/dt/GeminiDotnet)](https://www.nuget.org/packages/GeminiDotnet)|
| GeminiDotnet.Extensions.AI | [![NuGet Version](https://img.shields.io/nuget/v/GeminiDotnet.Extensions.AI)](https://www.nuget.org/packages/GeminiDotnet.Extensions.AI) | [![NuGet Downloads](https://img.shields.io/nuget/dt/GeminiDotnet.Extensions.AI)](https://www.nuget.org/packages/GeminiDotnet.Extensions.AI)

