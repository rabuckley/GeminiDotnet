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
        Assert.True(toolResult.AdditionalProperties.TryGetGeminiValue(
            GeminiContentProperties.Outcome, out CodeExecutionResultOutcome outcome));
        Assert.Equal(CodeExecutionResultOutcome.Ok, outcome);
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
        Assert.True(toolResult.AdditionalProperties.TryGetGeminiValue(
            GeminiContentProperties.Outcome, out CodeExecutionResultOutcome outcome));
        Assert.Equal(CodeExecutionResultOutcome.Failed, outcome);
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

    #region Code Execution Mapping Tests

    [Fact]
    public void CreateMappedChatResponse_WithCodeExecutionIds_ShouldUseTheServerIds()
    {
        // Arrange — live responses give the executableCode and its codeExecutionResult the same id
        // (probed 2026-09-01), so that id, not a synthesized one, is what correlates the pair.
        var response = ResponseWithParts(
            new Part
            {
                ExecutableCode = new ExecutableCode
                {
                    Id = "call_318937", Language = ExecutableCodeLanguage.Python, Code = "print(1)",
                },
            },
            new Part
            {
                CodeExecutionResult = new CodeExecutionResult
                {
                    Id = "call_318937", Outcome = CodeExecutionResultOutcome.Ok, Output = "1",
                },
            });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        var call = Assert.IsType<CodeInterpreterToolCallContent>(contents[0]);
        var codeResult = Assert.IsType<CodeInterpreterToolResultContent>(contents[1]);

        Assert.Equal("call_318937", call.CallId);
        Assert.Equal("call_318937", codeResult.CallId);
        Assert.Equal("call_318937", call.AdditionalProperties![GeminiContentProperties.Id]);
        Assert.Equal("call_318937", codeResult.AdditionalProperties![GeminiContentProperties.Id]);
    }

    [Fact]
    public void CreateMappedChatResponse_WithInterleavedIdLessCodeExecutions_ShouldCorrelateInOrder()
    {
        // Arrange — with no ids on the wire, each result pairs with the oldest unanswered call.
        var response = ResponseWithParts(
            new Part { ExecutableCode = new ExecutableCode { Language = ExecutableCodeLanguage.Python, Code = "a" } },
            new Part { ExecutableCode = new ExecutableCode { Language = ExecutableCodeLanguage.Python, Code = "b" } },
            new Part { CodeExecutionResult = new CodeExecutionResult { Outcome = CodeExecutionResultOutcome.Ok } },
            new Part { CodeExecutionResult = new CodeExecutionResult { Outcome = CodeExecutionResultOutcome.Ok } });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        var firstCall = Assert.IsType<CodeInterpreterToolCallContent>(contents[0]);
        var secondCall = Assert.IsType<CodeInterpreterToolCallContent>(contents[1]);
        var firstResult = Assert.IsType<CodeInterpreterToolResultContent>(contents[2]);
        var secondResult = Assert.IsType<CodeInterpreterToolResultContent>(contents[3]);

        Assert.NotEqual(firstCall.CallId, secondCall.CallId);
        Assert.Equal(firstCall.CallId, firstResult.CallId);
        Assert.Equal(secondCall.CallId, secondResult.CallId);
        Assert.Null(firstCall.AdditionalProperties);
        Assert.DoesNotContain(GeminiContentProperties.Id, firstResult.AdditionalProperties!.Keys);
    }

    [Fact]
    public void CreateMappedChatResponse_WithCodeExecutionThoughtSignature_ShouldPreserveItInAdditionalProperties()
    {
        // Arrange — Gemini needs the signature echoed back, and RawRepresentation does not survive JSON.
        var response = ResponseWithParts(
            new Part
            {
                ExecutableCode = new ExecutableCode { Language = ExecutableCodeLanguage.Python, Code = "print(1)" },
                ThoughtSignature = "signature",
            },
            new Part
            {
                CodeExecutionResult = new CodeExecutionResult { Outcome = CodeExecutionResultOutcome.Ok, Output = "1" },
            });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        var call = Assert.IsType<CodeInterpreterToolCallContent>(contents[0]);
        var codeResult = Assert.IsType<CodeInterpreterToolResultContent>(contents[1]);

        Assert.Equal("signature", call.AdditionalProperties![GeminiContentProperties.ThoughtSignature]);
        Assert.DoesNotContain(GeminiContentProperties.ThoughtSignature, codeResult.AdditionalProperties!.Keys);
    }

    #endregion

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
                ToolName = "url_context",
                ToolType = ToolType.UrlContext,
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
        Assert.Equal(ToolType.UrlContext, properties[GeminiContentProperties.ToolType]);
        Assert.Equal("url_context", properties[GeminiContentProperties.ToolName]);
        Assert.Equal("signature", properties[GeminiContentProperties.ThoughtSignature]);
        Assert.Equal(
            arguments.GetRawText(),
            Assert.IsType<JsonElement>(properties[GeminiContentProperties.Arguments]).GetRawText());
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
                ToolType = ToolType.UrlContext,
                Response = toolResponse,
            },
        });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var toolResult = Assert.IsType<ToolResultContent>(Assert.Single(Assert.Single(result.Messages).Contents));

        Assert.Equal("call-1", toolResult.CallId);

        var properties = Assert.IsType<AdditionalPropertiesDictionary>(toolResult.AdditionalProperties);
        Assert.Equal(ToolType.UrlContext, properties[GeminiContentProperties.ToolType]);
        Assert.Equal(
            toolResponse.GetRawText(),
            Assert.IsType<JsonElement>(properties[GeminiContentProperties.Response]).GetRawText());
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
            new Part { ToolCall = new ToolCall { ToolType = ToolType.FileSearch } },
            new Part { ToolCall = new ToolCall { ToolType = ToolType.UrlContext } },
            new Part { ToolResponse = new ToolResponse { ToolType = ToolType.FileSearch } },
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
            GeminiContentProperties.Id,
            Assert.IsType<ToolCallContent>(contents[0]).AdditionalProperties!.Keys);

        Assert.DoesNotContain(
            GeminiContentProperties.Id,
            Assert.IsType<ToolResultContent>(contents[1]).AdditionalProperties!.Keys);
    }

    [Fact]
    public void CreateMappedChatResponse_WithServerSideToolInvocationAmongText_ShouldMapOneContentPerPart()
    {
        // Arrange — Segment.PartIndex indexes the mapped contents, so the 1:1 order must hold.
        var response = ResponseWithParts(
            new Part { Text = "Let me look that up." },
            new Part { ToolCall = new ToolCall { Id = "call-1", ToolType = ToolType.UrlContext } },
            new Part { ToolResponse = new ToolResponse { Id = "call-1", ToolType = ToolType.UrlContext } },
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

    [Fact]
    public void CreateMappedChatResponse_WithAGoogleSearchInvocation_ShouldMapToTheWebSearchPair()
    {
        // Arrange — the invocation parts are the better record of a search than the pair synthesized from
        // groundingMetadata: they carry the id Gemini issued and the thought signature that led to it.
        var arguments = JsonSerializer.Deserialize<JsonElement>("""{"queries":["weather in London"]}""");
        var toolResponse = JsonSerializer.Deserialize<JsonElement>("""{"search_suggestions":"<html>"}""");

        var response = ResponseWithParts(
            new Part
            {
                ToolCall = new ToolCall
                {
                    Id = "call-1",
                    ToolName = "google_search",
                    ToolType = ToolType.GoogleSearchWeb,
                    Arguments = arguments,
                },
                ThoughtSignature = "call-signature",
            },
            new Part
            {
                ToolResponse = new ToolResponse
                {
                    Id = "call-1",
                    ToolType = ToolType.GoogleSearchWeb,
                    Response = toolResponse,
                },
                ThoughtSignature = "response-signature",
            });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        var call = Assert.IsType<WebSearchToolCallContent>(contents[0]);
        var searchResult = Assert.IsType<WebSearchToolResultContent>(contents[1]);

        Assert.Equal("call-1", call.CallId);
        Assert.Equal("call-1", searchResult.CallId);
        Assert.Equal(["weather in London"], call.Queries);

        // The sources live on the citation annotations and the raw response on the properties below.
        Assert.Null(searchResult.Outputs);

        var callProperties = Assert.IsType<AdditionalPropertiesDictionary>(call.AdditionalProperties);
        Assert.Equal(ToolType.GoogleSearchWeb, callProperties[GeminiContentProperties.ToolType]);
        Assert.Equal("google_search", callProperties[GeminiContentProperties.ToolName]);
        Assert.Equal("call-signature", callProperties[GeminiContentProperties.ThoughtSignature]);
        Assert.Equal(
            arguments.GetRawText(),
            Assert.IsType<JsonElement>(callProperties[GeminiContentProperties.Arguments]).GetRawText());

        var resultProperties = Assert.IsType<AdditionalPropertiesDictionary>(searchResult.AdditionalProperties);
        Assert.Equal(ToolType.GoogleSearchWeb, resultProperties[GeminiContentProperties.ToolType]);
        Assert.Equal("response-signature", resultProperties[GeminiContentProperties.ThoughtSignature]);
        Assert.Equal(
            toolResponse.GetRawText(),
            Assert.IsType<JsonElement>(resultProperties[GeminiContentProperties.Response]).GetRawText());
    }

    [Theory]
    [InlineData("""{}""")]
    [InlineData("""{"queries":"weather in London"}""")]
    [InlineData("""{"queries":[1]}""")]
    public void CreateMappedChatResponse_WithAGoogleSearchInvocationWithoutQueries_ShouldReportNoQueries(
        string argumentsJson)
    {
        // Arrange — args.queries is the only place the invocation states what was searched for, and a
        // future revision could drop or rename it. An unreadable value is reported as none rather than
        // failing the response.
        var response = ResponseWithParts(new Part
        {
            ToolCall = new ToolCall
            {
                Id = "call-1",
                ToolType = ToolType.GoogleSearchWeb,
                Arguments = JsonSerializer.Deserialize<JsonElement>(argumentsJson),
            },
        });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var call = Assert.IsType<WebSearchToolCallContent>(Assert.Single(Assert.Single(result.Messages).Contents));
        Assert.Null(call.Queries);
    }

    [Fact]
    public void CreateMappedChatResponse_WithAQuerylessInvocationAndGrounding_ShouldReportTheInvocationAlone()
    {
        // Arrange — the invocation is the complete record of the search. webSearchQueries is cumulative
        // across the turn, so it cannot be attributed to this call and is not reported under its id.
        var response = CreateGroundedResponse(
            new GroundingMetadata { WebSearchQueries = ["weather in London"] },
            new Part { ToolCall = new ToolCall { Id = "call-1", ToolType = ToolType.GoogleSearchWeb } },
            new Part { ToolResponse = new ToolResponse { Id = "call-1", ToolType = ToolType.GoogleSearchWeb } });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        AssertReportsTheInvocationAlone(Assert.Single(result.Messages).Contents);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithAQuerylessStreamedInvocation_ShouldReportTheInvocationAlone()
    {
        // Act
        var result = CreateStreamedResponse(DeserializeChunks(QuerylessStreamedSearchChunks));

        // Assert — the invocation's pair is the whole report; the grounding chunk synthesizes nothing.
        AssertReportsTheInvocationAlone(Assert.Single(result.Messages).Contents);
    }

    private static void AssertReportsTheInvocationAlone(IList<AIContent> contents)
    {
        var call = Assert.Single(contents.OfType<WebSearchToolCallContent>());

        Assert.Equal("call-1", call.CallId);
        Assert.Null(call.Queries);
        Assert.Equal("call-1", Assert.Single(contents.OfType<WebSearchToolResultContent>()).CallId);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithSeveralStreamedSearches_ShouldReportEachInvocationOnce()
    {
        // Act — chunks trimmed from a live gemini-3.1-flash-lite stream: two invocations in one chunk,
        // then webSearchQueries listing both in the other order.
        var result = CreateStreamedResponse(DeserializeChunks(SeveralStreamedSearchesChunks));

        // Assert — each invocation is reported under its own id with its own query, and the cumulative
        // queries synthesize no third call that would misattribute them to one of the two.
        var contents = Assert.Single(result.Messages).Contents;
        var calls = contents.OfType<WebSearchToolCallContent>().OrderBy(call => call.CallId).ToList();
        var results = contents.OfType<WebSearchToolResultContent>().OrderBy(toolResult => toolResult.CallId).ToList();

        Assert.Equal(["call_1795936", "call_1795940"], calls.Select(call => call.CallId));
        Assert.Equal(["current population of Tokyo"], calls[0].Queries);
        Assert.Equal(["current population of Delhi"], calls[1].Queries);
        Assert.Equal(["call_1795936", "call_1795940"], results.Select(toolResult => toolResult.CallId));

        Assert.NotEmpty(GetCitations(result));
    }

    [Fact]
    public void CreateMappedChatResponse_WithAGoogleSearchInvocationAndGrounding_ShouldReportOnePair()
    {
        // Arrange — groundingMetadata repeats the queries the invocation already carries, so reporting
        // both would show one search twice, under two ids.
        var response = CreateGroundedResponse(
            new GroundingMetadata
            {
                WebSearchQueries = ["weather in London"],
                GroundingChunks =
                [
                    new GroundingChunk { Web = new Web { Uri = "https://example.com", Title = "Example" } },
                ],
            },
            new Part
            {
                ToolCall = new ToolCall
                {
                    Id = "call-1",
                    ToolType = ToolType.GoogleSearchWeb,
                    Arguments = JsonSerializer.Deserialize<JsonElement>("""{"queries":["weather in London"]}"""),
                },
            },
            new Part { ToolResponse = new ToolResponse { Id = "call-1", ToolType = ToolType.GoogleSearchWeb } },
            new Part { Text = "It is raining." });

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        var call = Assert.Single(contents.OfType<WebSearchToolCallContent>());

        Assert.Equal("call-1", call.CallId);
        Assert.Equal(["weather in London"], call.Queries);
        Assert.Equal("call-1", Assert.Single(contents.OfType<WebSearchToolResultContent>()).CallId);

        // The citations still land on the grounded text.
        var text = Assert.Single(contents.OfType<TextContent>());
        Assert.Equal("Example", Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!)).Title);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithAStreamedGoogleSearch_ShouldRoundTripOnePair()
    {
        // Arrange
        var chunks = DeserializeChunks(StreamedSearchChunks);
        var expectedParts = chunks
            .SelectMany(chunk => chunk.Candidates![0].Content!.Parts!)
            .Where(part => part.ToolCall is not null || part.ToolResponse is not null)
            .ToList();

        // Act
        var result = CreateStreamedResponse(chunks);

        // Assert — one search, reported once. The assertion is on the parts a next turn would send, not
        // on which content instances survive coalescing.
        var contents = Assert.Single(result.Messages).Contents;
        Assert.Single(contents.OfType<WebSearchToolCallContent>());
        Assert.Single(contents.OfType<WebSearchToolResultContent>());

        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", result.Messages, new ChatOptions());
        var parts = Assert.Single(request.Contents).Parts!;

        Assert.Equal(expectedParts, parts.Where(part => part.ToolCall is not null || part.ToolResponse is not null));
    }

    private const string StreamedSearchChunks =
        """
        [
          {
            "candidates": [
              {
                "content": {
                  "parts": [
                    {
                      "toolCall": {
                        "id": "call-1",
                        "toolType": "GOOGLE_SEARCH_WEB",
                        "args": { "queries": ["weather in London"] }
                      },
                      "thoughtSignature": "call-signature"
                    }
                  ],
                  "role": "model"
                }
              }
            ],
            "responseId": "test-streamed-search"
          },
          {
            "candidates": [
              {
                "content": {
                  "parts": [
                    {
                      "toolResponse": {
                        "id": "call-1",
                        "toolType": "GOOGLE_SEARCH_WEB",
                        "response": { "search_suggestions": "chips" }
                      },
                      "thoughtSignature": "response-signature"
                    }
                  ],
                  "role": "model"
                }
              }
            ],
            "responseId": "test-streamed-search"
          },
          {
            "candidates": [
              {
                "content": { "parts": [{ "text": "It is raining." }], "role": "model" },
                "finishReason": "STOP",
                "groundingMetadata": { "webSearchQueries": ["weather in London"] }
              }
            ],
            "responseId": "test-streamed-search"
          }
        ]
        """;

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string SeveralStreamedSearchesChunks =
        """
        [
          {
            "candidates": [
              {
                "content": {
                  "parts": [
                    {
                      "thoughtSignature": "EvABCu0BCAIS",
                      "toolCall": {
                        "toolType": "GOOGLE_SEARCH_WEB",
                        "args": { "queries": ["current population of Tokyo"] },
                        "id": "call_1795936"
                      }
                    },
                    {
                      "thoughtSignature": "EnwKeggCEnYB",
                      "toolCall": {
                        "toolType": "GOOGLE_SEARCH_WEB",
                        "args": { "queries": ["current population of Delhi"] },
                        "id": "call_1795940"
                      }
                    }
                  ],
                  "role": "model"
                },
                "index": 0
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "XoCYauJay5f-4w_fkL2IBw"
          },
          {
            "candidates": [
              {
                "content": {
                  "parts": [
                    {
                      "thoughtSignature": "EoI7Cv86CAIS",
                      "toolResponse": {
                        "toolType": "GOOGLE_SEARCH_WEB",
                        "response": { "search_suggestions": "<style>.container {}</style>" },
                        "id": "call_1795936"
                      }
                    }
                  ],
                  "role": "model"
                }
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "XoCYauJay5f-4w_fkL2IBw"
          },
          {
            "candidates": [
              {
                "content": {
                  "parts": [
                    {
                      "thoughtSignature": "EvhcCvVcCAIS",
                      "toolResponse": {
                        "toolType": "GOOGLE_SEARCH_WEB",
                        "response": { "search_suggestions": "<style>.container {}</style>" },
                        "id": "call_1795940"
                      }
                    }
                  ],
                  "role": "model"
                }
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "XoCYauJay5f-4w_fkL2IBw"
          },
          {
            "candidates": [
              {
                "content": { "parts": [{ "text": "Tokyo has about 37 million people." }], "role": "model" },
                "index": 0
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "XoCYauJay5f-4w_fkL2IBw"
          },
          {
            "candidates": [
              {
                "content": { "parts": [{ "text": " Delhi has about 33 million." }], "role": "model" },
                "index": 0
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "XoCYauJay5f-4w_fkL2IBw"
          },
          {
            "candidates": [
              {
                "content": { "parts": [{ "text": "" }], "role": "model" },
                "finishReason": "STOP",
                "index": 0,
                "groundingMetadata": {
                  "searchEntryPoint": { "renderedContent": "<style>.container {}</style>" },
                  "groundingChunks": [
                    {
                      "web": {
                        "uri": "https://vertexaisearch.cloud.google.com/grounding-api-redirect/AUZIYQHw",
                        "title": "nippon.com"
                      }
                    },
                    {
                      "web": {
                        "uri": "https://vertexaisearch.cloud.google.com/grounding-api-redirect/AUZIYQEv",
                        "title": "wikipedia.org"
                      }
                    }
                  ],
                  "groundingSupports": [
                    {
                      "segment": { "startIndex": 0, "endIndex": 34, "text": "Tokyo has about 37 million people." },
                      "groundingChunkIndices": [0]
                    },
                    {
                      "segment": { "startIndex": 35, "endIndex": 62, "text": "Delhi has about 33 million." },
                      "groundingChunkIndices": [1]
                    }
                  ],
                  "webSearchQueries": ["current population of Delhi", "current population of Tokyo"]
                }
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "XoCYauJay5f-4w_fkL2IBw"
          }
        ]
        """;

    private const string QuerylessStreamedSearchChunks =
        """
        [
          {
            "candidates": [
              {
                "content": {
                  "parts": [{ "toolCall": { "id": "call-1", "toolType": "GOOGLE_SEARCH_WEB" } }],
                  "role": "model"
                }
              }
            ],
            "responseId": "test-streamed-queryless-search"
          },
          {
            "candidates": [
              {
                "content": {
                  "parts": [{ "toolResponse": { "id": "call-1", "toolType": "GOOGLE_SEARCH_WEB" } }],
                  "role": "model"
                },
                "finishReason": "STOP",
                "groundingMetadata": { "webSearchQueries": ["weather in London"] }
              }
            ],
            "responseId": "test-streamed-queryless-search"
          }
        ]
        """;

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
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, new CandidateMappingState(), DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(ChatRole.Assistant, result.Role);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithModelRole_ShouldMapToAssistantRole()
    {
        // Arrange
        var response = ResponseWithCandidateRole("model");

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, new CandidateMappingState(), DateTimeOffset.UtcNow);

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
    public void CreateMappedChatResponse_WithASegmentStartingAtZero_ShouldAttachARegion()
    {
        // Arrange — Gemini serializes proto3 JSON, so a segment that starts at the beginning of the part
        // arrives with no startIndex at all rather than with a zero.
        var response = JsonSerializer.Deserialize<GenerateContentResponse>(
            """
            {
              "candidates": [
                {
                  "content": { "parts": [{ "text": "Hello world." }], "role": "model" },
                  "groundingMetadata": {
                    "groundingChunks": [{ "web": { "uri": "https://example.com", "title": "Example" } }],
                    "groundingSupports": [
                      { "groundingChunkIndices": [0], "segment": { "endIndex": 5, "text": "Hello" } }
                    ]
                  },
                  "finishReason": "STOP"
                }
              ],
              "responseId": "test-grounding-zero-start"
            }
            """)!;

        // Act
        var result = GeminiToMEAIMapper.CreateMappedChatResponse(response, DateTimeOffset.UtcNow);

        // Assert
        var text = Assert.IsType<TextContent>(Assert.Single(Assert.Single(result.Messages).Contents));
        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(text.Annotations!));
        var region = Assert.IsType<TextSpanAnnotatedRegion>(Assert.Single(citation.AnnotatedRegions!));

        Assert.Equal("Hello", text.Text[region.StartIndex!.Value..region.EndIndex!.Value]);
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
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, new CandidateMappingState(), DateTimeOffset.UtcNow);

        // Assert — the citation carrier and the web search content appear after the text but before
        // the UsageContent.
        var contents = result.Contents;
        Assert.Equal("Search result summary.", Assert.IsType<TextContent>(contents[0]).Text);
        var carrier = Assert.IsType<TextContent>(contents[1]);
        Assert.IsType<WebSearchToolCallContent>(contents[2]);
        Assert.IsType<WebSearchToolResultContent>(contents[3]);
        Assert.IsType<UsageContent>(contents[4]);

        Assert.Equal(string.Empty, carrier.Text);
        var citation = Assert.IsType<CitationAnnotation>(Assert.Single(carrier.Annotations!));
        Assert.Equal("Example", citation.Title);
        Assert.Null(citation.AnnotatedRegions);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithStreamedGrounding_ShouldResolveRegionsAgainstTheWholeStream()
    {
        // Act
        var result = CreateStreamedResponse(DeserializeChunks(StreamedGroundingChunks));

        // Assert — every region indexes the aggregated text, across the chunk boundaries the segments
        // straddle and past the multi-byte characters the byte offsets have to be converted around.
        var citations = GetCitations(result);
        Assert.Equal(3, citations.Count);

        Assert.Equal([FirstGroundedSegment], GetRegionTexts(result, citations[0]));
        Assert.Equal([FirstGroundedSegment, SecondGroundedSegment], GetRegionTexts(result, citations[1]));
        Assert.Null(citations[2].AnnotatedRegions);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithAStreamedThought_ShouldNotIndexTheThoughtText()
    {
        // Arrange — a thought part does not count toward the segment offsets, so a stream that opens with
        // one still resolves to the same spans.
        var chunks = DeserializeChunks(StreamedGroundingChunks);

        chunks.Insert(0, DeserializeChunks(
            """
            [
              {
                "candidates": [
                  {
                    "content": {
                      "parts": [{ "text": "Let me check – carefully.", "thought": true }],
                      "role": "model"
                    }
                  }
                ]
              }
            ]
            """)[0]);

        // Act
        var result = CreateStreamedResponse(chunks);

        // Assert
        var citations = GetCitations(result);
        Assert.Equal([FirstGroundedSegment], GetRegionTexts(result, citations[0]));
        Assert.Equal([FirstGroundedSegment, SecondGroundedSegment], GetRegionTexts(result, citations[1]));
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithAStreamedSegmentNamingAPart_ShouldNotAttachARegion()
    {
        // Arrange — a streamed segment's offsets span the whole stream, so a part index contradicts what
        // the offsets can mean and the region is dropped rather than guessed at, even though 0..5 does
        // lie inside the streamed text.
        var chunks = DeserializeChunks(
            """
            [
              {
                "candidates": [{ "content": { "parts": [{ "text": "Hello world." }], "role": "model" } }],
                "responseId": "test-streamed-grounding-part-index"
              },
              {
                "candidates": [
                  {
                    "content": { "parts": [{ "text": "" }], "role": "model" },
                    "finishReason": "STOP",
                    "groundingMetadata": {
                      "groundingChunks": [{ "web": { "uri": "https://example.com", "title": "Example" } }],
                      "groundingSupports": [
                        {
                          "groundingChunkIndices": [0],
                          "segment": { "partIndex": 1, "endIndex": 5, "text": "Hello" }
                        }
                      ]
                    }
                  }
                ],
                "responseId": "test-streamed-grounding-part-index"
              }
            ]
            """);

        // Act
        var result = CreateStreamedResponse(chunks);

        // Assert
        var citation = Assert.Single(GetCitations(result));
        Assert.Equal("Example", citation.Title);
        Assert.Null(citation.AnnotatedRegions);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithTwoGroundingDeliveries_ShouldEmitOneWebSearchCall()
    {
        // Arrange — the live API delivers grounding metadata once, in the final chunk. Nothing in the wire
        // format promises that, so a later delivery is treated as a repeat and adds nothing.
        var chunks = DeserializeChunks(
            """
            [
              {
                "candidates": [
                  {
                    "content": { "parts": [{ "text": "First." }], "role": "model" },
                    "groundingMetadata": { "webSearchQueries": ["first query"] }
                  }
                ],
                "responseId": "test-streamed-grounding-twice"
              },
              {
                "candidates": [
                  {
                    "content": { "parts": [{ "text": "Second." }], "role": "model" },
                    "finishReason": "STOP",
                    "groundingMetadata": { "webSearchQueries": ["second query"] }
                  }
                ],
                "responseId": "test-streamed-grounding-twice"
              }
            ]
            """);

        // Act
        var result = CreateStreamedResponse(chunks);

        // Assert
        var contents = Assert.Single(result.Messages).Contents;
        var call = Assert.Single(contents.OfType<WebSearchToolCallContent>());
        var toolResult = Assert.Single(contents.OfType<WebSearchToolResultContent>());

        Assert.Equal(["first query"], call.Queries);
        Assert.Equal(call.CallId, toolResult.CallId);
    }

    [Theory]
    [InlineData(IdLessStreamedCodeExecutionChunks)]
    [InlineData(IdLessStreamedToolInvocationChunks)]
    public void CreateMappedChatResponseUpdate_WithAnIdLessCallAndResult_ShouldCorrelateAcrossChunks(string chunksJson)
    {
        // Act
        var result = CreateStreamedResponse(DeserializeChunks(chunksJson));

        // Assert — the call and the result Gemini split across two chunks share the id the mapper minted.
        var contents = Assert.Single(result.Messages).Contents;
        var callIds = contents.Select(GetCallId).Where(callId => callId is not null).ToList();

        Assert.Equal(2, callIds.Count);
        Assert.Equal(callIds[0], callIds[1]);
    }

    [Fact]
    public void CreateMappedChatResponseUpdate_WithASecondState_ShouldMintDifferentCallIds()
    {
        // Arrange — the correlation state belongs to one stream, so a second stream must not answer the
        // first stream's unanswered calls or reuse its ids.
        var chunks = DeserializeChunks(IdLessStreamedCodeExecutionChunks);

        // Act
        var first = CreateStreamedResponse(chunks);
        var second = CreateStreamedResponse(chunks);

        // Assert
        Assert.NotEqual(GetSingleCallId(first), GetSingleCallId(second));

        static string GetSingleCallId(ChatResponse response)
        {
            return Assert.Single(response.Messages[0].Contents.OfType<CodeInterpreterToolCallContent>()).CallId;
        }
    }

    private static string? GetCallId(AIContent content)
    {
        return content switch
        {
            CodeInterpreterToolCallContent call => call.CallId,
            CodeInterpreterToolResultContent toolResult => toolResult.CallId,
            ToolCallContent call => call.CallId,
            ToolResultContent toolResult => toolResult.CallId,
            _ => null,
        };
    }

    private static List<GenerateContentResponse> DeserializeChunks(string chunksJson)
    {
        return JsonSerializer.Deserialize<List<GenerateContentResponse>>(chunksJson)!;
    }

    private static ChatResponse CreateStreamedResponse(List<GenerateContentResponse> chunks)
    {
        var state = new CandidateMappingState();

        return chunks
            .Select(chunk => GeminiToMEAIMapper.CreateMappedChatResponseUpdate(chunk, state, DateTimeOffset.UtcNow))
            .ToChatResponse();
    }

    private static List<CitationAnnotation> GetCitations(ChatResponse response)
    {
        return Assert.Single(response.Messages).Contents
            .SelectMany(content => content.Annotations ?? [])
            .OfType<CitationAnnotation>()
            .ToList();
    }

    /// <summary>
    /// The text each of a citation's regions selects out of the aggregated response, which is what a
    /// streamed region indexes.
    /// </summary>
    private static List<string> GetRegionTexts(ChatResponse response, CitationAnnotation citation)
    {
        return citation.AnnotatedRegions!
            .Cast<TextSpanAnnotatedRegion>()
            .Select(region => response.Text[region.StartIndex!.Value..region.EndIndex!.Value])
            .ToList();
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
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, new CandidateMappingState(), DateTimeOffset.UtcNow);

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
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, new CandidateMappingState(), DateTimeOffset.UtcNow);

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
        var result = GeminiToMEAIMapper.CreateMappedChatResponseUpdate(response, new CandidateMappingState(), DateTimeOffset.UtcNow);

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


    /// <summary>
    /// The first two spans <see cref="StreamedGroundingChunks"/> grounds, as Gemini reported them. Both
    /// straddle a chunk boundary, and the second lies past a multi-byte character, so the byte offsets and
    /// the character indices differ.
    /// </summary>
    private const string FirstGroundedSegment =
        "Spain is the winner of the most recent FIFA World Cup, securing their second title in the 2026 tournament";

    /// <inheritdoc cref="FirstGroundedSegment"/>
    private const string SecondGroundedSegment =
        "They claimed the championship by defeating Argentina with a score of 1–0 after extra time in the final match";

    /// <summary>
    /// A recorded Google Search stream (2026-09-02, <c>gemini-3.1-flash-lite</c>), trimmed to its first
    /// four text chunks and its final chunk. The grounding metadata arrives once, with the finish reason,
    /// and its segment offsets index every text chunk of the stream at once. Chunk 2 is cited by no
    /// support.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string StreamedGroundingChunks =
        """
        [
          {
            "candidates": [{ "content": { "parts": [{ "text": "Spain is" }], "role": "model" } }],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "test-streamed-grounding"
          },
          {
            "candidates": [
              {
                "content": {
                  "parts": [{ "text": " the winner of the most recent FIFA World Cup, securing their second title in the" }],
                  "role": "model"
                }
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "test-streamed-grounding"
          },
          {
            "candidates": [
              {
                "content": {
                  "parts": [{ "text": " 2026 tournament. They claimed the championship by defeating Argentina with" }],
                  "role": "model"
                }
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "test-streamed-grounding"
          },
          {
            "candidates": [
              {
                "content": {
                  "parts": [{ "text": " a score of 1–0 after extra time in the final match.\n\n" }],
                  "role": "model"
                }
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "test-streamed-grounding"
          },
          {
            "candidates": [
              {
                "content": { "parts": [{ "text": "", "thoughtSignature": "signature" }], "role": "model" },
                "finishReason": "STOP",
                "groundingMetadata": {
                  "groundingChunks": [
                    { "web": { "uri": "https://vertexaisearch.cloud.google.com/grounding-api-redirect/AUZIYQEdM8RD5ss3G440", "title": "wikipedia.org" } },
                    { "web": { "uri": "https://vertexaisearch.cloud.google.com/grounding-api-redirect/AUZIYQFAX7IvE3SrxHGy", "title": "wikipedia.org" } },
                    { "web": { "uri": "https://vertexaisearch.cloud.google.com/grounding-api-redirect/AUZIYQF1mRH2iNoVrOwX", "title": "topendsports.com" } }
                  ],
                  "groundingSupports": [
                    {
                      "groundingChunkIndices": [0, 1],
                      "segment": {
                        "endIndex": 105,
                        "text": "Spain is the winner of the most recent FIFA World Cup, securing their second title in the 2026 tournament"
                      }
                    },
                    {
                      "groundingChunkIndices": [1],
                      "segment": {
                        "startIndex": 107,
                        "endIndex": 217,
                        "text": "They claimed the championship by defeating Argentina with a score of 1–0 after extra time in the final match"
                      }
                    }
                  ],
                  "webSearchQueries": ["who won the most recent Super Bowl", "who won the most recent FIFA World Cup"]
                }
              }
            ],
            "modelVersion": "gemini-3.1-flash-lite",
            "responseId": "test-streamed-grounding"
          }
        ]
        """;

    /// <summary>
    /// A streamed code execution with the ids Gemini normally sends omitted, so that the call and the
    /// result can only be correlated by the order they arrived in.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string IdLessStreamedCodeExecutionChunks =
        """
        [
          {
            "candidates": [
              {
                "content": {
                  "parts": [{ "executableCode": { "language": "PYTHON", "code": "print(sum(range(1, 11)))" } }],
                  "role": "model"
                }
              }
            ],
            "responseId": "test-streamed-id-less-code-execution"
          },
          {
            "candidates": [
              {
                "content": {
                  "parts": [{ "codeExecutionResult": { "outcome": "OUTCOME_OK", "output": "55\n" } }],
                  "role": "model"
                },
                "finishReason": "STOP"
              }
            ],
            "responseId": "test-streamed-id-less-code-execution"
          }
        ]
        """;

    /// <inheritdoc cref="IdLessStreamedCodeExecutionChunks"/>
    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string IdLessStreamedToolInvocationChunks =
        """
        [
          {
            "candidates": [
              {
                "content": {
                  "parts": [{ "toolCall": { "toolType": "GOOGLE_SEARCH_WEB", "args": { "queries": ["a query"] } } }],
                  "role": "model"
                }
              }
            ],
            "responseId": "test-streamed-id-less-tool-invocation"
          },
          {
            "candidates": [
              {
                "content": {
                  "parts": [{ "toolResponse": { "toolType": "GOOGLE_SEARCH_WEB", "response": { "search_suggestions": "chips" } } }],
                  "role": "model"
                },
                "finishReason": "STOP"
              }
            ],
            "responseId": "test-streamed-id-less-tool-invocation"
          }
        ]
        """;

    #endregion
}
