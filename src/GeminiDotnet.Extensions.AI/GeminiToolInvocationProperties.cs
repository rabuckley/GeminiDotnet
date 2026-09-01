using GeminiDotnet.V1Beta;
using Microsoft.Extensions.AI;
using System.Text.Json;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Keys for the fields of a Gemini server-side tool invocation that <see cref="ToolCallContent"/> and
/// <see cref="ToolResultContent"/> have no property for, carried in
/// <see cref="AIContent.AdditionalProperties"/>.
/// </summary>
/// <remarks>
/// <para>
/// Gemini returns a <see cref="Part.ToolCall"/> for each built-in tool it ran and a
/// <see cref="Part.ToolResponse"/> holding that tool's output, and requires both to be sent back
/// unchanged on every later turn. The typed <see cref="Part"/> is on
/// <see cref="AIContent.RawRepresentation"/> and is echoed verbatim when it survives; these entries
/// rebuild the part when it does not, as after a round trip through JSON.
/// </para>
/// <para>
/// Each value may also arrive as a <see cref="JsonElement"/> holding it, as a host that round-trips its
/// history through JSON delivers. A value of any other type throws a
/// <see cref="GeminiMappingException"/> rather than being dropped, because a turn that silently loses a
/// tool invocation still succeeds, with the model no longer knowing what it already looked up.
/// </para>
/// </remarks>
public static class GeminiToolInvocationProperties
{
    /// <summary>
    /// Key for the identifier Gemini issued for the invocation, as a <see cref="string"/>. Read from
    /// both <see cref="ToolCallContent"/> and <see cref="ToolResultContent"/>, and present only when
    /// Gemini issued one. <see cref="ToolCallContent.CallId"/> is not a substitute: it always holds a
    /// value, synthesized to correlate the pair when Gemini issued none, and a synthesized identifier
    /// echoed back is one the server never handed out.
    /// </summary>
    public const string Id = "id";

    /// <summary>
    /// Key for the kind of tool that was invoked, as a <see cref="V1Beta.ToolType"/>. Read from both
    /// <see cref="ToolCallContent"/> and <see cref="ToolResultContent"/>, and required on both: Gemini
    /// rejects an invocation echoed back without it.
    /// </summary>
    public const string ToolType = "toolType";

    /// <summary>
    /// Key for the name of the tool that was invoked, as a <see cref="string"/>. Read from
    /// <see cref="ToolCallContent"/>, and present only when Gemini reported one.
    /// </summary>
    public const string ToolName = "toolName";

    /// <summary>
    /// Key for the arguments the tool was invoked with, as a <see cref="JsonElement"/>. Read from
    /// <see cref="ToolCallContent"/>, and present only when Gemini reported any.
    /// </summary>
    public const string Arguments = "args";

    /// <summary>
    /// Key for the output the tool produced, as a <see cref="JsonElement"/>. Read from
    /// <see cref="ToolResultContent"/>, and present only when Gemini reported one.
    /// </summary>
    public const string Response = "response";

    /// <summary>
    /// Key for the opaque signature of the thought that led to the invocation, as a
    /// <see cref="string"/>. Read from both <see cref="ToolCallContent"/> and
    /// <see cref="ToolResultContent"/>, and present only when Gemini reported one.
    /// </summary>
    public const string ThoughtSignature = "thoughtSignature";
}
