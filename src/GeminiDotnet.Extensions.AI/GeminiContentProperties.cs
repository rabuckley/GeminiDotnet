using GeminiDotnet.V1Beta;
using Microsoft.Extensions.AI;
using System.Text.Json;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Keys for what a Gemini <see cref="Part"/> says that the mapped <see cref="AIContent"/> has no property
/// for, carried in <see cref="AIContent.AdditionalProperties"/> or, where a member says so, in
/// <see cref="AIAnnotation.AdditionalProperties"/> on an annotation over the content.
/// </summary>
/// <remarks>
/// <para>
/// Gemini requires the parts it generated for a server-side tool run, a <see cref="Part.ToolCall"/> with
/// its <see cref="Part.ToolResponse"/> or a <see cref="Part.ExecutableCode"/> with its
/// <see cref="Part.CodeExecutionResult"/>, to be sent back unchanged on every later turn. The typed
/// <see cref="Part"/> on <see cref="AIContent.RawRepresentation"/> supplies the tool fields when it
/// survives; these entries rebuild the part when it does not, as after a round trip through JSON.
/// A recorded <see cref="ThoughtSignature"/> takes precedence over the raw part's signature.
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
    /// Key for the transcription Gemini reported alongside the part, as a
    /// <see cref="V1Beta.AudioTranscription"/>, carried in
    /// <see cref="AIAnnotation.AdditionalProperties"/> on an annotation over the content. It holds the
    /// speaker label when diarization was asked for, and the word timings when word timestamps were.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The annotation covers the transcribed span only where the content's text is the transcript, which
    /// is the part that carried nothing but its transcription. A part that carried text of its own is
    /// annotated without a region, because none of its text is the transcript.
    /// </para>
    /// <para>
    /// The annotation's <see cref="AIAnnotation.RawRepresentation"/> holds the whole <see cref="Part"/>,
    /// so the typed <see cref="V1Beta.AudioTranscription"/> is reachable through it as well; this entry
    /// exists because that property is dropped when a response is serialized.
    /// <see cref="Part.AudioTranscription"/> is output only, so a transcription is never sent back to
    /// Gemini.
    /// </para>
    /// </remarks>
    /// <example>
    /// Each speaker turn is its own content, so one loop over the aggregated response reaches every
    /// segment with the label that belongs to it:
    /// <code>
    /// List&lt;ChatResponseUpdate&gt; updates = [];
    ///
    /// await foreach (var update in client.GetStreamingResponseAsync(messages, options))
    /// {
    ///     updates.Add(update);
    /// }
    ///
    /// foreach (var content in updates.ToChatResponse().Messages.SelectMany(message =&gt; message.Contents))
    /// {
    ///     if (content is not TextContent text)
    ///     {
    ///         continue;
    ///     }
    ///
    ///     // A grounded content carries citation annotations too, so pick the annotation by its key. A
    ///     // region says the content's text is the transcript, so only then is it the speaker's words.
    ///     foreach (var annotation in text.Annotations ?? [])
    ///     {
    ///         if (annotation.AnnotatedRegions is not null
    ///             &amp;&amp; annotation.AdditionalProperties?.TryGetGeminiValue(
    ///                 GeminiContentProperties.AudioTranscription, out AudioTranscription? transcription) is true)
    ///         {
    ///             Console.WriteLine($"{transcription.SpeakerLabel}: {text.Text}");
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public const string AudioTranscription = "audioTranscription";

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
