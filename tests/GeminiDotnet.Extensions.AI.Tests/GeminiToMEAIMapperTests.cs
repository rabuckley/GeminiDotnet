using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.Models;
using Microsoft.Extensions.AI;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

#pragma warning disable MEAI001 // Experimental API (CodeInterpreter*, WebSearchTool*Content)

namespace GeminiDotnet.Extensions.AI;

public sealed class GeminiToMEAIMapperTests
{
    [Fact]
    public void CreateMappedChatResponse_WithCodeExecution_ShouldMapToCodeInterpreterTypes()
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

        var toolCall = Assert.IsType<CodeInterpreterToolCallContent>(contents[1]);
        Assert.NotNull(toolCall.CallId);
        var codeInput = Assert.Single(toolCall.Inputs!);
        var dataContent = Assert.IsType<DataContent>(codeInput);
        Assert.Equal("text/x-python", dataContent.MediaType);
        var code = System.Text.Encoding.UTF8.GetString(dataContent.Data.Span);
        Assert.Equal(actualContent.Parts[1].ExecutableCode!.Code, code);

        var toolResult = Assert.IsType<CodeInterpreterToolResultContent>(contents[2]);
        Assert.Equal(toolCall.CallId, toolResult.CallId);
        var outputContent = Assert.Single(toolResult.Outputs!);
        var textOutput = Assert.IsType<TextContent>(outputContent);
        Assert.Equal(actualContent.Parts[2].CodeExecutionResult!.Output, textOutput.Text);

