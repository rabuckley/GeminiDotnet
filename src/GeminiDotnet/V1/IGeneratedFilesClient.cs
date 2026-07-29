using GeminiDotnet.V1.Files;
using GeminiDotnet.V1.FileSearchStores;
using GeminiDotnet.V1.FilesRegister;
using GeminiDotnet.V1.Models;
using GeminiDotnet.V1.TunedModels;
using File = GeminiDotnet.V1.Files.File;

namespace GeminiDotnet.V1;

public interface IGeneratedFilesClient
{
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
