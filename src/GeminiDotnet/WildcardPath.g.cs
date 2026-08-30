namespace GeminiDotnet;

/// <summary>
/// Escapes the value of a multi-segment wildcard path capture ("{path=**}"): each '/'-separated
/// segment is escaped individually so the value's structure survives while its content cannot
/// break out of the path or corrupt the URI.
/// </summary>
internal static class WildcardPath
{
    public static string Escape(string value)
    {
        var segments = value.Split('/');

        for (var i = 0; i < segments.Length; i++)
        {
            segments[i] = Uri.EscapeDataString(segments[i]);
        }

        return string.Join('/', segments);
    }
}
