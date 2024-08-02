using System.Text;

namespace GodotUIXml;

public abstract record Literal()
{
    public sealed record NumberLiteral(double Value) : Literal();

    public sealed record StringLiteral(string Value) : Literal();

    public sealed record BooleanLiteral(bool Value) : Literal();

    public sealed record ArrayLiteral(List<Literal> Value) : Literal();

    public abstract record VectorLiteral() : Literal();

    public sealed record Vector2Literal(NumberLiteral X, NumberLiteral Y) : VectorLiteral();
    public sealed record Vector3Literal(NumberLiteral X, NumberLiteral Y, NumberLiteral Z) : VectorLiteral();

    public static Literal Parse(string text)
    {
        return text switch
        {
            var s when s.StartsWith("#") => new NumberLiteral(double.Parse(s[1..])),
            var s when s.StartsWith('"') && s.EndsWith('"') => new StringLiteral(s[1..^1]),
            var s when s.StartsWith('\'') && s.EndsWith('\'') => new StringLiteral(s[1..^1]),
            "true" or "false" => new BooleanLiteral(true),
            var s when s.StartsWith("[") && s.EndsWith("]") => ParseArray(s[1..^1]),
            var s when s.StartsWith("(") && s.EndsWith(")") => ParseVector(s[1..^1]),
            _ => new StringLiteral(text)
        };
    }

    public string ToCSharpLiteral()
    {
        return this switch
        {
            NumberLiteral n => n.Value.ToString(),
            StringLiteral s => $"\"{s.Value}\"",
            BooleanLiteral b => b.Value.ToString().ToLower(),
            ArrayLiteral a => $"[ {string.Join(", ", a.Value.Select(x => x.ToCSharpLiteral()))} ]",
            VectorLiteral v => v switch
            {
                Vector2Literal v2 => $"new Vector2({v2.X.Value}, {v2.Y.Value})",
                Vector3Literal v3 => $"new Vector3({v3.X.Value}, {v3.Y.Value}, {v3.Z.Value})",
                _ => throw new NotImplementedException()
            },
            _ => throw new NotImplementedException()
        };
    }

    protected static NumberLiteral ParseNumber(string input)
    {
        var value = double.Parse(input);
        return new NumberLiteral(value);
    }

    protected static StringLiteral ParseString(string input)
    {
        return new StringLiteral(input);
    }

    protected static BooleanLiteral ParseBoolean(string input)
    {
        return new BooleanLiteral(bool.Parse(input));
    }

    protected static ArrayLiteral ParseArray(string input)
    {
        var array = new List<Literal>();
        var splited = input.Split(',').Select(x => x.Trim());
        foreach (var item in splited)
        {
            array.Add(Parse(item));
        }
        return new ArrayLiteral(array);
    }

    protected static VectorLiteral ParseVector(string input)
    {
        var vector = new List<NumberLiteral>();
        var splited = input.Split(',').Select(x => x.Trim()).ToArray();

        return splited switch
        {
            { Length: 2 } => new Vector2Literal(ParseNumber(splited.ElementAt(0)), ParseNumber(splited.ElementAt(1))),
            { Length: 3 } => new Vector3Literal(ParseNumber(splited.ElementAt(0)), ParseNumber(splited.ElementAt(1)), ParseNumber(splited.ElementAt(2))),
            _ => throw new NotImplementedException()
        };
    }
}
