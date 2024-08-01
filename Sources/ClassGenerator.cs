using System.Text;
using System.Xml.Linq;

namespace GodotUIXml;

class ClassGenerator(string className, XElement root)
{
    public string className { get; } = className;

    const string IDAttribute = "id";
    const string ProtoSceneAttribute = "proto_scene";

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
        source.AppendLine($"public partial class {className} : Control");
        source.AppendLine("{");

        GenerateProperties(root);
        GenerateReadyMethod();

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

        GenerateInstantiationBlock(root);

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
        source.AppendLine($"{target}.{property} = \"{value}\";");
    }

    private void GenerateInstantiationBlock(XElement element)
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

        var id = element.Attribute(IDAttribute);
        if (id is not null)
        {
            var name = id.Value;
            source.AppendLine($"{name} = {node};");
        }

        var attrs = element.Attributes().Where(a => a.Name != IDAttribute && a.Name != ProtoSceneAttribute);
        foreach (var attr in attrs)
        {
            SetProperty(node, attr.Name.LocalName, attr.Value);
        }

        AddChild(rootStack.Peek(), node);

        var elements = element.Elements();
        if (elements.Count() == 0) return;

        Root(node);
        foreach (var elem in elements)
        {
            GenerateInstantiationBlock(elem);
        }
        rootStack.Pop();
    }
}

