using GeminiDotnet.V1Beta.FileSearchStores;

namespace GeminiDotnet.V1Beta;

public interface IFileSearchStoresClient
{
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
    Task<ListFileSearchStoresResponse> ListFileSearchStoresAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an empty <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    /// <param name="request">
    /// Required. The <see cref="V1Beta.FileSearchStores.FileSearchStore"/> to create.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<FileSearchStore> CreateFileSearchStoreAsync(
        FileSearchStore request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    /// <param name="fileSearchStore">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<FileSearchStore> GetFileSearchStoreAsync(
        string fileSearchStore,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    /// <param name="fileSearchStore">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="force">
    /// Optional. If set to true, any <see cref="V1Beta.FileSearchStores.Document"/>s and objects related to this
    /// <see cref="V1Beta.FileSearchStores.FileSearchStore"/> will also be deleted.
    /// If false (the default), a <c>FAILED_PRECONDITION</c> error will be returned if
    /// <see cref="V1Beta.FileSearchStores.FileSearchStore"/> contains any <see cref="V1Beta.FileSearchStores.Document"/>s.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteFileSearchStoreAsync(
        string fileSearchStore,
        bool? force = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Imports a <see cref="V1Beta.Files.File"/> from File Service to a <see cref="V1Beta.FileSearchStores.FileSearchStore"/>.
    /// </summary>
    /// <param name="fileSearchStore">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="request">
    /// The request body.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<ImportFileOperation> ImportFileAsync(
        string fileSearchStore,
        ImportFileRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads data to a FileSearchStore, preprocesses and chunks before storing
    /// it in a FileSearchStore Document.
    /// </summary>
    /// <param name="fileSearchStore">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="request">
    /// The request body.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<UploadToFileSearchStoreOperation> UploadToFileSearchStoreAsync(
        string fileSearchStore,
        UploadToFileSearchStoreRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all <see cref="V1Beta.FileSearchStores.Document"/>s in a <see cref="V1Beta.Corpora.Corpus"/>.
    /// </summary>
    /// <param name="fileSearchStore">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
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
    Task<ListDocumentsResponse> ListDocumentsAsync(
        string fileSearchStore,
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific <see cref="V1Beta.FileSearchStores.Document"/>.
    /// </summary>
    /// <param name="fileSearchStore">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="document">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Document> GetDocumentAsync(
        string fileSearchStore,
        string document,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a <see cref="V1Beta.FileSearchStores.Document"/>.
    /// </summary>
    /// <param name="fileSearchStore">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="document">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="force">
    /// Optional. If set to true, any <c>Chunk</c>s and objects related to this <see cref="V1Beta.FileSearchStores.Document"/> will
    /// also be deleted.
    /// If false (the default), a <c>FAILED_PRECONDITION</c> error will be returned if
    /// <see cref="V1Beta.FileSearchStores.Document"/> contains any <c>Chunk</c>s.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Empty> DeleteDocumentAsync(
        string fileSearchStore,
        string document,
        bool? force = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest state of a long-running operation.  Clients can use this
    /// method to poll the operation result at intervals as recommended by the API
    /// service.
    /// </summary>
    /// <param name="fileSearchStore">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="operation">
    /// Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Operation> GetOperationByFileSearchStoreAndOperationAsync(
        string fileSearchStore,
        string operation,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest state of a long-running operation.  Clients can use this
    /// method to poll the operation result at intervals as recommended by the API
    /// service.
    /// </summary>
    /// <param name="fileSearchStoresId">
    /// Part of <c>name</c>. The name of the operation resource.
    /// </param>
    /// <param name="operationsId">
    /// Part of <c>name</c>. See documentation of <c>fileSearchStoresId</c>.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task<Operation> GetOperationByFileSearchStoresIdAndOperationsIdAsync(
        string fileSearchStoresId,
        string operationsId,
        CancellationToken cancellationToken = default);

}
