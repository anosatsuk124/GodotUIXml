using System.Text;

namespace GodotUIXml;

public abstract record Literal()
{
    public abstract string ToCSharpLiteral();

    public sealed record NumberLiteral(double Value) : Literal()
    {
        public override string ToCSharpLiteral()
    => Value.ToString();
    }

    public sealed record StringLiteral(string Value) : Literal()
    {
        public override string ToCSharpLiteral() =>
            $"\"{Value}\"";
    }

    public sealed record BooleanLiteral(bool Value) : Literal()
    {
        public override string ToCSharpLiteral()
            => Value.ToString().ToLower();
    }

    public sealed record ListLiteral(List<Literal> Value) : Literal()
    {
        public override string ToCSharpLiteral()
        => string.Join(", ", Value.Select(x => x.ToCSharpLiteral()));
    }

    public sealed record ArrayLiteral(ListLiteral Value) : Literal()
    {
        public override string ToCSharpLiteral()
            => $"[ {Value.ToCSharpLiteral()} ]";
    }

    public sealed record ClassLiteral(string Name, ListLiteral Value) : Literal()
    {
        public override string ToCSharpLiteral()
            => $"new {Name}({Value.ToCSharpLiteral()})";
    }

    public abstract record VectorLiteral() : Literal();

    public sealed record Vector2Literal(NumberLiteral X, NumberLiteral Y) : VectorLiteral()
    {
        public override string ToCSharpLiteral()
            => $"new Vector2({X.ToCSharpLiteral()}, {Y.ToCSharpLiteral()})";
    }

    public sealed record Vector3Literal(NumberLiteral X, NumberLiteral Y, NumberLiteral Z) : VectorLiteral()
    {
        public override string ToCSharpLiteral()
            => $"new Vector3({X.ToCSharpLiteral()}, {Y.ToCSharpLiteral()}, {Z.ToCSharpLiteral()})";
    }

    public static Literal Parse(string text)
    {
        return text switch
        {
        ['#', .. var s] => ParseNumber(s),
        ['@', .. var s] => ParseClass(s),
        ['"', .. var s, '"'] => ParseString(s),
        ['\'', .. var s, '\''] => ParseString(s),
        ['[', .. var s, ']'] => ParseArray(s),
        ['(', .. var s, ')'] => ParseVector(s),
            "true" or "false" => ParseBoolean(text),
            _ => ParseString(text),
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
        var list = ParseList(input);
        return new ArrayLiteral(list);
    }

    protected static ClassLiteral ParseClass(string input, string? name = null)
    {
        if (name is not null)
        {
            var listString = input.Substring(name.Length).TrimStart('(').TrimEnd(')');
            return new ClassLiteral(name, ParseList(listString));
        }

        var _name = input.Split('(').First();

        return ParseClass(input, _name);
    }

    private static ListLiteral ParseList(string input)
    {
        var array = new List<Literal>();
        var splited = input.Split(',').Select(x => x.Trim());
        foreach (var item in splited)
        {
            array.Add(Parse(item));
        }

        return new ListLiteral(array);
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
