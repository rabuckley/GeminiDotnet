using Microsoft.Extensions.AI;
using System.Text;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// What the earlier parts of a candidate leave behind for its later parts to refer to.
/// </summary>
/// <remarks>
/// <para>
/// One instance covers one candidate: <see cref="GeminiChatClient.GetStreamingResponseAsync"/> creates a
/// single instance for the whole stream, and <see cref="GeminiToMEAIMapper.CreateMappedChatResponse"/> a
/// fresh one per candidate, so a streamed and a whole response run the same mapping code.
/// </para>
/// <para>
/// Streamed grounding was probed on 2026-09-02 against <c>gemini-3.1-flash-lite</c> and
/// <c>gemini-3.1-pro-preview</c>. <c>groundingMetadata</c> arrives once, in the chunk carrying
/// <c>finishReason: STOP</c>, with every chunk, support and query at once — so there is nothing to
/// accumulate for the chunks themselves. What has to be carried is the text the supports index: a
/// streamed <see cref="V1Beta.Segment"/> gives UTF-8 byte offsets into the concatenation of every
/// non-thought text part of the whole stream, with <see cref="V1Beta.Segment.PartIndex"/> absent.
/// Thought parts and tool invocation parts do not count toward those offsets. The same probe showed Gemini
/// running several <c>GOOGLE_SEARCH_WEB</c> invocations in one turn, as two <c>toolCall</c> parts in one
/// chunk or as one call/response pair after another, and that
/// <see cref="V1Beta.GroundingMetadata.WebSearchQueries"/> is cumulative across every search of the turn
/// and not in invocation order.
/// </para>
/// <para>
/// A <see cref="TextSpanAnnotatedRegion"/> therefore means two things. On a whole response it sits on the
/// grounded <see cref="TextContent"/> and indexes that content's own text, as the Gemini spec defines the
/// offsets. On a streamed update it sits on an empty carrier <see cref="TextContent"/> and indexes the
/// stream's whole text, which is <see cref="ChatMessage.Text"/> once the updates are aggregated.
/// </para>
/// </remarks>
internal sealed class CandidateMappingState
{
    public ServerToolCallIds CodeExecutionCallIds { get; } = new();

    private readonly Dictionary<V1Beta.ToolType, ServerToolCallIds> _toolCallIds = [];

    public ServerToolCallIds ToolCallIds(V1Beta.ToolType toolType)
    {
        if (!_toolCallIds.TryGetValue(toolType, out var callIds))
        {
            callIds = new ServerToolCallIds();
            _toolCallIds.Add(toolType, callIds);
        }

        return callIds;
    }

    /// <summary>
    /// The text a grounding segment's byte offsets index: every non-thought <see cref="V1Beta.Part.Text"/>
    /// mapped so far, joined.
    /// </summary>
    /// <remarks>
    /// A builder rather than a string, so that the whole text is copied once per grounding delivery instead
    /// of once per support.
    /// </remarks>
    public StringBuilder Text { get; } = new();

    /// <summary>
    /// Whether a web search has been reported for this candidate.
    /// </summary>
    /// <remarks>
    /// Set when a <c>GOOGLE_SEARCH_WEB</c> <see cref="V1Beta.Part.ToolCall"/> is mapped, and when a pair is
    /// synthesized from <see cref="V1Beta.GroundingMetadata.WebSearchQueries"/>.
    /// <see cref="GeminiToMEAIMapper"/> reads it to synthesize at most one pair per candidate.
    /// </remarks>
    public bool HasReportedWebSearch { get; set; }

    public V1Beta.UsageMetadata? LatestUsage { get; set; }

    public UsageDetails ReportedUsage { get; } = new();
}
