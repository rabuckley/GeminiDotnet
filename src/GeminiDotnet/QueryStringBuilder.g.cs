// The ".g.cs" suffix makes the compiler treat this as generated code, where a
// nullable annotation needs the context turned on in the file itself (CS8669).
#nullable enable

using System.Globalization;
using System.Text;

namespace GeminiDotnet;

/// <summary>
/// Builds the query-string portion of a request path. A null value omits the pair entirely;
/// the first appended pair contributes the leading '?', subsequent pairs '&'. Names are
/// spec-supplied literals and are appended verbatim; values are escaped.
/// </summary>
internal sealed class QueryStringBuilder
{
    private readonly StringBuilder _builder = new();

    public QueryStringBuilder Add(string name, string? value)
    {
        if (value is null)
        {
            return this;
        }

        AppendSeparatorAndName(name);
        _builder.Append(Uri.EscapeDataString(value));
        return this;
    }

    public QueryStringBuilder Add(string name, int? value)
    {
        if (value is null)
        {
            return this;
        }

        AppendSeparatorAndName(name);
        _builder.Append(value.Value.ToString(CultureInfo.InvariantCulture));
        return this;
    }

    public QueryStringBuilder Add(string name, long? value)
    {
        if (value is null)
        {
            return this;
        }

        AppendSeparatorAndName(name);
        _builder.Append(value.Value.ToString(CultureInfo.InvariantCulture));
        return this;
    }

    public QueryStringBuilder Add(string name, bool? value)
    {
        if (value is null)
        {
            return this;
        }

        AppendSeparatorAndName(name);
        _builder.Append(value.Value ? "true" : "false");
        return this;
    }

    private void AppendSeparatorAndName(string name)
    {
        _builder.Append(_builder.Length == 0 ? '?' : '&');
        _builder.Append(name);
        _builder.Append('=');
    }

    public override string ToString() => _builder.ToString();
}
