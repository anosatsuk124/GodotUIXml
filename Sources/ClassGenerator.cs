using System.Text;
using System.Xml.Linq;

namespace GodotUIXml;

class ClassGenerator
{
    public string className { get; }
    public XElement root { get; }

    private ClassGenerator(string _className, XElement _root)
    {
        className = _className;
        root = _root;
    }

    protected const string IDAttribute = "id";
    protected const string ProtoSceneAttribute = "proto_scene";

    private int rootIndex = 0;
    private int sceneIndex = 0;
    private int nodeIndex = 0;

    private Stack<string> rootStack = new Stack<string>();

    private StringBuilder source = new StringBuilder();

    public static ClassGenerator FromXml(string xmlPath)
    {
        var doc = XDocument.Load(xmlPath);
        var name = Path.GetFileNameWithoutExtension(xmlPath);

        if (doc.Root is null) throw new Exception("Root element is missing");
        return new ClassGenerator(name, doc.Root);
    }

    public string Generate()
    {
        GenerateClassFile();

        return source.ToString();
    }

    private void GenerateClassFile()
    {
        source.AppendLine("using Godot;");
        GenerateClass();
    }

    private void GenerateClass()
    {
        var type = root.Name.LocalName;
        source.AppendLine($"public partial class {className} : {type}");
        source.AppendLine("{");

        GenerateProperties(root);

        GenerateDefaultConstructor();

        GenerateReadyMethod();

        source.AppendLine("}");
    }

    private void GenerateDefaultConstructor()
    {
        source.AppendLine($"public {className}() : base()");
        source.AppendLine("{");
        source.AppendLine("}");
    }

    private void GenerateProperties(XElement element)
    {
        var id = element.Attribute(IDAttribute);
        if (id is not null)
        {
            var type = element.Name.LocalName;
            var name = id.Value;
            source.AppendLine($"public {type} {name};");
        }

        var elements = element.Elements();
        foreach (var elem in elements)
        {
            GenerateProperties(elem);
        }
    }

    private void GenerateReadyMethod()
    {
        source.AppendLine("public override void _Ready()");
        source.AppendLine("{");

        Root("this");

        GenerateSetPropertyStatemement("this", root);

        var children = root.Elements();
        GenerateInstantiationBlock(children);

        source.AppendLine("}");
    }

    private string Root(string value)
    {
        var var_name = $"root_{rootIndex}";

        rootStack.Push(var_name);

        source.AppendLine($"var {var_name} = {value};");
        rootIndex++;

        return var_name;
    }

    private string Scene(string value)
    {
        var var_name = $"scene_{sceneIndex}";

        source.AppendLine($"var {var_name} = {value};");
        sceneIndex++;

        return var_name;
    }

    private string Node(string value)
    {
        var var_name = $"node_{nodeIndex}";

        source.AppendLine($"var {var_name} = {value};");
        nodeIndex++;

        return var_name;
    }

    private void AddChild(string parent, string child)
    {
        source.AppendLine($"{parent}.AddChild({child});");
    }

    private void SetProperty(string target, string property, string value)
    {
        var sb = new StringBuilder();
        sb.Append($"{target}.{property} = ");

        var literal = Literal.Parse(value);

        sb.Append(literal.ToCSharpLiteral());

        sb.Append(";");

        source.AppendLine(sb.ToString());
    }

    private void GenerateSetPropertyStatemement(string node, XElement element)
    {
        var id = element.Attribute(IDAttribute);
        if (id is not null)
        {
            var name = id.Value;
            source.AppendLine($"this.{name} = {node};");
        }

        var attrs = element.Attributes().Where(a => a.Name != IDAttribute && a.Name != ProtoSceneAttribute);
        foreach (var attr in attrs)
        {
            SetProperty(node, attr.Name.LocalName, attr.Value);
        }
    }

    private void GenerateInstantiationStatements(XElement element)
    {
        var type = element.Name.LocalName;
        var scene = element.Attribute(ProtoSceneAttribute);

        string node;
        if (scene is not null)
        {
            var path = scene.Value;
            var loadedScene = Scene($"GD.Load<PackedScene>(\"res://{path}\")");
            node = Node($"{loadedScene}.Instantiate<{type}>()");
        }
        else
        {
            node = Node($"new {type}()");
        }

        GenerateSetPropertyStatemement(node, element);

        AddChild(rootStack.Peek(), node);

        Root(node);
    }

    private void GenerateInstantiationBlock(IEnumerable<XElement> elements)
    {
        foreach (var element in elements)
        {
            GenerateInstantiationStatements(element);

            var children = element.Elements();
            GenerateInstantiationBlock(children);
            rootStack.Pop();
        }
    }
}

