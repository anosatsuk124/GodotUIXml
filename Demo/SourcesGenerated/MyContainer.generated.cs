using Godot;
public partial class MyContainer : TabContainer
{
public Control editor1;
public MyEditor editor2;
public override void _Ready()
{
var root_0 = this;
var node_0 = new Control();
this.editor1 = node_0;
root_0.AddChild(node_0);
var root_1 = node_0;
var node_1 = new MyEditor();
this.editor2 = node_1;
node_1.Name = "Editor2";
root_0.AddChild(node_1);
var root_2 = node_1;
}
}
