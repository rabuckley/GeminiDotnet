using Microsoft.Extensions.AI;
using System.Text.Json;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Defines keys for Gemini-specific properties carried in Microsoft.Extensions.AI additional-property
/// dictionaries.
/// </summary>
public static class GeminiAdditionalProperties
{
    /// <summary>
    /// Key for the <see cref="ThinkingConfiguration"/> property that configures thinking/reasoning behavior.
    /// Read from <see cref="ChatOptions.AdditionalProperties"/>.
    /// </summary>
    [Obsolete("Use Microsoft.Extensions.AI.ReasoningEffort")]
    public const string ThinkingConfiguration = "thinkingConfig";

    /// <summary>
    /// Key for the response modalities property that specifies the desired output types (e.g., text, image).
    /// Read from <see cref="ChatOptions.AdditionalProperties"/>.
    /// </summary>
    public const string ResponseModalities = "responseModalities";

    /// <summary>
    /// Key for the <see cref="V1Beta.ImageConfiguration"/> property that configures image generation settings.
    /// Read from <see cref="ChatOptions.AdditionalProperties"/>.
    /// </summary>
    public const string ImageConfiguration = "imageConfig";

    /// <summary>
    /// Key for the metadata filter applied to the documents and chunks a file search retrieves, using the
    /// filter syntax described by https://google.aip.dev/160.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This key is read from <see cref="AITool.AdditionalProperties"/> on the
    /// <see cref="HostedFileSearchTool"/> itself, not from <see cref="ChatOptions.AdditionalProperties"/>.
    /// </para>
    /// <para>
    /// The value must be a <see cref="string"/>, or a <see cref="JsonElement"/> holding one, as a host
    /// that round-trips its settings through JSON delivers. A value of any other type throws a
    /// <see cref="GeminiMappingException"/>; it is not dropped, because a request that silently loses
    /// its filter still succeeds, grounded on the documents the filter was written to exclude.
    /// </para>
    /// </remarks>
    /// <example>
    /// <see cref="AITool.AdditionalProperties"/> has no setter, so the dictionary must be passed to the
    /// constructor:
    /// <code>
    /// var tool = new HostedFileSearchTool(new Dictionary&lt;string, object?&gt;
    /// {
    ///     [GeminiAdditionalProperties.MetadataFilter] = "author = \"Robert Graves\"",
    /// })
    /// {
    ///     Inputs = [new HostedVectorStoreContent("fileSearchStores/poems")],
    /// };
    /// </code>
    /// </example>
    public const string MetadataFilter = "metadataFilter";
}