        var text2 = Assert.IsType<TextContent>(contents[3]);
        Assert.Equal(actualContent.Parts[3].Text, text2.Text);
    }

    [Fact]
    public void CreateMappedChatResponse_WithCodeExecution_ShouldPreserveOutcomeInAdditionalProperties()
    {
        // Arrange
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(PythonCodeExecutionExampleResponse)!;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert — the execution outcome should be preserved so consumers can
        // distinguish success from failure.
        var toolResult = Assert.Single(result.Messages).Contents
            .OfType<CodeInterpreterToolResultContent>()
            .Single();

        Assert.NotNull(toolResult.AdditionalProperties);
        Assert.Equal("Ok", toolResult.AdditionalProperties["outcome"]);
    }

    [Fact]
    public void CreateMappedChatResponse_WithFailedCodeExecution_ShouldPreserveFailedOutcome()
    {
        // Arrange
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts =
                        [
                            new Part
                            {
                                ExecutableCode = new ExecutableCode
                                {
                                    Language = ExecutableCodeLanguage.Python,
                                    Code = "1/0",
                                },
                            },
                            new Part
                            {
                                CodeExecutionResult = new CodeExecutionResult
                                {
                                    Outcome = CodeExecutionResultOutcome.Failed,
                                    Output = "ZeroDivisionError: division by zero",
                                },
                            },
                        ],
                    },
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test-failed-exec",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var toolResult = Assert.Single(result.Messages).Contents
            .OfType<CodeInterpreterToolResultContent>()
            .Single();

        Assert.NotNull(toolResult.AdditionalProperties);
        Assert.Equal("Failed", toolResult.AdditionalProperties["outcome"]);
    }

    [Fact]
    public void CreateMappedChatResponse_WithUnspecifiedOutcome_ShouldNotSetAdditionalProperties()
    {
        // Arrange
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts =
                        [
                            new Part
                            {
                                ExecutableCode = new ExecutableCode
                                {
                                    Language = ExecutableCodeLanguage.Python,
                                    Code = "print('hi')",
                                },
                            },
                            new Part
                            {
                                CodeExecutionResult = new CodeExecutionResult
                                {
                                    Outcome = CodeExecutionResultOutcome.Unspecified,
                                    Output = "hi",
                                },
                            },
                        ],
                    },
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test-unspecified-exec",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert — Unspecified outcome should not pollute AdditionalProperties
        var toolResult = Assert.Single(result.Messages).Contents
            .OfType<CodeInterpreterToolResultContent>()
            .Single();

        Assert.Null(toolResult.AdditionalProperties);
    }

    [Fact]
    public void CreateMappedChatResponse_WithNullCodeExecutionOutput_ShouldReturnEmptyOutputs()
    {
        // Arrange
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts =
                        [
                            new Part
                            {
                                ExecutableCode = new ExecutableCode
                                {
                                    Language = ExecutableCodeLanguage.Python,
                                    Code = "x = 1",
                                },
                            },
                            new Part
                            {
                                CodeExecutionResult = new CodeExecutionResult
                                {
                                    Outcome = CodeExecutionResultOutcome.Ok,
                                    Output = null,
                                },
                            },
                        ],
                    },
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test-null-output-exec",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var toolResult = Assert.Single(result.Messages).Contents
            .OfType<CodeInterpreterToolResultContent>()
            .Single();

        Assert.Empty(toolResult.Outputs!);
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

    [Fact]
    public void CreateMappedChatResponse_WithThoughtFunctionCall_ShouldSetInformationalOnly()
    {
        // Arrange — a function call part with Thought=true represents the model
        // reasoning about calling a function, not requesting it.
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts =
                        [
                            new Part
                            {
                                Thought = true,
                                FunctionCall = new FunctionCall
                                {
                                    Id = "thought-call-1",
                                    Name = "get_weather",
                                    Arguments = JsonSerializer.SerializeToElement(
                                        new Dictionary<string, object> { ["city"] = "London" }),
                                },
                            },
                        ],
                    },
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            ModelVersion = "gemini-2.5-flash",
            ResponseId = "test-thought-fc",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var functionCall = Assert.IsType<FunctionCallContent>(Assert.Single(result.Messages).Contents[0]);
        Assert.True(functionCall.InformationalOnly);
        Assert.Equal("get_weather", functionCall.Name);
    }

    [Fact]
    public void CreateMappedChatResponse_WithRegularFunctionCall_ShouldNotSetInformationalOnly()
    {
        // Arrange — a regular function call (no Thought flag) should have InformationalOnly=false.
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts =
                        [
                            new Part
                            {
                                FunctionCall = new FunctionCall
                                {
                                    Id = "call-1",
                                    Name = "get_weather",
                                    Arguments = JsonSerializer.SerializeToElement(
                                        new Dictionary<string, object> { ["city"] = "Paris" }),
                                },
                            },
                        ],
                    },
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            ModelVersion = "gemini-2.5-flash",
            ResponseId = "test-regular-fc",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var functionCall = Assert.IsType<FunctionCallContent>(Assert.Single(result.Messages).Contents[0]);
        Assert.False(functionCall.InformationalOnly);
    }

    [Fact]
    public void CreateMappedChatResponse_WithFileData_ShouldMapToHostedFileContent()
    {
        // Arrange
        const string fileUri = "https://generativelanguage.googleapis.com/v1beta/files/abc123";
        const string mimeType = "text/csv";

        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts = [new Part { FileData = new FileData { FileUri = fileUri, MimeType = mimeType } }],
                    },
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test-filedata",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var content = Assert.Single(Assert.Single(result.Messages).Contents);
        var fileContent = Assert.IsType<HostedFileContent>(content);
        Assert.Equal(fileUri, fileContent.FileId);
        Assert.Equal(mimeType, fileContent.MediaType);
    }

    #region GroundingMetadata Mapping Tests

    [Fact]
    public void CreateMappedChatResponse_WithGroundingMetadata_ShouldMapToWebSearchContent()
    {
        // Arrange
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts = [new Part { Text = "Here are the best restaurants in NYC." }],
                    },
                    GroundingMetadata = new GroundingMetadata
                    {
                        WebSearchQueries = ["best restaurants NYC"],
                        GroundingChunks =
                        [
                            new GroundingChunk
                            {
                                Web = new Web { Uri = "https://example.com/restaurants", Title = "Top NYC Restaurants" }
                            },
                        ],
                    },
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test-grounding",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;

        Assert.IsType<TextContent>(contents[0]);

        var toolCall = Assert.IsType<WebSearchToolCallContent>(contents[1]);
        Assert.NotNull(toolCall.CallId);
        var query = Assert.Single(toolCall.Queries!);
        Assert.Equal("best restaurants NYC", query);

        var toolResult = Assert.IsType<WebSearchToolResultContent>(contents[2]);
        Assert.Equal(toolCall.CallId, toolResult.CallId);
        var uriContent = Assert.IsType<UriContent>(Assert.Single(toolResult.Outputs!));
        Assert.Equal("https://example.com/restaurants", uriContent.Uri.ToString());
        Assert.Equal("text/html", uriContent.MediaType);
        Assert.Equal("Top NYC Restaurants", uriContent.AdditionalProperties!["title"]);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithGroundingMetadata_ShouldAppendBeforeUsageContent()
    {
        // Arrange
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts = [new Part { Text = "Search result summary." }],
                    },
                    GroundingMetadata = new GroundingMetadata
                    {
                        WebSearchQueries = ["test query"],
                        GroundingChunks =
                        [
                            new GroundingChunk { Web = new Web { Uri = "https://example.com", Title = "Example" } },
                        ],
                    },
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            UsageMetadata = new UsageMetadata
            {
                PromptTokenCount = 10,
                CandidatesTokenCount = 20,
                TotalTokenCount = 30,
            },
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test-grounding-streaming",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, DateTimeOffset.UtcNow);

        // Assert — web search content appears after text but before UsageContent
        var contents = result.Contents;
        Assert.IsType<TextContent>(contents[0]);
        Assert.IsType<WebSearchToolCallContent>(contents[1]);
        Assert.IsType<WebSearchToolResultContent>(contents[2]);
        Assert.IsType<UsageContent>(contents[3]);
    }

    [Fact]
    public void CreateMappedChatResponse_WithNullGroundingMetadata_ShouldProduceNoWebSearchContent()
    {
        // Arrange
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts = [new Part { Text = "No grounding here." }],
                    },
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test-no-grounding",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        Assert.DoesNotContain(contents, c => c is WebSearchToolCallContent);
        Assert.DoesNotContain(contents, c => c is WebSearchToolResultContent);
    }

    [Fact]
    public void CreateMappedChatResponse_WithEmptyGroundingMetadata_ShouldProduceNoWebSearchContent()
    {
        // Arrange — GroundingMetadata is present but both collections are null
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts = [new Part { Text = "Empty grounding." }],
                    },
                    GroundingMetadata = new GroundingMetadata(),
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test-empty-grounding",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        Assert.DoesNotContain(contents, c => c is WebSearchToolCallContent);
        Assert.DoesNotContain(contents, c => c is WebSearchToolResultContent);
    }

    #endregion

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
