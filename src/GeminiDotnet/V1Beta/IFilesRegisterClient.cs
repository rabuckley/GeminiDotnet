using GeminiDotnet.V1Beta.AuthTokens;
using GeminiDotnet.V1Beta.CachedContents;
using GeminiDotnet.V1Beta.Corpora;
using GeminiDotnet.V1Beta.Files;
using GeminiDotnet.V1Beta.FileSearchStores;
using GeminiDotnet.V1Beta.FilesRegister;
using GeminiDotnet.V1Beta.GeneratedFiles;
using GeminiDotnet.V1Beta.Models;
using GeminiDotnet.V1Beta.TunedModels;
using File = GeminiDotnet.V1Beta.Files.File;

namespace GeminiDotnet.V1Beta;

public interface IFilesRegisterClient
{
    /// <summary>
    /// Registers a Google Cloud Storage files with FileService. The user is
    /// expected to provide Google Cloud Storage URIs and will receive a File
    /// resource for each URI in return. Note that the files are not copied, just
    /// registered with File API. If one file fails to register, the whole request
    /// fails.
    /// </summary>
    /// <param name="request">The request body.</param>
    /// <param name="cancellationToken"></param>
    Task<RegisterFilesResponse> RegisterFilesAsync(
        RegisterFilesRequest request,
        CancellationToken cancellationToken = default);

}
