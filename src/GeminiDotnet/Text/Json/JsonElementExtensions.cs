using System.Text.Json;

namespace GeminiDotnet.Text.Json;

internal static class JsonElementExtensions
{
    private const char RootCharacter = '#';
    private const char SeparatorCharacter = '/';
    private const char TildeCharacter = '~';
    private static string EscapedSeparator => $"{TildeCharacter}1";
    private static string EscapedTilde => $"{TildeCharacter}0";
    private static string RootPathStart => $"{RootCharacter}{SeparatorCharacter}";

    public static bool TryGetFromReference(
        this JsonElement element,
        string referencePath,
        out JsonElement value
    )
    {
        value = default;

        if (
            string.IsNullOrEmpty(referencePath) ||
            !referencePath.StartsWith(RootPathStart, StringComparison.OrdinalIgnoreCase)
        )
        {
            return false;
        }

        var result = true;
        var current = element;
        var segments = referencePath[2..].Split(SeparatorCharacter);

        foreach (var segment in segments)
        {
            var unescapedSegment = segment
                .Replace(
                    EscapedSeparator,
                    SeparatorCharacter.ToString(),
                    StringComparison.CurrentCultureIgnoreCase
                )
                .Replace(
                    EscapedTilde,
                    TildeCharacter.ToString(),
                    StringComparison.OrdinalIgnoreCase
                );

            if (current.ValueKind is JsonValueKind.Object)
            {
                if (!current.TryGetProperty(unescapedSegment, out var next))
                {
                    result = false;
                    break;
                }

                current = next;
                continue;
            }

            if (current.ValueKind is JsonValueKind.Array)
            {
                if (!int.TryParse(unescapedSegment, out var index) || index < 0)
                {
                    result = false;
                    break;
                }

                if (index >= current.GetArrayLength())
                {
                    result = false;
                    break;
                }

                var next = current[index];
                current = next;
                continue;
            }
        }

        value = result ? current : default;
        return result;
    }
}
