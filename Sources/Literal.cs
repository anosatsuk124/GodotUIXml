using System.Text;

namespace GodotUIXml;

public abstract record Literal()
{
    public sealed record NumberLiteral(double Value) : Literal();

    public sealed record StringLiteral(string Value) : Literal();

    public sealed record BooleanLiteral(bool Value) : Literal();

    public sealed record ArrayLiteral(List<Literal> Value) : Literal();

    public static Literal Parse(string text)
    {
        return text switch
        {
            var s when s.StartsWith("#") => new NumberLiteral(double.Parse(s)),
            _ => throw new NotImplementedException()
        };
    }

    public static NumberLiteral ParseNumber(string input)
    {
        var value = double.Parse(input);
        return new NumberLiteral(value);
    }

    public static StringLiteral ParseString(string input)
    {
        return new StringLiteral(input);
    }

    public static BooleanLiteral ParseBoolean(string input)
    {
        return new BooleanLiteral(bool.Parse(input));
    }

    public static ArrayLiteral ParseArray(string input)
    {
        var array = new List<Literal>();
        var splited = input.Split(',').Select(x => x.Trim());
        foreach (var item in splited)
        {
            array.Add(Parse(item));
        }
        return new ArrayLiteral(array);
    }
}
