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
    public ReferenceTestCases()
    {
    }
}

public sealed record ReferenceTestCase : IXunitSerializable
{
    public string TestCaseName { get; private set; }
    public JsonElement JsonElement { get; private set; }
    public string ReferencePath { get; private set; }
    public bool ExpectedResult { get; private set; }

    public ReferenceTestCase()
    {
        TestCaseName = string.Empty;
        JsonElement = new JsonElement();
        ReferencePath = string.Empty;
        ExpectedResult = false;
    }

    public ReferenceTestCase(
        string testCaseName,
        JsonElement jsonElement,
        string referencePath,
        bool expectedResult
    )
    {
        TestCaseName = testCaseName;
        JsonElement = jsonElement;
        ReferencePath = referencePath;
        ExpectedResult = expectedResult;
    }

    public void Deserialize(IXunitSerializationInfo info)
    {
        TestCaseName = info.GetValue<string>(nameof(TestCaseName)) ?? string.Empty;
        JsonElement = info.GetValue<JsonElement>(nameof(JsonElement));
        ReferencePath = info.GetValue<string>(nameof(ReferencePath)) ?? string.Empty;
        ExpectedResult = info.GetValue<bool>(nameof(ExpectedResult));
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(TestCaseName), TestCaseName);
        info.AddValue(nameof(JsonElement), JsonElement);
        info.AddValue(nameof(ReferencePath), ReferencePath);
        info.AddValue(nameof(ExpectedResult), ExpectedResult);
    }
}
