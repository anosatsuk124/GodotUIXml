# Godot UI Generator from XML

## Usage and Installation

### Installation

```shell
git clone git@github.com:anosatsuk124/GodotUIXml.git
cd GodotUIXml
```

### Usage

```shell
dotnet run /path/to/xml/File.xml
```

🥳: That's it! The generated code will be saved as `File.generated.cs`.

## Sample Generated Code

> [!NOTE]
> For more detailed examples, see [`/examples`](https://github.com/anosatsuk124/GodotUIXml/tree/master/examples) directory.

FileName: `TestNode.xml`

```xml
<Control id="root" proto_scene="my_window.tscn">
  <Button id="unique1"
    Text="Hello">
    <Label id="unique2" Text="World"/>
    <RichTextLabel id="unique4"></RichTextLabel>
    <Label id="unique3"></Label>
  </Button>
  <Label id="unique5"></Label>
  <!-- With no ids below -->
  <Button Text="Hello No id"/>
  <Label proto_scene="my_label.tscn"/>
</Control>
```

FileName: `TestNode.generated.cs`

```csharp
using Godot;

public partial class TestNode : Node
{
    // Only the elements with an `id` should be generated.
    // The names and classes of the properties are determined by the `id` attribute and the element name.

    public Control root;
    public Button unique1;
    public Label unique2;
    public RichTextLabel unique4;
    public Label unique3;
    public Label unique5;

    // All the local variable names are suffixed by the index numbers.
    // `root_{index}` is a variable to `addChild`.
    // `proto_scene` attribute is used to load the scene.
    // `scene_{index}` is used to load the scene.
    // `node_{index}` is used to instantiate a node.

    public override void _Ready()
    {
        var root_0 = this;
        var scene_0 = GD.Load<PackedScene>("res://my_window.tscn");
        var node_0 = scene_0.Instantiate<Control>();
        root = node_0;
        root_0.AddChild(node_0);
        var root_1 = node_0;
        var node_1 = new Button();
        unique1 = node_1;
        node_1.Text = "Hello";
        root_1.AddChild(node_1);
        var root_2 = node_1;
        var node_2 = new Label();
        unique2 = node_2;
        node_2.Text = "World";
        root_2.AddChild(node_2);
        var node_3 = new RichTextLabel();
        unique4 = node_3;
        root_2.AddChild(node_3);
        var node_4 = new Label();
        unique3 = node_4;
        root_2.AddChild(node_4);
        var node_5 = new Label();
        unique5 = node_5;
        root_1.AddChild(node_5);
        var node_6 = new Button();
        node_6.Text = "Hello No id";
        root_1.AddChild(node_6);
        var scene_1 = GD.Load<PackedScene>("res://my_label.tscn");
        var node_7 = scene_1.Instantiate<Label>();
        root_1.AddChild(node_7);
    }
}
```
