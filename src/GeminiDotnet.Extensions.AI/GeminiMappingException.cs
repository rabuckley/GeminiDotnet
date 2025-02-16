using System.Diagnostics.CodeAnalysis;

namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// An exception that is thrown when mapping fails between GeminiDotnet and Microsoft.Extensions.AI types.
/// </summary>
public sealed class GeminiMappingException : Exception
{
    private GeminiMappingException(string message) : base(message)
    {
    }

    [DoesNotReturn]
    internal static void Throw(string fromPropertyName, string toPropertyName, string reason)
    {
        var message = $"Mapping from '{fromPropertyName}' to '{toPropertyName}' failed. Reason: {reason}";
        throw new GeminiMappingException(message);
    }
}
