using Godot;
public partial class TestNode2 : TestNode
{
public TestNode Nyan;
public override void _Ready()
{
var root_0 = this;
var node_0 = new Button();
root_0.AddChild(node_0);
var root_1 = node_0;
var scene_0 = GD.Load<PackedScene>("res://Scenes/TestNode.tscn");
var node_1 = scene_0.Instantiate<TestNode>();
this.Nyan = node_1;
node_1.TestValue = "Myaow";
root_1.AddChild(node_1);
var root_2 = node_1;
}
public TestNode GetRoot()
{
return this.GetChild<TestNode>(0);
}
}
