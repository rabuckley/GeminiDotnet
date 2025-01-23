using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using GeminiDotnet.ContentGeneration;

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
}