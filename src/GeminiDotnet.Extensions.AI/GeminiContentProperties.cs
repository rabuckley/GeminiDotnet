using GeminiDotnet.V1;
using Microsoft.Extensions.AI;
using System.Text.Json;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Keys for Gemini <see cref="Part"/> metadata stored in <see cref="AIContent.AdditionalProperties"/>
/// or <see cref="AIAnnotation.AdditionalProperties"/>, as specified by each member.
/// </summary>
/// <remarks>
/// <para>
/// Preserve these entries when saving chat history. Gemini requires server-side tool calls and
/// results on later turns. The adapter reuses the <see cref="Part"/> in
/// <see cref="AIContent.RawRepresentation"/> when available and rebuilds it from these entries
/// after deserialization. A recorded <see cref="ThoughtSignature"/> overrides the raw part's signature.
/// </para>
/// <para>
/// Use <see cref="AdditionalPropertiesDictionaryExtensions.TryGetGeminiValue{T}"/> to read both typed
/// values and deserialized <see cref="JsonElement"/> values. An invalid value can throw
/// <see cref="GeminiMappingException"/> during replay.
/// </para>
/// <para>
/// Under Native AOT, use <see cref="GeminiJsonUtilities.DefaultOptions"/> to serialize history
/// containing the <see cref="V1"/> enum values under <see cref="ToolType"/> or <see cref="Outcome"/>.
/// </para>
/// <para>
/// Keep <see cref="ToolType"/> on stored web search calls and results. The adapter skips web search
/// content without this key, including content it creates from <see cref="V1.GroundingMetadata"/>.
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
    /// from every <see cref="AIContent"/> and present only when Gemini reported one. Live responses put
    /// the signature of a code execution on the <see cref="Part.ExecutableCode"/> part, so it is normally
    /// the call content that carries it.
    /// </summary>
    /// <remarks>
    /// A mapped <see cref="TextContent"/> carries its signature under this key in the
    /// <see cref="AIAnnotation.AdditionalProperties"/> of a plain <see cref="AIAnnotation"/>, never a
    /// <see cref="CitationAnnotation"/>. A consumer reading citations should match on that type rather
    /// than on <see cref="AIAnnotation.AnnotatedRegions"/>, which a citation can also lack. A mapped
    /// <see cref="TextReasoningContent"/> carries its signature in
    /// <see cref="TextReasoningContent.ProtectedData"/>. On both types that slot is read first, and this
    /// key in the content's own <see cref="AIContent.AdditionalProperties"/> only when the slot holds
    /// none.
    /// </remarks>
    public const string ThoughtSignature = "thoughtSignature";

    /// <summary>
    /// Key for whether the part is a thought, as a <see cref="bool"/>. Read from every
    /// <see cref="AIContent"/> except <see cref="TextReasoningContent"/>, which is always a thought, and
    /// <see cref="FunctionCallContent"/>, which reports it as
    /// <see cref="FunctionCallContent.InformationalOnly"/>. Present only when Gemini marked the part as a
    /// thought; a value of <see langword="false"/> is the same as no value.
    /// </summary>
    /// <remarks>
    /// Preserve this property when replaying conversation history to Gemini.
    /// </remarks>
    public const string Thought = "thought";

    /// <summary>
    /// A <see cref="V1.ToolType"/> required on web search calls and results for replay, and on
    /// other server-side tool calls and results when rebuilding them without the raw <see cref="Part"/>.
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
    /// <see cref="AdditionalPropertiesDictionary{TValue}.TryGetValue{T}"/> reports as absent. Read it with
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
