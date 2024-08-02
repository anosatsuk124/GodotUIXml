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
            ['#', .. var s] => ParseNumber(s),
            ['"', .. var s, '"'] => ParseString(s),
            ['\'', .. var s, '\''] => ParseString(s),
            ['[', .. var s, ']'] => ParseArray(s),
            ['(', .. var s, ')'] => ParseVector(s),
            "true" or "false" => ParseBoolean(text),
            _ => ParseString(text),
        };
    }

    public string ToCSharpLiteral()
    {
        return this switch
        {
            NumberLiteral n => n.Value.ToString(),
            StringLiteral s => $"\"{s.Value.ToString()}\"",
            BooleanLiteral b => b.Value.ToString().ToLower(),
            ArrayLiteral a => $"[ {string.Join(", ", a.Value.Select(x => x.ToCSharpLiteral()))} ]",
            VectorLiteral v => v switch
            {
                Vector2Literal v2 => $"new Vector2({v2.X.ToCSharpLiteral()}, {v2.Y.ToCSharpLiteral()})",
                Vector3Literal v3 => $"new Vector3({v3.X.ToCSharpLiteral()}, {v3.Y.ToCSharpLiteral()}, {v3.Z.ToCSharpLiteral()})",
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
