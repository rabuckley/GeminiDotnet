using GeminiDotnet.V1Beta.GeneratedFiles;

namespace GeminiDotnet.V1Beta;

public interface IGeneratedFilesClient
{
    /// <summary>
    /// Lists the generated files owned by the requesting project.
    /// </summary>
    /// <param name="pageSize">
    /// Optional. Maximum number of <see cref="V1Beta.GeneratedFiles.GeneratedFile"/>s to return per page.
    /// If unspecified, defaults to 10. Maximum <c>page_size</c> is 50.
    /// </param>
    /// <param name="pageToken">Optional. A page token from a previous <c>ListGeneratedFiles</c> call.</param>
    /// <param name="cancellationToken"></param>
    Task<ListGeneratedFilesResponse> ListGeneratedFilesAsync(
        int? pageSize = null,
        string? pageToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a generated file. When calling this method via REST, only the metadata
    /// of the generated file is returned. To retrieve the file content via REST,
    /// add alt=media as a query parameter.
    /// </summary>
    /// <param name="generatedFile">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<GeneratedFile> GetGeneratedFileAsync(
        string generatedFile,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest state of a long-running operation.  Clients can use this
    /// method to poll the operation result at intervals as recommended by the API
    /// service.
    /// </summary>
    /// <param name="generatedFile">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="operation">Resource ID segment making up resource <c>name</c>. It identifies the resource within its parent collection as described in https://google.aip.dev/122.</param>
    /// <param name="cancellationToken"></param>
    Task<Operation> GetOperationByGeneratedFileAndOperationAsync(
        string generatedFile,
        string operation,
        CancellationToken cancellationToken = default);

}
