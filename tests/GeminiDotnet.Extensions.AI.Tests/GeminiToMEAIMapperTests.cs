using GeminiDotnet.Extensions.AI.Contents;
using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.Models;
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
        var contents = Assert.Single(result.Messages).Contents;

        var text1 = Assert.IsType<TextContent>(contents[0]);
        Assert.Equal(actualContent.Parts[0].Text, text1.Text);

        var code1 = Assert.IsType<ExecutableCodeContent>(contents[1]);
        Assert.Equal(actualContent.Parts[1].ExecutableCode!.Code, code1.Code);
        Assert.Equal(actualContent.Parts[1].ExecutableCode!.Language, code1.Language);

        var code2 = Assert.IsType<CodeExecutionContent>(contents[2]);
        Assert.Equal(actualContent.Parts[2].CodeExecutionResult!.Output, code2.Output);

        var text2 = Assert.IsType<TextContent>(contents[3]);
        Assert.Equal(actualContent.Parts[3].Text, text2.Text);
    }

    [Fact]
    public void CreateMappedChatResponse_WithEmptyCandidates_ShouldNotThrow()
    {
        // Arrange — empty candidates list should not cause IndexOutOfRangeException
        var response = new GenerateContentResponse
        {
            Candidates = [],
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test-empty-candidates"
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.Empty(result.Messages);
        Assert.Null(result.FinishReason);
    }

    [Fact]
    public void CreateMappedChatResponse_WithNullParts_ShouldReturnEmptyContents()
    {
        // Arrange
        var response = new GenerateContentResponse
        {
            Candidates = [new Candidate { Content = new Content { Role = "model", Parts = null! } }],
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test"
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var message = Assert.Single(result.Messages);
        Assert.NotNull(message.Contents);
        Assert.Empty(message.Contents);
    }

    [Fact]
    public void CreateMappedChatResponse_ShouldSetResponseId()
    {
        // Arrange
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(NonStreamingResponseWithUsage)!;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(response.ResponseId, result.ResponseId);
    }

    #region UsageMetadata Mapping Tests

    [Fact]
    public void CreateMappedChatResponseUpdate_WithUsageMetadata_AddsUsageContent()
    {
        // Arrange
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(StreamingResponseWithUsage)!;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, DateTimeOffset.UtcNow);

        // Assert
        var usageContent = result.Contents.OfType<UsageContent>().SingleOrDefault();
        Assert.NotNull(usageContent);
        Assert.Equal(100, usageContent.Details.InputTokenCount);
        Assert.Equal(50, usageContent.Details.OutputTokenCount);
        Assert.Equal(150, usageContent.Details.TotalTokenCount);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithoutUsageMetadata_NoUsageContent()
    {
        // Arrange
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(StreamingResponseWithoutUsage)!;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, DateTimeOffset.UtcNow);

        // Assert
        var usageContent = result.Contents.OfType<UsageContent>().SingleOrDefault();
        Assert.Null(usageContent);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithFullUsageMetadata_MapsAllFields()
    {
        // Arrange
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(StreamingResponseWithFullUsage)!;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, DateTimeOffset.UtcNow);

        // Assert
        var usageContent = result.Contents.OfType<UsageContent>().SingleOrDefault();
        Assert.NotNull(usageContent);

        var details = usageContent.Details;
        Assert.Equal(100, details.InputTokenCount);
        // OutputTokenCount = candidatesTokenCount (50) + thoughtsTokenCount (30) per M.E.AI convention
        Assert.Equal(80, details.OutputTokenCount);
        Assert.Equal(150, details.TotalTokenCount);
        Assert.Equal(25, details.CachedInputTokenCount);
        Assert.Equal(30, details.ReasoningTokenCount);
        Assert.NotNull(details.AdditionalCounts);
        Assert.Equal(10, details.AdditionalCounts[GeminiAdditionalCounts.ToolUsePromptTokenCount]);
    }

    [Fact]
    public void CreateMappedChatResponse_WithUsageMetadata_MapsToUsageProperty()
    {
        // Arrange
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(NonStreamingResponseWithUsage)!;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.NotNull(result.Usage);
        Assert.Equal(100, result.Usage.InputTokenCount);
        Assert.Equal(50, result.Usage.OutputTokenCount);
        Assert.Equal(150, result.Usage.TotalTokenCount);
    }

    [Fact]
    public void CreateMappedChatResponse_WithFullUsageMetadata_MapsAllFields()
    {
        // Arrange
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(NonStreamingResponseWithFullUsage)!;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.NotNull(result.Usage);
        Assert.Equal(100, result.Usage.InputTokenCount);
        // OutputTokenCount = candidatesTokenCount (50) + thoughtsTokenCount (30) per M.E.AI convention
        Assert.Equal(80, result.Usage.OutputTokenCount);
        Assert.Equal(150, result.Usage.TotalTokenCount);
        Assert.Equal(25, result.Usage.CachedInputTokenCount);
        Assert.Equal(30, result.Usage.ReasoningTokenCount);
        Assert.NotNull(result.Usage.AdditionalCounts);
        Assert.Equal(10, result.Usage.AdditionalCounts[GeminiAdditionalCounts.ToolUsePromptTokenCount]);
    }

    [Fact]
    public void CreateMappedUsageDetails_WithNullOutputCounts_ShouldReturnNullOutputTokenCount()
    {
        // Arrange — usage has promptTokenCount but no candidatesTokenCount or
        // thoughtsTokenCount. OutputTokenCount should be null, not 0.
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(
            """
            {
              "candidates": [
                {
                  "content": { "parts": [{ "text": "Hi" }], "role": "model" },
                  "finishReason": "STOP"
                }
              ],
              "usageMetadata": {
                "promptTokenCount": 10,
                "totalTokenCount": 10
              },
              "modelVersion": "gemini-2.0-flash",
              "responseId": "test-null-output"
            }
            """)!;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.NotNull(result.Usage);
        Assert.Equal(10, result.Usage.InputTokenCount);
        Assert.Null(result.Usage.OutputTokenCount);
    }

    #endregion

    #region BatchEmbedContents Mapping Tests

    [Fact]
    public void CreateMappedGeneratedEmbeddings_BatchResponse_ReturnsCorrectCount()
    {
        // Arrange
        var response = new BatchEmbedContentsResponse
        {
            Embeddings =
            [
                new ContentEmbedding { Values = new float[] { 0.1f, 0.2f, 0.3f } },
                new ContentEmbedding { Values = new float[] { 0.4f, 0.5f, 0.6f } },
                new ContentEmbedding { Values = new float[] { 0.7f, 0.8f, 0.9f } },
            ]
        };
        var createdAt = DateTimeOffset.UtcNow;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedGeneratedEmbeddings(response, "test-model", createdAt);

        // Assert
        Assert.Equal(3, result.Count);

        for (int i = 0; i < result.Count; i++)
        {
            Assert.Equal("test-model", result[i].ModelId);
            Assert.Equal(createdAt, result[i].CreatedAt);
            Assert.Equal(3, result[i].Vector.Length);
        }

        // Verify order is preserved
        Assert.Equal(0.1f, result[0].Vector.Span[0]);
        Assert.Equal(0.4f, result[1].Vector.Span[0]);
        Assert.Equal(0.7f, result[2].Vector.Span[0]);
    }

    [Fact]
    public void CreateMappedGeneratedEmbeddings_BatchResponse_WithEmptyValues_PreservesIndexCorrelation()
    {
        // Arrange — the middle entry has default (empty) Values, simulating a
        // missing embedding. The mapper must still produce 3 embeddings so that
        // result[i] maps to input[i].
        var response = new BatchEmbedContentsResponse
        {
            Embeddings =
            [
                new ContentEmbedding { Values = new float[] { 0.1f, 0.2f } },
                new ContentEmbedding(), // Values defaults to ReadOnlyMemory<float>.Empty
                new ContentEmbedding { Values = new float[] { 0.7f, 0.8f } },
            ]
        };
        var createdAt = DateTimeOffset.UtcNow;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedGeneratedEmbeddings(response, "test-model", createdAt);

        // Assert — all 3 entries present, middle one has zero-length vector
        Assert.Equal(3, result.Count);
        Assert.Equal(2, result[0].Vector.Length);
        Assert.Equal(0, result[1].Vector.Length);
        Assert.Equal(2, result[2].Vector.Length);
    }

    #endregion

    #region Test Data

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
          "modelVersion": "gemini-1.5-flash",
          "responseId": "bvq7aInSLPn9nsEP3MKX6A4"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string StreamingResponseWithUsage =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [{ "text": "Hello" }],
                "role": "model"
              },
              "finishReason": "STOP"
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 100,
            "candidatesTokenCount": 50,
            "totalTokenCount": 150
          },
          "modelVersion": "gemini-2.0-flash",
          "responseId": "test-response-1"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string StreamingResponseWithoutUsage =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [{ "text": "Hello" }],
                "role": "model"
              }
            }
          ],
          "modelVersion": "gemini-2.0-flash",
          "responseId": "test-response-2"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string StreamingResponseWithFullUsage =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [{ "text": "Hello" }],
                "role": "model"
              },
              "finishReason": "STOP"
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 100,
            "candidatesTokenCount": 50,
            "totalTokenCount": 150,
            "cachedContentTokenCount": 25,
            "thoughtsTokenCount": 30,
            "toolUsePromptTokenCount": 10
          },
          "modelVersion": "gemini-2.0-flash",
          "responseId": "test-response-3"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string NonStreamingResponseWithUsage =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [{ "text": "Hello, I'm an AI assistant." }],
                "role": "model"
              },
              "finishReason": "STOP"
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 100,
            "candidatesTokenCount": 50,
            "totalTokenCount": 150
          },
          "modelVersion": "gemini-2.0-flash",
          "responseId": "test-response-4"
        }
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string NonStreamingResponseWithFullUsage =
        """
        {
          "candidates": [
            {
              "content": {
                "parts": [{ "text": "Hello, I'm an AI assistant." }],
                "role": "model"
              },
              "finishReason": "STOP"
            }
          ],
          "usageMetadata": {
            "promptTokenCount": 100,
            "candidatesTokenCount": 50,
            "totalTokenCount": 150,
            "cachedContentTokenCount": 25,
            "thoughtsTokenCount": 30,
            "toolUsePromptTokenCount": 10
          },
          "modelVersion": "gemini-2.0-flash",
          "responseId": "test-response-5"
        }
        """;

    #endregion
}
