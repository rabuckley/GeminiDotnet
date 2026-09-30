using GeminiDotnet.V1Beta.FileSearchStores;

namespace GeminiDotnet.V1Beta;

public partial interface IFileSearchStoresDocumentsClient
{
    /// <summary>
    /// Lists all <see cref="V1Beta.FileSearchStores.Document"/>s in a <see cref="V1Beta.Corpora.Corpus"/>.
    /// </summary>
    /// <param name="parent">
    /// Required. The name of the <see cref="V1Beta.FileSearchStores.FileSearchStore"/> containing <see cref="V1Beta.FileSearchStores.Document"/>s. Example: <c>fileSearchStores/my-file-search-store-123</c>
    /// A resource name of the form <c>fileSearchStores/{fileSearchStoreId}</c>.
    /// </param>
    /// <param name="pageSize">
    /// Optional. The maximum number of <see cref="V1Beta.FileSearchStores.Document"/>s to return (per page).
    /// The service may return fewer <see cref="V1Beta.FileSearchStores.Document"/>s.
    /// If unspecified, at most 10 <see cref="V1Beta.FileSearchStores.Document"/>s will be returned.
    /// The maximum size limit is 20 <see cref="V1Beta.FileSearchStores.Document"/>s per page.
    /// </param>
    /// <param name="pageToken">
    /// Optional. A page token, received from a previous <c>ListDocuments</c> call.
    /// Provide the <c>next_page_token</c> returned in the response as an argument to
    /// the next request to retrieve the next page.
    /// When paginating, all other parameters provided to <c>ListDocuments</c>
    /// must match the call that provided the page token.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<ListDocumentsResponse> ListAsync(
        string parent,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific <see cref="V1Beta.FileSearchStores.Document"/>.
    /// </summary>
    /// <param name="name">
    /// Required. The name of the <see cref="V1Beta.FileSearchStores.Document"/> to retrieve. Example: <c>fileSearchStores/my-file-search-store-123/documents/the-doc-abc</c>
    /// A resource name of the form <c>fileSearchStores/{fileSearchStoreId}/documents/{documentId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Document> GetAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="V1Beta.FileSearchStores.Document"/>.
    /// </summary>
    /// <param name="name">
    /// Required. The resource name of the <see cref="V1Beta.FileSearchStores.Document"/> to delete. Example: <c>fileSearchStores/my-file-search-store-123/documents/the-doc-abc</c>
    /// A resource name of the form <c>fileSearchStores/{fileSearchStoreId}/documents/{documentId}</c>.
    /// </param>
    /// <param name="force">
    /// Optional. If set to true, any <c>Chunk</c>s and objects related to this <see cref="V1Beta.FileSearchStores.Document"/> will
    /// also be deleted.
    /// If false (the default), a <c>FAILED_PRECONDITION</c> error will be returned if
    /// <see cref="V1Beta.FileSearchStores.Document"/> contains any <c>Chunk</c>s.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteAsync(
        string name,
        bool? force = null,
        CancellationToken cancellationToken = default);

}
