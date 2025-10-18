using GeminiDotnet.Text.Json;
using System.Text.Json;

namespace GeminiDotnet;

public sealed class JsonElementExtensionTests
{
    private const string TestSchemaJson = """
    {
        "definitions": {
            "user": {
                "type": "object",
                "properties": {
                    "name": { "type": "string" },
                    "email": { "type": "string" }
                }
            },
            "errorCodes": [
                400,
                404,
                500
            ],
            "a/b": {
                "description": "A key containing a forward slash."
            },
            "c~d": {
                "description": "A key containing a tilde."
            }
        },
        "userReference": {
            "$ref": "#/definitions/user"
        }
    }
    """;

    private readonly JsonElement _testSchema;

    public JsonElementExtensionTests()
    {
        _testSchema = JsonDocument.Parse(TestSchemaJson).RootElement;
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithValidObjectDefinition_ItShouldReturnExpectedResult()
    {
        var reference = "#/definitions/user";
        var expectedValue = _testSchema.GetProperty("definitions").GetProperty("user");
        
        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.True(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithValidReferenceToPropertyOfObjectDefinition_ItShouldReturnExpectedResult()
    {
        var reference = "#/definitions/user/properties/name";
        var expectedValue = _testSchema
            .GetProperty("definitions")
            .GetProperty("user")
            .GetProperty("properties")
            .GetProperty("name");

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.True(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithValidReferenceToArrayItem_ItShouldReturnExpectedResult()
    {
        var reference = "#/definitions/errorCodes/1";
        var expectedValue = _testSchema
            .GetProperty("definitions")
            .GetProperty("errorCodes")[1];

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.True(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithValidReferenceWithEscapedForwardSlash_ItShouldReturnExpectedResult()
    {
        var reference = "#/definitions/a~1b";
        var expectedValue = _testSchema
            .GetProperty("definitions")
            .GetProperty("a/b");

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.True(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithValidReferenceWithEscapedTilde_ItShouldReturnExpectedResult()
    {
        var reference = "#/definitions/c~0d";
        var expectedValue = _testSchema
            .GetProperty("definitions")
            .GetProperty("c~d");

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.True(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithInvalidNonExistentDefinition_ItShouldReturnExpectedResult()
    {
        var reference = "#/definitions/nonexistent";
        JsonElement expectedValue = default;

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.False(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithInvalidIndex_ItShouldReturnExpectedResult()
    {
        var reference = "#/definitions/errorCodes/5";
        JsonElement expectedValue = default;

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.False(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithNonAbsolutePath_ItShouldReturnExpectedResult()
    {
        var reference = "definitions/user";
        JsonElement expectedValue = default;

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.False(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithRootPath_ItShouldReturnExpectedResult()
    {
        var reference = "#";
        var expectedValue = _testSchema;

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.True(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithEmptyPath_ItShouldReturnExpectedResult()
    {
        var reference = string.Empty;
        JsonElement expectedValue = default;

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.False(result);
        Assert.Equivalent(expectedValue, resultValue);
    }
}
