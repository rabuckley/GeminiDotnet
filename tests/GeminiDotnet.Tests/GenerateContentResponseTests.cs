using GeminiDotnet.ContentGeneration;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace GeminiDotnet;

public sealed class GenerateContentResponseTests
{
    [Theory]
    [MemberData(nameof(ExampleResponses))]
    public void JsonRoundtrip(string json)
    {
        // Arrange
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(json);

        // Act
        var serialized = JsonSerializer.Serialize(response);
        var deserialized = JsonSerializer.Deserialize<GenerateContentResponse>(serialized);

        // Assert
        Assert.Equivalent(response, deserialized);
    }

    public static IEnumerable<TheoryDataRow<string>> ExampleResponses()
    {
        yield return PythonCodeExecutionExampleResponse;
        yield return CodeExExample2;
        yield return JsonSchemaOutputExample;
        yield return VideoExampleResponse;
    }

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string PythonCodeExecutionExampleResponse =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  {
                    "text": "To print \"Hello, World!\" using Python, I will use the `print()` function.  This is a standard function in Python used for displaying output to the console.\\Here's the Python code:\\"
                  },
                  {
                    "executableCode": {
                      "language": "PYTHON",
                      "code": "print(\"Hello, World!\")"
                    }
                  },
                  {
                    "codeExecutionResult": {
                      "outcome": "OUTCOME_OK",
                      "output": "Hello, World!"
                    }
                  },
                  {
                    "text": "The code successfully prints \"Hello, World!\" to the console.  No further analysis or information gathering is needed."
                  }
                ],
                "role": "model"
              },
              "finishReason": "STOP",
              "avgLogprobs": -0.030912578841786324
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 10,
            "candidatesTokenCount": 81,
            "totalTokenCount": 91
          },
          "modelVersion": "gemini-1.5-flash"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    public const string CodeExExample2 =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  {
                    "executableCode": {
                      "language": "PYTHON",
                      "code": "print(\"Hello, World!\")"
                    }
                  },
                  {
                    "codeExecutionResult": {
                      "outcome": "OUTCOME_OK",
                      "output": "Hello, World!"
                    }
                  },
                  {
                    "text": "Hello, World!"
                  }
                ],
                "role": "model"
              },
              "finishReason": "STOP"
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 10,
            "candidatesTokenCount": 17,
            "totalTokenCount": 27,
            "promptTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 10
              }
            ],
            "candidatesTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 17
              }
            ]
          },
          "modelVersion": "gemini-2.0-flash"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string JsonSchemaOutputExample =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  {
                    "text": "{ \"name\": \"Ryan\", \"age\": 42 }"
                  }
                ],
                "role": "model"
              },
              "finishReason": "MAX_TOKENS",
              "citationMetadata": {
                "citationSources": [
                  {
                    "startIndex": 257,
                    "endIndex": 23567
                  },
                  {
                    "startIndex": 23578,
                    "endIndex": 24592
                  }
                ]
              },
              "avgLogprobs": -0.022648460378868095
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 12,
            "candidatesTokenCount": 8190,
            "totalTokenCount": 8202,
            "promptTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 12
              }
            ],
            "candidatesTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 8190
              }
            ]
          },
          "modelVersion": "gemini-2.0-flash"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    public const string VideoExampleResponse =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  {
                    "text": ":\\n\\nIn a talk from Lenny's Podcast between Lenny Ratchitsky and Michael Tru"
                  }
                ],
                "role": "model"
              }
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 21,
            "totalTokenCount": 21,
            "promptTokensDetails": [
              {
                "modality": "TEXT",
                "tokenCount": 21
              },
              {
                "modality": "VIDEO"
              },
              {
                "modality": "AUDIO"
              }
            ]
          },
          "modelVersion": "gemini-2.0-flash"
        }
        """;
}
