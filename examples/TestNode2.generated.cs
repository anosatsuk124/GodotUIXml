using Godot;
public partial class TestNode2 : Control
{
public TestNode Nyan;
public override void _Ready()
{
var root_0 = this;
var scene_0 = GD.Load<PackedScene>("res://Scenes/TestNode.tscn");
var node_0 = scene_0.Instantiate<TestNode>();
root_0.AddChild(node_0);
var root_1 = node_0;
var node_1 = new Button();
root_1.AddChild(node_1);
var root_2 = node_1;
var scene_1 = GD.Load<PackedScene>("res://Scenes/TestNode.tscn");
var node_2 = scene_1.Instantiate<TestNode>();
this.Nyan = node_2;
node_2.TestValue = "Myaow";
root_2.AddChild(node_2);
}
public TestNode GetRoot()
{
return this.GetChild<TestNode>(0);
}
}
