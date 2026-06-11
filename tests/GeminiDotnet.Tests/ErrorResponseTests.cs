using GeminiDotnet.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace GeminiDotnet;

public sealed class ErrorResponseTests
{
    [Fact]
    public void Deserialize_WithErrorResponse()
    {
        // Arrange
        var typeInfo = V1BetaJsonContext.Default.GetTypeInfo<ErrorResponse>();

        // Act
        var errorResponse = JsonSerializer.Deserialize(ErrorResponseJson, typeInfo);

        // Assert
        Assert.NotNull(errorResponse);
        Assert.Equal(HttpStatusCode.BadRequest, errorResponse.Error.StatusCode);
        Assert.Equal("Please use a valid role: user, model.", errorResponse.Error.Message);
        Assert.Equal("INVALID_ARGUMENT", errorResponse.Error.Status);
    }

    [StringSyntax(StringSyntaxAttribute.Json)]
    private const string ErrorResponseJson =
        """
        {
          "error": {
            "code": 400,
            "message": "Please use a valid role: user, model.",
            "status": "INVALID_ARGUMENT"
          }
        }
        """;
}
