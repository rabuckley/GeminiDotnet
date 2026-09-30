using GeminiDotnet.V1Beta.FileSearchStores;

namespace GeminiDotnet.V1Beta;

public partial interface IFileSearchStoresClient
{
    /// <summary>
    /// Provides access to the Documents API operations.
    /// </summary>
    IFileSearchStoresDocumentsClient Documents { get; }

    /// <summary>
    /// Provides access to the Operations API operations.
    /// </summary>
    IFileSearchStoresOperationsClient Operations { get; }

    /// <summary>
    /// Provides access to the Upload API operations.
    /// </summary>
    IFileSearchStoresUploadClient Upload { get; }

    /// <summary>
    /// Lists all <c>FileSearchStores</c> owned by the user.
    /// </summary>
    /// <param name="pageSize">
    /// Optional. The maximum number of <c>FileSearchStores</c> to return (per page).
    /// The service may return fewer <c>FileSearchStores</c>.
    /// If unspecified, at most 10 <c>FileSearchStores</c> will be returned.
    /// The maximum size limit is 20 <c>FileSearchStores</c> per page.
    /// </param>
    /// <param name="pageToken">
    /// Optional. A page token, received from a previous <c>ListFileSearchStores</c> call.
    /// Provide the <c>next_page_token</c> returned in the response as an argument to
    /// the next request to retrieve the next page.
    /// When paginating, all other parameters provided to <c>ListFileSearchStores</c>
    /// must match the call that provided the page token.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<ListFileSearchStoresResponse> ListAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an empty <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    /// <param name="request">Required. The <see cref="V1Beta.FileSearchStores.FileSearchStore"/> to create.</param>
    /// <param name="cancellationToken"></param>
    Task<FileSearchStore> CreateAsync(
        FileSearchStore request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    /// <param name="name">
    /// Required. The name of the <see cref="V1Beta.FileSearchStores.FileSearchStore"/>. Example: <c>fileSearchStores/my-file-search-store-123</c>
    /// A resource name of the form <c>fileSearchStores/{fileSearchStoreId}</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<FileSearchStore> GetAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    /// <param name="name">
    /// Required. The resource name of the <see cref="V1Beta.FileSearchStores.FileSearchStore"/>. Example: <c>fileSearchStores/my-file-search-store-123</c>
    /// A resource name of the form <c>fileSearchStores/{fileSearchStoreId}</c>.
    /// </param>
    /// <param name="force">
    /// Optional. If set to true, any <see cref="V1Beta.FileSearchStores.Document"/>s and objects related to this
    /// <see cref="V1Beta.FileSearchStores.FileSearchStore"/> will also be deleted.
    /// If false (the default), a <c>FAILED_PRECONDITION</c> error will be returned if
    /// <see cref="V1Beta.FileSearchStores.FileSearchStore"/> contains any <see cref="V1Beta.FileSearchStores.Document"/>s.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteAsync(
        string name,
        bool? force = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Imports a <see cref="V1Beta.Files.File"/> from File Service to a <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    /// <param name="fileSearchStoreName">
    /// Required. Immutable. The name of the <see cref="V1Beta.FileSearchStores.FileSearchStore"/> to import the file into. Example: <c>fileSearchStores/my-file-search-store-123</c>
    /// A resource name of the form <c>fileSearchStores/{fileSearchStoreId}</c>.
    /// </param>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<ImportFileOperation> ImportFileAsync(
        string fileSearchStoreName,
        ImportFileRequest request,
        CancellationToken cancellationToken = default);

}
