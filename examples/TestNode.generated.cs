using Godot;
public partial class TestNode : Control
{
public Control root;
public Button unique1;
public Label unique2;
public RichTextLabel unique4;
public Label unique3;
public Label unique5;
public override void _Ready()
{
var root_0 = this;
this.root = this;
var node_0 = new Button();
this.unique1 = node_0;
node_0.Text = "Hello";
root_0.AddChild(node_0);
var root_1 = node_0;
var node_1 = new Label();
this.unique2 = node_1;
node_1.Text = "World";
root_1.AddChild(node_1);
var root_2 = node_1;
var node_2 = new RichTextLabel();
this.unique4 = node_2;
root_1.AddChild(node_2);
var root_3 = node_2;
var node_3 = new Label();
this.unique3 = node_3;
root_1.AddChild(node_3);
var root_4 = node_3;
var node_4 = new Label();
this.unique5 = node_4;
root_0.AddChild(node_4);
var root_5 = node_4;
var node_5 = new Button();
node_5.Text = "Hello No id";
root_0.AddChild(node_5);
var root_6 = node_5;
var scene_0 = GD.Load<PackedScene>("res://my_label.tscn");
var node_6 = scene_0.Instantiate<Label>();
root_0.AddChild(node_6);
var root_7 = node_6;
}
public Control GetRoot()
{
return this.GetChild<Control>(0);
}
}
