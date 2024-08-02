using Godot;
public partial class MyMain : Container
{
public override void _Ready()
{
var root_0 = this;
var scene_0 = GD.Load<PackedScene>("res://main.tscn");
var node_0 = scene_0.Instantiate<Main>();
node_0.Position = new Vector2(213, 11);
root_0.AddChild(node_0);
var root_1 = node_0;
}
}
