namespace GodotUIXml;

class Program
{
    static void Main(string[] args)
    {
        var path = Path.GetFullPath(args[0]);

        var generator = ClassGenerator.FromXml(path);

        var generatedFile = generator.Generate();

        File.WriteAllText($"{generator.className}.generated.cs", generatedFile);
    }
}
