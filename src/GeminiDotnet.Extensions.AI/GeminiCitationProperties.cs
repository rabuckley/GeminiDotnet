using GeminiDotnet.V1Beta;
using Microsoft.Extensions.AI;
using System.Text.Json.Nodes;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Keys for the Gemini-specific fields of a grounding chunk that <see cref="CitationAnnotation"/> has no
/// property for, carried in <see cref="AIAnnotation.AdditionalProperties"/>.
/// </summary>
/// <remarks>
/// Every key is optional: it is present only when the chunk carried a value for it. The typed
/// <see cref="GroundingChunk"/> is on <see cref="AIAnnotation.RawRepresentation"/>; these entries exist
/// as well because that property is dropped when a response is serialized.
/// </remarks>
public static class GeminiCitationProperties
{
    /// <summary>
    /// Key for the page the retrieved chunk was found on, as an <see cref="int"/>. Reported for
    /// <see cref="GeminiToolNames.FileSearch"/>.
    /// </summary>
    public const string PageNumber = "pageNumber";

    /// <summary>
    /// Key for the name of the file search store holding the document, as a <see cref="string"/>, in the
    /// form <c>fileSearchStores/123</c>. Reported for <see cref="GeminiToolNames.FileSearch"/>.
    /// </summary>
    public const string FileSearchStore = "fileSearchStore";

    /// <summary>
    /// Key for the metadata supplied when the document was uploaded, as an
    /// <see cref="AdditionalPropertiesDictionary"/> whose values are a <see cref="string"/>, a
    /// <see cref="float"/>, or a <see cref="JsonArray"/> of strings. Reported for
    /// <see cref="GeminiToolNames.FileSearch"/>.
    /// </summary>
    public const string CustomMetadata = "customMetadata";

    /// <summary>
    /// Key for the URL of the image asset itself, as a <see cref="string"/>, where
    /// <see cref="CitationAnnotation.Url"/> is the page it appears on. Reported for
    /// <see cref="GeminiToolNames.GoogleSearch"/>.
    /// </summary>
    public const string ImageUri = "imageUri";

    /// <summary>
    /// Key for the root domain of the page the image is from, as a <see cref="string"/>, for example
    /// <c>example.com</c>. Reported for <see cref="GeminiToolNames.GoogleSearch"/>.
    /// </summary>
    public const string Domain = "domain";
}
