using Microsoft.Extensions.AI;
using System.Text;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Tracks tool calls, citation text and usage across a candidate's chunks.
/// </summary>
/// <remarks>
/// In the streamed grounding fixture, Gemini sends metadata with the final chunk and omits
/// <see cref="V1.Segment.PartIndex"/>. Its UTF-8 byte offsets index the joined non-thought text.
/// See <see cref="GeminiChatClient.GetStreamingResponseAsync"/> for how those offsets map to
/// <see cref="TextSpanAnnotatedRegion"/>.
/// </remarks>
internal sealed class CandidateMappingState
{
    public ServerToolCallIds CodeExecutionCallIds { get; } = new();

    private readonly Dictionary<V1.ToolType, ServerToolCallIds> _toolCallIds = [];

    public ServerToolCallIds ToolCallIds(V1.ToolType toolType)
    {
        if (!_toolCallIds.TryGetValue(toolType, out var callIds))
        {
            callIds = new ServerToolCallIds();
            _toolCallIds.Add(toolType, callIds);
        }

        return callIds;
    }

    public StringBuilder Text { get; } = new();

    public bool HasReportedWebSearch { get; set; }

    public V1.UsageMetadata? LatestUsage { get; set; }

    public UsageDetails ReportedUsage { get; } = new();
}
