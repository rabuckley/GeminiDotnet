using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.Models;
using Microsoft.Extensions.AI;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;

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

    #region Server-Side Tool Invocation Mapping Tests

    [Fact]
    public void CreateMappedChatResponse_WithServerSideToolCall_ShouldMapToToolCallContent()
    {
        // Arrange
        var arguments = JsonSerializer.Deserialize<JsonElement>("""{"query":"weather in London"}""");

        var response = ResponseWithParts(new Part
        {
            ToolCall = new ToolCall
            {
                Id = "call-1",
                ToolName = "google_search",
                ToolType = ToolType.GoogleSearchWeb,
                Arguments = arguments,
            },
            ThoughtSignature = "signature",
        });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var toolCall = Assert.IsType<ToolCallContent>(Assert.Single(Assert.Single(result.Messages).Contents));

        Assert.Equal("call-1", toolCall.CallId);

        var properties = Assert.IsType<AdditionalPropertiesDictionary>(toolCall.AdditionalProperties);
        Assert.Equal(ToolType.GoogleSearchWeb, properties[GeminiToolInvocationProperties.ToolType]);
        Assert.Equal("google_search", properties[GeminiToolInvocationProperties.ToolName]);
        Assert.Equal("signature", properties[GeminiToolInvocationProperties.ThoughtSignature]);
        Assert.Equal(
            arguments.GetRawText(),
            Assert.IsType<JsonElement>(properties[GeminiToolInvocationProperties.Arguments]).GetRawText());
    }

    [Fact]
    public void CreateMappedChatResponse_WithServerSideToolResponse_ShouldMapToToolResultContent()
    {
        // Arrange
        var toolResponse = JsonSerializer.Deserialize<JsonElement>("""{"results":["18C and raining"]}""");

        var response = ResponseWithParts(new Part
        {
            ToolResponse = new ToolResponse
            {
                Id = "call-1",
                ToolType = ToolType.GoogleSearchWeb,
                Response = toolResponse,
            },
        });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var toolResult = Assert.IsType<ToolResultContent>(Assert.Single(Assert.Single(result.Messages).Contents));

        Assert.Equal("call-1", toolResult.CallId);

        var properties = Assert.IsType<AdditionalPropertiesDictionary>(toolResult.AdditionalProperties);
        Assert.Equal(ToolType.GoogleSearchWeb, properties[GeminiToolInvocationProperties.ToolType]);
        Assert.Equal(
            toolResponse.GetRawText(),
            Assert.IsType<JsonElement>(properties[GeminiToolInvocationProperties.Response]).GetRawText());
    }

    [Fact]
    public void CreateMappedChatResponse_WithIdLessServerSideToolInvocation_ShouldCorrelateTheSynthesizedCallId()
    {
        // Arrange — ToolCall.Id and ToolResponse.Id are both optional on the wire, but
        // ToolCallContent.CallId is not, and the pair still has to correlate.
        var response = ResponseWithParts(
            new Part { ToolCall = new ToolCall { ToolType = ToolType.UrlContext } },
            new Part { ToolResponse = new ToolResponse { ToolType = ToolType.UrlContext } });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        var toolCall = Assert.IsType<ToolCallContent>(contents[0]);
        var toolResult = Assert.IsType<ToolResultContent>(contents[1]);

        Assert.NotEmpty(toolCall.CallId);
        Assert.Equal(toolCall.CallId, toolResult.CallId);
    }

    [Fact]
    public void CreateMappedChatResponse_WithParallelIdLessServerSideToolInvocations_ShouldCorrelateInOrder()
    {
        // Arrange — Gemini can run several built-in tools in one turn and report the responses after
        // the calls, so each response has to pair with its own call, not with whichever came last.
        var response = ResponseWithParts(
            new Part { ToolCall = new ToolCall { ToolType = ToolType.GoogleSearchWeb } },
            new Part { ToolCall = new ToolCall { ToolType = ToolType.UrlContext } },
            new Part { ToolResponse = new ToolResponse { ToolType = ToolType.GoogleSearchWeb } },
            new Part { ToolResponse = new ToolResponse { ToolType = ToolType.UrlContext } });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        var firstCall = Assert.IsType<ToolCallContent>(contents[0]);
        var secondCall = Assert.IsType<ToolCallContent>(contents[1]);
        var firstResult = Assert.IsType<ToolResultContent>(contents[2]);
        var secondResult = Assert.IsType<ToolResultContent>(contents[3]);

        Assert.NotEqual(firstCall.CallId, secondCall.CallId);
        Assert.Equal(firstCall.CallId, firstResult.CallId);
        Assert.Equal(secondCall.CallId, secondResult.CallId);
    }

    [Fact]
    public void CreateMappedChatResponse_WithAnIdLessServerSideToolCall_ShouldNotReportTheSynthesizedCallId()
    {
        // Arrange — the synthesized CallId correlates the pair for M.E.AI consumers, but it is not an
        // id Gemini issued, so nothing may echo it back as one.
        var response = ResponseWithParts(
            new Part { ToolCall = new ToolCall { ToolType = ToolType.UrlContext } },
            new Part { ToolResponse = new ToolResponse { ToolType = ToolType.UrlContext } });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;

        Assert.DoesNotContain(
            GeminiToolInvocationProperties.Id,
            Assert.IsType<ToolCallContent>(contents[0]).AdditionalProperties!.Keys);

        Assert.DoesNotContain(
            GeminiToolInvocationProperties.Id,
            Assert.IsType<ToolResultContent>(contents[1]).AdditionalProperties!.Keys);
    }

    [Fact]
    public void CreateMappedChatResponse_WithServerSideToolInvocationAmongText_ShouldMapOneContentPerPart()
    {
        // Arrange — Segment.PartIndex indexes the mapped contents, so the 1:1 order must hold.
        var response = ResponseWithParts(
            new Part { Text = "Let me look that up." },
            new Part { ToolCall = new ToolCall { Id = "call-1", ToolType = ToolType.GoogleSearchWeb } },
            new Part { ToolResponse = new ToolResponse { Id = "call-1", ToolType = ToolType.GoogleSearchWeb } },
            new Part { Text = "It is raining." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;

        Assert.Equal(4, contents.Count);
        Assert.Equal("Let me look that up.", Assert.IsType<TextContent>(contents[0]).Text);
        Assert.IsType<ToolCallContent>(contents[1]);
        Assert.IsType<ToolResultContent>(contents[2]);
        Assert.Equal("It is raining.", Assert.IsType<TextContent>(contents[3]).Text);
    }

    [Fact]
    public void CreateMappedChatResponse_WithAnUnrecognisedPart_ShouldThrowGeminiMappingException()
    {
        // Arrange — a part carrying only a thought signature has no field this mapper reads.
        var response = ResponseWithParts(new Part { ThoughtSignature = "signature" });

        // Act
        void Act() => GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    private static GenerateContentResponse ResponseWithParts(params Part[] parts) => new()
    {
        Candidates = [new Candidate { Content = new Content { Role = "model", Parts = parts } }],
    };

    #endregion

    #region Candidate Role Mapping Tests

    [Fact]
    public void CreateMappedChatResponse_WithNoContent_ShouldMapToAssistantRole()
    {
        // Arrange
        var response = new GenerateContentResponse
        {
            Candidates = [new Candidate { Content = null, FinishReason = CandidateFinishReason.Stop }],
            ModelVersion = "gemini-2.0-flash",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(ChatRole.Assistant, Assert.Single(result.Messages).Role);
    }

    [Fact]
    public void CreateMappedChatResponse_WithoutRole_ShouldMapToAssistantRole()
    {
        // Arrange
        var response = ResponseWithCandidateRole(null);

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(ChatRole.Assistant, Assert.Single(result.Messages).Role);
    }

    [Fact]
    public void CreateMappedChatResponse_WithModelRole_ShouldMapToAssistantRole()
    {
        // Arrange
        var response = ResponseWithCandidateRole("model");

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(ChatRole.Assistant, Assert.Single(result.Messages).Role);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithoutRole_ShouldMapToAssistantRole()
    {
        // Arrange
        var response = ResponseWithCandidateRole(null);

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(ChatRole.Assistant, result.Role);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithModelRole_ShouldMapToAssistantRole()
    {
        // Arrange
        var response = ResponseWithCandidateRole("model");

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(ChatRole.Assistant, result.Role);
    }

    [Fact]
    public void CreateMappedChatResponse_WithUserRole_ShouldMapToUserRole()
    {
        // Arrange
        var response = ResponseWithCandidateRole("user");

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(ChatRole.User, Assert.Single(result.Messages).Role);
    }

    [Fact]
    public void CreateMappedChatResponse_WithUnsupportedRole_ShouldThrow()
    {
        // Arrange
        var response = ResponseWithCandidateRole("assistant");

        // Act
        void Act() => GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        Assert.Throws<GeminiMappingException>(Act);
    }

    private static GenerateContentResponse ResponseWithCandidateRole(string? role) => new()
    {
        Candidates =
        [
            new Candidate
            {
                Content = new Content { Role = role, Parts = [new Part { Text = "The answer is 42." }] },
                FinishReason = CandidateFinishReason.Stop,
            },
        ],
        ModelVersion = "gemini-2.0-flash",
    };

    #endregion

    #region GroundingMetadata Mapping Tests

    [Fact]
    public void CreateMappedChatResponse_WithWebGroundingChunk_ShouldProduceCitationAnnotation()
    {
        // Arrange
        var response = CreateGroundedResponse(
            new GroundingMetadata
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
            new Part { Text = "Here are the best restaurants in NYC." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;

        var text = Assert.IsType<TextContent>(contents[0]);
        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!));
        Assert.Equal(GeminiToolNames.GoogleSearch, citation.ToolName);
        Assert.Equal("Top NYC Restaurants", citation.Title);
        Assert.Equal("https://example.com/restaurants", citation.Url!.ToString());

        var toolCall = Assert.IsType<WebSearchToolCallContent>(contents[1]);
        Assert.Equal("best restaurants NYC", Assert.Single(toolCall.Queries!));

        // The sources live on the annotations, so the result carries no outputs.
        var toolResult = Assert.IsType<WebSearchToolResultContent>(contents[2]);
        Assert.Equal(toolCall.CallId, toolResult.CallId);
        Assert.Null(toolResult.Outputs);
    }

    [Fact]
    public void CreateMappedChatResponse_WithRetrievedContextWithoutUri_ShouldCarryTitleSnippetAndFileId()
    {
        // Arrange — the shape a file search produces: evidence, but no search was run. retrievedContext
        // frequently omits uri, which is why the evidence is an annotation rather than a UriContent.
        var response = CreateGroundedResponse(
            new GroundingMetadata
            {
                GroundingChunks =
                [
                    new GroundingChunk
                    {
                        RetrievedContext = new RetrievedContext
                        {
                            Title = "Poems",
                            Text = "The tenth muse.",
                            FileSearchStore = "fileSearchStores/poems",
                            MediaId = "fileSearchStores/poems/media/blob123",
                            PageNumber = 4,
                            Uri = null,
                        },
                    },
                ],
            },
            new Part { Text = "A line from the poem." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var text = Assert.IsType<TextContent>(Assert.Single(Assert.Single(result.Messages).Contents));
        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!));

        Assert.Equal(GeminiToolNames.FileSearch, citation.ToolName);
        Assert.Equal("Poems", citation.Title);
        Assert.Equal("The tenth muse.", citation.Snippet);
        Assert.Equal("fileSearchStores/poems/media/blob123", citation.FileId);
        Assert.Null(citation.Url);
        Assert.Equal(4, citation.AdditionalProperties![GeminiCitationProperties.PageNumber]);
        Assert.Equal(
            "fileSearchStores/poems",
            citation.AdditionalProperties[GeminiCitationProperties.FileSearchStore]);
    }

    [Fact]
    public void CreateMappedChatResponse_WithRetrievedContextWithoutMediaId_ShouldLeaveFileIdUnset()
    {
        // Arrange
        var response = CreateGroundedResponse(
            new GroundingMetadata
            {
                GroundingChunks =
                [
                    new GroundingChunk
                    {
                        RetrievedContext = new RetrievedContext
                        {
                            Title = "Poems",
                            FileSearchStore = "fileSearchStores/poems",
                        },
                    },
                ],
            },
            new Part { Text = "A line from the poem." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert — a store is not a file, so it must not masquerade as one.
        var text = Assert.IsType<TextContent>(Assert.Single(Assert.Single(result.Messages).Contents));
        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!));

        Assert.Null(citation.FileId);
        Assert.Equal(
            "fileSearchStores/poems",
            citation.AdditionalProperties![GeminiCitationProperties.FileSearchStore]);
    }

    [Fact]
    public void CreateMappedChatResponse_WithCustomMetadata_ShouldFlattenToSerializableValues()
    {
        // Arrange — AdditionalProperties is serialized with the consumer's options, so the values
        // must be types Microsoft.Extensions.AI's own serializer context knows.
        var response = CreateGroundedResponse(
            new GroundingMetadata
            {
                GroundingChunks =
                [
                    new GroundingChunk
                    {
                        RetrievedContext = new RetrievedContext
                        {
                            Title = "Poems",
                            CustomMetadata =
                            [
                                new GroundingChunkCustomMetadata { Key = "author", StringValue = "Bradstreet" },
                                new GroundingChunkCustomMetadata { Key = "year", NumericValue = 1650 },
                                new GroundingChunkCustomMetadata
                                {
                                    Key = "tags",
                                    StringListValue = new GroundingChunkStringList { Values = ["verse", "colonial"] },
                                },
                                new GroundingChunkCustomMetadata { StringValue = "no key, dropped" },
                            ],
                        },
                    },
                ],
            },
            new Part { Text = "A line from the poem." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var text = Assert.IsType<TextContent>(Assert.Single(Assert.Single(result.Messages).Contents));
        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!));
        var metadata = Assert.IsType<AdditionalPropertiesDictionary>(
            citation.AdditionalProperties![GeminiCitationProperties.CustomMetadata]);

        Assert.Equal(3, metadata.Count);
        Assert.Equal("Bradstreet", metadata["author"]);
        Assert.Equal(1650f, metadata["year"]);

        var tags = Assert.IsType<JsonArray>(metadata["tags"]);
        Assert.Equal(["verse", "colonial"], tags.Select(t => t!.GetValue<string>()));
    }

    [Fact]
    public void CreateMappedChatResponse_WithOneSupportCitingTwoChunks_ShouldGiveEachAnnotationItsOwnRegion()
    {
        // Arrange
        var response = CreateGroundedResponse(
            new GroundingMetadata
            {
                GroundingChunks =
                [
                    new GroundingChunk { Web = new Web { Uri = "https://one.example", Title = "One" } },
                    new GroundingChunk { Web = new Web { Uri = "https://two.example", Title = "Two" } },
                ],
                GroundingSupports =
                [
                    new GroundingSupport
                    {
                        GroundingChunkIndices = new[] { 0, 1 },
                        Segment = new Segment { PartIndex = 0, StartIndex = 0, EndIndex = 5 },
                    },
                ],
            },
            new Part { Text = "Hello world." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert — TextSpanAnnotatedRegion is mutable, so a shared instance would let an edit on one
        // citation reach the other.
        var text = Assert.IsType<TextContent>(Assert.Single(Assert.Single(result.Messages).Contents));
        var citations = text.Annotations!.OfType<CitationAnnotation>().ToList();
        Assert.Equal(2, citations.Count);

        var first = Assert.Single(citations[0].AnnotatedRegions!);
        var second = Assert.Single(citations[1].AnnotatedRegions!);

        Assert.NotSame(first, second);

        Assert.IsType<TextSpanAnnotatedRegion>(first).EndIndex = 11;
        Assert.Equal(5, Assert.IsType<TextSpanAnnotatedRegion>(second).EndIndex);
    }

    [Fact]
    public void CreateMappedChatResponse_WithImageAndMapsChunks_ShouldProduceCitationAnnotations()
    {
        // Arrange
        var response = CreateGroundedResponse(
            new GroundingMetadata
            {
                GroundingChunks =
                [
                    new GroundingChunk
                    {
                        Image = new Image
                        {
                            Title = "A photo of the Shard",
                            SourceUri = "https://example.com/shard",
                            ImageUri = "https://example.com/shard.jpg",
                            Domain = "example.com",
                        },
                    },
                    new GroundingChunk
                    {
                        Maps = new Maps
                        {
                            Title = "The Shard",
                            Uri = "https://maps.example.com/shard",
                            PlaceId = "places/shard-123",
                            Text = "A 72-storey skyscraper in Southwark.",
                        },
                    },
                ],
            },
            new Part { Text = "The Shard is in London." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var text = Assert.IsType<TextContent>(Assert.Single(Assert.Single(result.Messages).Contents));
        var citations = text.Annotations!.OfType<CitationAnnotation>().ToList();
        Assert.Equal(2, citations.Count);

        Assert.Equal(GeminiToolNames.GoogleSearch, citations[0].ToolName);
        Assert.Equal("A photo of the Shard", citations[0].Title);
        Assert.Equal("https://example.com/shard", citations[0].Url!.ToString());
        Assert.Equal(
            "https://example.com/shard.jpg",
            citations[0].AdditionalProperties![GeminiCitationProperties.ImageUri]);
        Assert.Equal("example.com", citations[0].AdditionalProperties!["domain"]);

        Assert.Equal(GeminiToolNames.GoogleMaps, citations[1].ToolName);
        Assert.Equal("The Shard", citations[1].Title);
        Assert.Equal("places/shard-123", citations[1].FileId);
        Assert.Equal("A 72-storey skyscraper in Southwark.", citations[1].Snippet);
    }

    [Fact]
    public void CreateMappedChatResponse_WithGroundingSupport_ShouldAnnotateRegionOnNamedPart()
    {
        // Arrange
        var response = CreateGroundedResponse(
            new GroundingMetadata
            {
                GroundingChunks =
                [
                    new GroundingChunk { Web = new Web { Uri = "https://example.com", Title = "Example" } },
                ],
                GroundingSupports =
                [
                    new GroundingSupport
                    {
                        GroundingChunkIndices = new[] { 0 },
                        Segment = new Segment { PartIndex = 1, StartIndex = 6, EndIndex = 11 },
                    },
                ],
            },
            new Part { Text = "First part." },
            new Part { Text = "Hello world, again." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        Assert.Null(Assert.IsType<TextContent>(contents[0]).Annotations);

        var text = Assert.IsType<TextContent>(contents[1]);
        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!));
        var region = Assert.IsType<TextSpanAnnotatedRegion>(Assert.Single(citation.AnnotatedRegions!));

        Assert.Equal("world", text.Text[region.StartIndex!.Value..region.EndIndex!.Value]);
    }

    [Fact]
    public void CreateMappedChatResponse_WithMultiByteSegment_ShouldConvertByteOffsetsToCharacterIndices()
    {
        // Arrange — "Héllo wörld" is 13 UTF-8 bytes but 11 UTF-16 characters. Gemini's segment
        // offsets are bytes; TextSpanAnnotatedRegion's are characters.
        const string answer = "Héllo wörld";

        var response = CreateGroundedResponse(
            new GroundingMetadata
            {
                GroundingChunks =
                [
                    new GroundingChunk { Web = new Web { Uri = "https://example.com", Title = "Example" } },
                ],
                GroundingSupports =
                [
                    new GroundingSupport
                    {
                        GroundingChunkIndices = new[] { 0 },
                        Segment = new Segment { PartIndex = 0, StartIndex = 7, EndIndex = 13 },
                    },
                ],
            },
            new Part { Text = answer });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var text = Assert.IsType<TextContent>(Assert.Single(Assert.Single(result.Messages).Contents));
        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!));
        var region = Assert.IsType<TextSpanAnnotatedRegion>(Assert.Single(citation.AnnotatedRegions!));

        Assert.Equal(6, region.StartIndex);
        Assert.Equal(11, region.EndIndex);
        Assert.Equal("wörld", answer[region.StartIndex!.Value..region.EndIndex!.Value]);
    }

    [Fact]
    public void CreateMappedChatResponse_WithOutOfRangeSupportIndices_ShouldAnnotateWithoutRegion()
    {
        // Arrange
        var response = CreateGroundedResponse(
            new GroundingMetadata
            {
                GroundingChunks =
                [
                    new GroundingChunk { Web = new Web { Uri = "https://example.com", Title = "Example" } },
                ],
                GroundingSupports =
                [
                    new GroundingSupport
                    {
                        GroundingChunkIndices = new[] { 0 },
                        Segment = new Segment { PartIndex = 7, StartIndex = 0, EndIndex = 5 },
                    },
                    new GroundingSupport
                    {
                        GroundingChunkIndices = new[] { 9 },
                        Segment = new Segment { PartIndex = 0, StartIndex = 0, EndIndex = 5 },
                    },
                ],
            },
            new Part { Text = "Hello world." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var text = Assert.IsType<TextContent>(Assert.Single(Assert.Single(result.Messages).Contents));
        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!));
        Assert.Null(citation.AnnotatedRegions);
    }

    [Fact]
    public void CreateMappedChatResponse_WithChunkNoSupportReferences_ShouldAnnotateWithoutRegion()
    {
        // Arrange
        var response = CreateGroundedResponse(
            new GroundingMetadata
            {
                GroundingChunks =
                [
                    new GroundingChunk { Web = new Web { Uri = "https://cited.example", Title = "Cited" } },
                    new GroundingChunk { Web = new Web { Uri = "https://loose.example", Title = "Loose" } },
                ],
                GroundingSupports =
                [
                    new GroundingSupport
                    {
                        GroundingChunkIndices = new[] { 0 },
                        Segment = new Segment { PartIndex = 0, StartIndex = 0, EndIndex = 5 },
                    },
                ],
            },
            new Part { Text = "Hello world." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var text = Assert.IsType<TextContent>(Assert.Single(Assert.Single(result.Messages).Contents));
        var citations = text.Annotations!.OfType<CitationAnnotation>().ToList();
        Assert.Equal(2, citations.Count);

        Assert.Equal("Cited", citations[0].Title);
        Assert.NotNull(citations[0].AnnotatedRegions);

        Assert.Equal("Loose", citations[1].Title);
        Assert.Null(citations[1].AnnotatedRegions);
    }

    [Fact]
    public void CreateMappedChatResponse_WithGroundingMetadataAndNoTextPart_ShouldStillSurfaceAnnotations()
    {
        // Arrange
        var response = CreateGroundedResponse(
            new GroundingMetadata
            {
                GroundingChunks =
                [
                    new GroundingChunk { Web = new Web { Uri = "https://example.com", Title = "Example" } },
                ],
            },
            new Part { FileData = new FileData { FileUri = "files/abc123", MimeType = "text/plain" } });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        var text = Assert.IsType<TextContent>(contents[1]);
        Assert.Equal(string.Empty, text.Text);

        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!));
        Assert.Equal("Example", citation.Title);
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
        var text = Assert.IsType<TextContent>(contents[0]);
        Assert.IsType<WebSearchToolCallContent>(contents[1]);
        Assert.IsType<WebSearchToolResultContent>(contents[2]);
        Assert.IsType<UsageContent>(contents[3]);

        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!));
        Assert.Equal("Example", citation.Title);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithGroundingSupports_ShouldNotAttachRegions()
    {
        // Arrange — streaming segment indices are cumulative across responses, so a per-update mapper
        // cannot place them; the citation is emitted without a region.
        var response = new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content
                    {
                        Role = "model",
                        Parts = [new Part { Text = "Hello world." }],
                    },
                    GroundingMetadata = new GroundingMetadata
                    {
                        GroundingChunks =
                        [
                            new GroundingChunk { Web = new Web { Uri = "https://example.com", Title = "Example" } },
                        ],
                        GroundingSupports =
                        [
                            new GroundingSupport
                            {
                                GroundingChunkIndices = new[] { 0 },
                                Segment = new Segment { PartIndex = 0, StartIndex = 0, EndIndex = 5 },
                            },
                        ],
                    },
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test-grounding-streaming-supports",
        };

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, DateTimeOffset.UtcNow);

        // Assert
        var text = Assert.IsType<TextContent>(Assert.Single(result.Contents));
        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!));
        Assert.Null(citation.AnnotatedRegions);
    }

    [Fact]
    public void CreateMappedChatResponse_WithNullGroundingMetadata_ShouldProduceNoGroundingContent()
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
        Assert.DoesNotContain(contents, c => c is WebSearchToolCallContent or WebSearchToolResultContent);
        Assert.Null(Assert.IsType<TextContent>(Assert.Single(contents)).Annotations);
    }

    [Fact]
    public void CreateMappedChatResponse_WithEmptyGroundingMetadata_ShouldProduceNoGroundingContent()
    {
        // Arrange
        var response = CreateGroundedResponse(new GroundingMetadata(), new Part { Text = "Empty grounding." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        Assert.DoesNotContain(contents, c => c is WebSearchToolCallContent or WebSearchToolResultContent);
        Assert.Null(Assert.IsType<TextContent>(Assert.Single(contents)).Annotations);
    }

    private static GenerateContentResponse CreateGroundedResponse(
        GroundingMetadata groundingMetadata,
        params Part[] parts)
    {
        return new GenerateContentResponse
        {
            Candidates =
            [
                new Candidate
                {
                    Content = new Content { Role = "model", Parts = parts },
                    GroundingMetadata = groundingMetadata,
                    FinishReason = CandidateFinishReason.Stop,
                },
            ],
            ModelVersion = "gemini-2.0-flash",
            ResponseId = "test-grounding",
        };
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
