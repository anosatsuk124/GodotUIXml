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
var scene_0 = GD.Load<PackedScene>("res://my_window.tscn");
var node_0 = scene_0.Instantiate<Control>();
this.root = node_0;
root_0.AddChild(node_0);
var root_1 = node_0;
var node_1 = new Button();
this.unique1 = node_1;
node_1.Text = "Hello";
root_1.AddChild(node_1);
var root_2 = node_1;
var node_2 = new Label();
this.unique2 = node_2;
node_2.Text = "World";
root_2.AddChild(node_2);
var node_3 = new RichTextLabel();
this.unique4 = node_3;
root_2.AddChild(node_3);
var node_4 = new Label();
this.unique3 = node_4;
root_2.AddChild(node_4);
var node_5 = new Label();
this.unique5 = node_5;
root_1.AddChild(node_5);
var node_6 = new Button();
node_6.Text = "Hello No id";
root_1.AddChild(node_6);
var scene_1 = GD.Load<PackedScene>("res://my_label.tscn");
var node_7 = scene_1.Instantiate<Label>();
root_1.AddChild(node_7);
}
public Control GetRoot()
{
return this.GetChild<Control>(0);
}
}
