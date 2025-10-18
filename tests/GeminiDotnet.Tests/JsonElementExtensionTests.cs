using GeminiDotnet.ContentGeneration;
using System.Text.Json;
using Xunit.Sdk;

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
        JsonElement? expectedValue = null;

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.False(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithInvalidIndex_ItShouldReturnExpectedResult()
    {
        var reference = "#/definitions/errorCodes/5";
        JsonElement? expectedValue = null;

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.False(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    [Fact]
    public void TryGetFromReference_WhenCalledWithNonAbsolutePath_ItShouldReturnExpectedResult()
    {
        var reference = "definitions/user";
        JsonElement? expectedValue = null;

        var result = _testSchema.TryGetFromReference(reference, out var resultValue);

        Assert.False(result);
        Assert.Equivalent(expectedValue, resultValue);
    }

    // [Theory]
    // [ClassData(typeof(ReferenceTestCases))]
    // public void TryGetFromReference_WhenCalledWithJsonElementReference_ItShouldReturnExpectedValue(ReferenceTestCase testCase)
    // {
    //     var result = testCase.JsonElement.TryGetFromReference(testCase.ReferencePath, out var resultValue);

    //     Assert.Equal(testCase.ExpectedResult, result);
    //     Assert.Equivalent(testCase.ExpectedValue, resultValue);
    // }
}

public class ReferenceTestCases : TheoryData<ReferenceTestCase>
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

    public ReferenceTestCases()
    {
        var testSchemaElement = JsonDocument.Parse(TestSchemaJson).RootElement;

        Add(
            new ReferenceTestCase(
                "Valid reference to object definition",
                testSchemaElement,
                "#/definitions/user",
                true,
                testSchemaElement.GetProperty("definitions").GetProperty("user")
            )
        );

        Add(
            new ReferenceTestCase(
                "Valid reference to property of object definition",
                testSchemaElement,
                "#/definitions/user/properties/name",
                true,
                testSchemaElement.GetProperty("definitions").GetProperty("user").GetProperty("properties").GetProperty("name")
            )
        );

        Add(
            new ReferenceTestCase(
                "Valid reference to array item",
                testSchemaElement,
                "#/definitions/errorCodes/1",
                true,
                testSchemaElement.GetProperty("definitions").GetProperty("errorCodes")[1]
            )
        );

        Add(
            new ReferenceTestCase(
                "Valid reference with escaped forward slash",
                testSchemaElement,
                "#/definitions/a~1b",
                true,
                testSchemaElement.GetProperty("definitions").GetProperty("a/b")
            )
        );

        Add(
            new ReferenceTestCase(
                "Valid reference with escaped tilde",
                testSchemaElement,
                "#/definitions/c~0d",
                true,
                testSchemaElement.GetProperty("definitions").GetProperty("c~d")
            )
        );

        Add(
            new ReferenceTestCase(
                "Invalid reference to nonexistent definition",
                testSchemaElement,
                "#/definitions/nonexistent",
                false,
                null
            )
        );

        Add(
            new ReferenceTestCase(
                "Invalid reference to out-of-bounds array index",
                testSchemaElement,
                "#/definitions/errorCodes/5",
                false,
                null
            )
        );

        Add(
            new ReferenceTestCase(
                "Reference to root element",
                testSchemaElement,
                "#",
                true,
                testSchemaElement
            )
        );

        Add(
            new ReferenceTestCase(
                "Invalid reference without absolute path",
                testSchemaElement,
                "definitions/user",
                false,
                null
            )
        );
    }
}

public class ReferenceTestCase : IXunitSerializable
{
    public string TestCaseName { get; private set; }
    public JsonElement JsonElement { get; private set; }
    public string ReferencePath { get; private set; }
    public bool ExpectedResult { get; private set; }
    public JsonElement? ExpectedValue { get; private set; }

    public ReferenceTestCase()
    {
        TestCaseName = string.Empty;
        JsonElement = new JsonElement();
        ReferencePath = string.Empty;
        ExpectedResult = false;
        ExpectedValue = null;
    }

    public ReferenceTestCase(
        string testCaseName,
        JsonElement jsonElement,
        string referencePath,
        bool expectedResult,
        JsonElement? expectedValue
    )
    {
        TestCaseName = testCaseName;
        JsonElement = jsonElement;
        ReferencePath = referencePath;
        ExpectedResult = expectedResult;
        ExpectedValue = expectedValue;
    }

    public void Deserialize(IXunitSerializationInfo info)
    {
    }

    public void Serialize(IXunitSerializationInfo info)
    {
    }

    public override string ToString() => TestCaseName;
}
