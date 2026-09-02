using GeminiDotnet.V1Beta;
using Microsoft.Extensions.AI;
using System.Text.Json;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Keys for the fields of a Gemini <see cref="Part"/> that the mapped <see cref="AIContent"/> has no
/// property for, carried in <see cref="AIContent.AdditionalProperties"/>.
/// </summary>
/// <remarks>
/// <para>
/// Gemini requires the parts it generated for a server-side tool run, a <see cref="Part.ToolCall"/> with
/// its <see cref="Part.ToolResponse"/> or a <see cref="Part.ExecutableCode"/> with its
/// <see cref="Part.CodeExecutionResult"/>, to be sent back unchanged on every later turn. The typed
/// <see cref="Part"/> is on <see cref="AIContent.RawRepresentation"/> and is echoed verbatim when it
/// survives; these entries rebuild the part when it does not, as after a round trip through JSON.
/// </para>
/// <para>
/// After such a round trip each value arrives as a <see cref="JsonElement"/> holding it. Read it with
/// <see cref="AdditionalPropertiesDictionaryExtensions.TryGetGeminiValue{T}"/>, which handles both forms.
/// A value of any other type throws a <see cref="GeminiMappingException"/> when the part is rebuilt rather
/// than being dropped, because a turn that silently loses a tool run still succeeds, with the model no
/// longer knowing what it already looked up or computed.
/// </para>
/// <para>
/// <see cref="ToolType"/> and <see cref="Outcome"/> hold <see cref="V1Beta"/> enum values. In a
/// reflection-enabled application they serialize under any <see cref="JsonSerializerOptions"/>, because
/// the enum types carry their own <see cref="System.Text.Json.Serialization.JsonConverterAttribute"/>.
/// Under Native AOT the history must be persisted with <see cref="GeminiJsonUtilities.DefaultOptions"/>,
/// whose resolver knows the <see cref="V1Beta"/> types; the options of
/// <see cref="AIJsonUtilities.DefaultOptions"/> alone cannot write them.
/// </para>
/// <para>
/// These entries also tell the two kinds of web search content apart. A
/// <see cref="WebSearchToolCallContent"/> or <see cref="WebSearchToolResultContent"/> carrying them is a
/// Google Search invocation Gemini reported, and is echoed back on the next turn; one without them was
/// synthesized from <see cref="V1Beta.GroundingMetadata"/>, has no part behind it, and is dropped. A
/// pipeline that strips <see cref="AIContent.AdditionalProperties"/>, or a history the caller rebuilds
/// from its own store without them, therefore loses the search from the next turn without an error.
/// </para>
/// </remarks>
public static class GeminiContentProperties
{
    /// <summary>
    /// Key for the identifier Gemini issued for the part, as a <see cref="string"/>. Read from
    /// <see cref="ToolCallContent"/>, <see cref="ToolResultContent"/>,
    /// <see cref="WebSearchToolCallContent"/>, <see cref="WebSearchToolResultContent"/>,
    /// <see cref="CodeInterpreterToolCallContent"/> and <see cref="CodeInterpreterToolResultContent"/>,
    /// and present only when Gemini issued one. <see cref="ToolCallContent.CallId"/> is not a substitute:
    /// it always holds a value, synthesized to correlate the pair when Gemini issued none, and a
    /// synthesized identifier echoed back is one the server never handed out.
    /// </summary>
    public const string Id = "id";

    /// <summary>
    /// Key for the opaque signature of the thought that led to the part, as a <see cref="string"/>. Read
    /// from <see cref="ToolCallContent"/>, <see cref="ToolResultContent"/>,
    /// <see cref="WebSearchToolCallContent"/>, <see cref="WebSearchToolResultContent"/>,
    /// <see cref="CodeInterpreterToolCallContent"/> and <see cref="CodeInterpreterToolResultContent"/>,
    /// and present only when Gemini reported one. Live responses put the signature of a code execution on
    /// the <see cref="Part.ExecutableCode"/> part, so it is normally the call content that carries it.
    /// </summary>
    public const string ThoughtSignature = "thoughtSignature";

    /// <summary>
    /// Key for the kind of tool that was invoked, as a <see cref="V1Beta.ToolType"/>. Read from
    /// <see cref="ToolCallContent"/>, <see cref="ToolResultContent"/>,
    /// <see cref="WebSearchToolCallContent"/> and <see cref="WebSearchToolResultContent"/>, and required
    /// on all four: Gemini rejects an invocation echoed back without it. See the remarks on this class
    /// for the serialization requirement under Native AOT.
    /// </summary>
    public const string ToolType = "toolType";

    /// <summary>
    /// Key for the name of the tool that was invoked, as a <see cref="string"/>. Read from
    /// <see cref="ToolCallContent"/> and <see cref="WebSearchToolCallContent"/>, and present only when
    /// Gemini reported one.
    /// </summary>
    public const string ToolName = "toolName";

    /// <summary>
    /// Key for the arguments the tool was invoked with, as a <see cref="JsonElement"/>. Read from
    /// <see cref="ToolCallContent"/> and <see cref="WebSearchToolCallContent"/>, and present only when
    /// Gemini reported any.
    /// </summary>
    /// <remarks>
    /// <see cref="WebSearchToolCallContent.Queries"/> holds the strings of this value's <c>queries</c>
    /// array, and is <see langword="null"/> when Gemini reported no such array.
    /// </remarks>
    public const string Arguments = "args";

    /// <summary>
    /// Key for the output the tool produced, as a <see cref="JsonElement"/>. Read from
    /// <see cref="ToolResultContent"/> and <see cref="WebSearchToolResultContent"/>, and present only when
    /// Gemini reported one. <see cref="WebSearchToolResultContent.Outputs"/> is always
    /// <see langword="null"/>: the sources behind a search live on the citation annotations.
    /// </summary>
    public const string Response = "response";

    /// <summary>
    /// Key for the outcome of a code execution, as a <see cref="CodeExecutionResultOutcome"/>. Read from
    /// <see cref="CodeInterpreterToolResultContent"/>, and present only when Gemini reported one other
    /// than <see cref="CodeExecutionResultOutcome.Unspecified"/>. Optional when the part is rebuilt: an
    /// absent outcome maps to <see cref="CodeExecutionResultOutcome.Unspecified"/>, which Gemini accepts.
    /// See the remarks on this class for the serialization requirement under Native AOT.
    /// </summary>
    /// <example>
    /// After a round trip through JSON the value is a <see cref="JsonElement"/>, which
    /// <see cref="AdditionalPropertiesDictionary.TryGetValue{T}"/> reports as absent. Read it with
    /// <see cref="AdditionalPropertiesDictionaryExtensions.TryGetGeminiValue{T}"/>, which handles both
    /// forms:
    /// <code>
    /// if (result.AdditionalProperties?.TryGetGeminiValue(GeminiContentProperties.Outcome,
    ///         out CodeExecutionResultOutcome outcome) is true
    ///     &amp;&amp; outcome is CodeExecutionResultOutcome.Failed)
    /// {
    ///     // The code raised; result.Outputs holds stderr.
    /// }
    /// </code>
    /// </example>
    public const string Outcome = "outcome";
}
