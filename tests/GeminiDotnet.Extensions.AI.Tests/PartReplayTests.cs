using GeminiDotnet.V1Beta;
using Microsoft.Extensions.AI;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GeminiDotnet.Extensions.AI;

// Part is generated, so a new field can appear in a regeneration without either mapper handling it.
// The classification test fails until the field is placed in ReplayedSamples or NotReplayed.
public sealed class PartReplayTests
{
    // One model part per replayed property, each setting that property to a non-default value.
    private static readonly Dictionary<string, Part> ReplayedSamples = new()
    {
        [nameof(Part.Text)] = new Part { Text = "Hello." },
        [nameof(Part.InlineData)] = new Part
        {
            InlineData = new Blob { Data = new byte[] { 1, 2, 3 }, MimeType = "image/png" },
        },
        [nameof(Part.FileData)] = new Part
        {
            FileData = new FileData { FileUri = "https://example.com/video.mp4", MimeType = "video/mp4" },
        },
        [nameof(Part.FunctionCall)] = new Part
        {
            FunctionCall = new FunctionCall
            {
                Id = "call-1",
                Name = "get_weather",
                Arguments = JsonDocument.Parse("""{"city":"Paris"}""").RootElement,
            },
        },
        [nameof(Part.ToolCall)] = new Part
        {
            ToolCall = new ToolCall
            {
                Id = "tool-1",
                ToolType = ToolType.GoogleSearchWeb,
                Arguments = JsonDocument.Parse("""{"queries":["weather paris"]}""").RootElement,
            },
        },
        [nameof(Part.ToolResponse)] = new Part
        {
            ToolResponse = new ToolResponse
            {
                Id = "tool-1",
                ToolType = ToolType.GoogleSearchWeb,
                Response = JsonDocument.Parse("""{"results":[]}""").RootElement,
            },
        },
        [nameof(Part.ExecutableCode)] = new Part
        {
            ExecutableCode = new ExecutableCode
            {
                Id = "code-1",
                Language = ExecutableCodeLanguage.Python,
                Code = "print(1 + 1)",
            },
        },
        [nameof(Part.CodeExecutionResult)] = new Part
        {
            CodeExecutionResult = new CodeExecutionResult
            {
                Id = "code-1",
                Outcome = CodeExecutionResultOutcome.Ok,
                Output = "2",
            },
        },
        // On an image rather than text: reasoning text always carried the flag, other content did not.
        [nameof(Part.Thought)] = new Part
        {
            Thought = true,
            InlineData = new Blob { Data = new byte[] { 1 }, MimeType = "image/png" },
        },
        [nameof(Part.ThoughtSignature)] = new Part
        {
            ThoughtSignature = "signature",
            FunctionCall = new FunctionCall
            {
                Id = "call-1",
                Name = "get_time",
                Arguments = JsonDocument.Parse("{}").RootElement,
            },
        },
    };

    // Each reason records a decision; moving a property to ReplayedSamples is the fix for a gap.
    private static readonly Dictionary<string, string> NotReplayed = new()
    {
        [nameof(Part.AudioTranscription)] = "Output only; Gemini does not accept it in a request.",
        [nameof(Part.FunctionResponse)] = "Never in model output; the client sends it in a user turn.",
        [nameof(Part.PartMetadata)] = "Gap: kept only on RawRepresentation and never sent back.",
        [nameof(Part.MediaProcessing)] = "Request-only setting with no M.E.AI source.",
        [nameof(Part.MediaResolution)] = "Request-only setting with no M.E.AI source.",
        [nameof(Part.SpeechMetadata)] = "Request-only setting with no M.E.AI source.",
        [nameof(Part.VideoMetadata)] = "Request-only setting with no M.E.AI source.",
    };

    public static TheoryData<string, bool> ReplayedProperties()
    {
        var data = new TheoryData<string, bool>();
        foreach (var property in ReplayedSamples.Keys)
        {
            data.Add(property, false);
            data.Add(property, true);
        }

        return data;
    }

    [Fact]
    public void EveryPartProperty_ShouldBeClassifiedAsReplayedOrNotReplayed()
    {
        // Arrange
        var properties = typeof(Part).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(property => property.Name)
            .Order()
            .ToList();

        // Act
        var classified = ReplayedSamples.Keys.Concat(NotReplayed.Keys).Order().ToList();

        // Assert
        Assert.Empty(ReplayedSamples.Keys.Intersect(NotReplayed.Keys));
        Assert.Equal(properties, classified);
    }

    [Theory]
    [MemberData(nameof(ReplayedProperties))]
    public void CreateMappedGenerateContentRequest_WithAModelPartInTheHistory_ShouldSendTheFieldBack(
        string propertyName,
        bool persistAsJson)
    {
        // Arrange
        var original = ReplayedSamples[propertyName];
        var response = new GenerateContentResponse
        {
            Candidates = [new Candidate { Content = new Content { Role = "model", Parts = [original] } }],
        };

        IList<ChatMessage> messages = GeminiToMEAIMapper
            .CreateMappedChatResponse(response, DateTimeOffset.UtcNow)
            .Messages;

        if (persistAsJson)
        {
            var json = JsonSerializer.Serialize(messages, GeminiJsonUtilities.DefaultOptions);
            messages = JsonSerializer.Deserialize<List<ChatMessage>>(json, GeminiJsonUtilities.DefaultOptions)!;
        }

        // Act
        var request = MEAIToGeminiMapper.CreateMappedGenerateContentRequest("", messages, new ChatOptions());

        // Assert
        var replayed = Assert.Single(Assert.Single(request.Contents!).Parts!);
        var jsonName = typeof(Part).GetProperty(propertyName)!.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name;
        var expected = JsonSerializer.SerializeToNode(original)![jsonName];
        var actual = JsonSerializer.SerializeToNode(replayed)![jsonName];
        Assert.True(
            JsonNode.DeepEquals(expected, actual),
            $"Sent {actual?.ToJsonString() ?? "nothing"} for {propertyName}, expected {expected!.ToJsonString()}.");
    }
}
