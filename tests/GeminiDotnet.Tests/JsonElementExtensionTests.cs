using System.Text.Json;
using Xunit.Sdk;

namespace GeminiDotnet;

public class JsonElementExtensionTests
{
    [Theory]
    [ClassData(typeof(ReferenceTestCases))]
    public void TryGetFromReference_WhenCalledWithJsonElementReference_ItShouldReturnExpectedValue(ReferenceTestCase testCase)
    {
        throw new NotImplementedException(testCase.TestCaseName);
    }
}

public sealed class ReferenceTestCases : TheoryData<ReferenceTestCase>
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

public sealed record ReferenceTestCase : IXunitSerializable
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
        TestCaseName = info.GetValue<string>(nameof(TestCaseName)) ?? string.Empty;
        JsonElement = info.GetValue<JsonElement>(nameof(JsonElement));
        ReferencePath = info.GetValue<string>(nameof(ReferencePath)) ?? string.Empty;
        ExpectedResult = info.GetValue<bool>(nameof(ExpectedResult));
        ExpectedValue = info.GetValue<JsonElement?>(nameof(ExpectedValue));
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(TestCaseName), TestCaseName);
        info.AddValue(nameof(JsonElement), JsonElement);
        info.AddValue(nameof(ReferencePath), ReferencePath);
        info.AddValue(nameof(ExpectedResult), ExpectedResult);
        info.AddValue(nameof(ExpectedValue), ExpectedValue);
    }
}
