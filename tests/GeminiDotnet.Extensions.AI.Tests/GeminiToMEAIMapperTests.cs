using GeminiDotnet.ContentGeneration;
using Microsoft.Extensions.AI;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace GeminiDotnet.Extensions.AI;

public sealed class GeminiToMEAIMapperTests
{
    [Fact]
    public void CreateMappedChatResponse_WithCodeExecution_ShouldMapToText()
    {
        // Arrange
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(PythonCodeExecutionExampleResponse)!;
        var actualContent = response.Candidates[0].Content;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Choices).Contents;

        var text1 = Assert.IsType<TextContent>(contents[0]);
        Assert.Equal(actualContent.Parts[0].Text, text1.Text);

        var code1 = Assert.IsType<TextContent>(contents[1]);
        Assert.Contains(actualContent.Parts[1].ExecutableCode!.Code, code1.Text);
        Assert.Contains(actualContent.Parts[1].ExecutableCode!.Language, code1.Text);

        var code2 = Assert.IsType<TextContent>(contents[2]);
        Assert.Contains(actualContent.Parts[2].CodeExecutionResult!.Output, code2.Text);
        Assert.Contains(actualContent.Parts[2].CodeExecutionResult!.Outcome.ToString(), code2.Text);


        var text2 = Assert.IsType<TextContent>(contents[3]);
        Assert.Equal(actualContent.Parts[3].Text, text2.Text);
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
