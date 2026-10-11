using GeminiDotnet.Testing;
using GeminiDotnet.Text.Json;
using GeminiDotnet.V1Beta;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace GeminiDotnet;

public sealed class GenerateContentRequestTests
{
    [Theory]
    [MemberData(nameof(Examples))]
    public void Deserialize_Examples(string example)
    {
        // Act
        var request = JsonSerializer.Deserialize(
            example,
            V1BetaJsonContext.Default.GenerateContentRequest);

        // Assert
        Assert.NotNull(request);
        Assert.Equal(3, request.Contents!.Count);
    }

    [Theory]
    [MemberData(nameof(Examples))]
    public void JsonRoundtrip_ShouldPreserveEveryField(string example)
    {
        // Arrange
        var typeInfo = V1BetaJsonContext.Default.GenerateContentRequest;

        // Act
        var request = JsonSerializer.Deserialize(example, typeInfo);
        var serialized = JsonSerializer.Serialize(request, typeInfo);

        // Assert
        // Comparing with the source, not a second pass, is what catches a field the model lacks.
        Assert.Equal(JsonText.Normalize(example), JsonText.Normalize(serialized));
    }

    public static IEnumerable<TheoryDataRow<string>> Examples()
    {
        yield return ChatExample1Json;
        yield return EveryFieldExampleJson;
    }

    [StringSyntax(StringSyntaxAttribute.Json)]
    const string ChatExample1Json =
        """
        {
          "model": "models/gemini-1.5-pro",
          "contents": [
            {
              "role": "user",
              "parts": [
                {
                  "text": "Hello"
                }
              ]
            },
            {
              "role": "model",
              "parts": [
                {
                  "text": "Great to meet you. What would you like to know?"
                }
              ]
            },
            {
              "role": "user",
              "parts": [
                {
                  "text": "I have two dogs in my house. How many paws are in my house?"
                }
              ]
            }
          ]
        }
        """;

    /// <summary>
    /// Sets every top-level field of the request, so that the round trip catches one the model drops.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    const string EveryFieldExampleJson =
        """
        {
          "model": "models/gemini-2.5-flash",
          "systemInstruction": {
            "parts": [ { "text": "You are terse." } ]
          },
          "contents": [
            {
              "role": "user",
              "parts": [ { "text": "What's the weather in Paris?" } ]
            },
            {
              "role": "model",
              "parts": [
                { "functionCall": { "id": "call-1", "name": "get_weather", "args": { "city": "Paris" } } }
              ]
            },
            {
              "role": "user",
              "parts": [
                { "functionResponse": { "id": "call-1", "name": "get_weather", "response": { "result": "18°C and sunny" } } }
              ]
            }
          ],
          "tools": [
            {
              "functionDeclarations": [
                {
                  "name": "get_weather",
                  "description": "Gets the weather for a city.",
                  "parametersJsonSchema": {
                    "type": "object",
                    "properties": { "city": { "type": "string" } },
                    "required": [ "city" ]
                  }
                }
              ]
            },
            { "googleSearch": {} }
          ],
          "toolConfig": {
            "functionCallingConfig": { "mode": "ANY", "allowedFunctionNames": [ "get_weather" ] },
            "includeServerSideToolInvocations": true
          },
          "safetySettings": [
            { "category": "HARM_CATEGORY_HARASSMENT", "threshold": "BLOCK_ONLY_HIGH" }
          ],
          "generationConfig": {
            "temperature": 0.5,
            "topP": 0.95,
            "topK": 40,
            "maxOutputTokens": 256,
            "stopSequences": [ "END" ],
            "thinkingConfig": { "includeThoughts": true, "thinkingBudget": 1024 }
          },
          "cachedContent": "cachedContents/weather",
          "labels": { "team": "weather" },
          "serviceTier": "flex",
          "store": false,
          "continuationToken": "AQID"
        }
        """;
}
