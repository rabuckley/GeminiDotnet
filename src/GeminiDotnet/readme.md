# GeminiDotnet

GeminiDotnet maps directly to Google's Gemini API and supports Native AOT through System.Text.Json source generation. For a common API across model providers, use [`GeminiDotnet.Extensions.AI`](https://www.nuget.org/packages/GeminiDotnet.Extensions.AI).

## Get started

```sh
dotnet add package GeminiDotnet
```

Create a `GeminiClient` and call the V1 models client to stream a response.

```cs
using GeminiDotnet;
using GeminiDotnet.V1;

var options = new GeminiClientOptions
{
    ApiKey = "<your-api-key>"
};

var client = new GeminiClient(options);

var request = new GenerateContentRequest
{
    Contents =
    [
        new Content
        {
            Role = ChatRoles.User,
            Parts = [new Part { Text = "Who was the first person to walk on the moon?" }]
        }
    ]
};

await foreach (var result in client.V1.Models.StreamGenerateContentAsync("models/gemini-3.7-flash", request, cancellationToken))
{
    // Use the result as it is returned.
}
```

Use the client for the resource, such as `client.V1.Models` or `client.V1.Files`. Pass a full resource name such as `models/gemini-3.7-flash` or `files/abc123`. A malformed name throws `ArgumentException`.

`client.V1Beta` can change in any release. To use it, suppress the `GEMINI001` diagnostic. See the [V1Beta stability policy](https://github.com/rabuckley/GeminiDotnet#v1beta-stability).
