using Godot;
public partial class MyEditor : VBoxContainer
{
public TextEdit textEdit;
public override void _Ready()
{
var root_0 = this;
this.Name = "Editor";
var node_0 = new TextEdit();
this.textEdit = node_0;
node_0.PlaceholderText = "Enter your text here";
node_0.CustomMinimumSize = new Vector2(320, 320);
node_0.DrawSpaces = true;
root_0.AddChild(node_0);
var root_1 = node_0;
var node_1 = new Button();
node_1.Text = "Enter";
root_0.AddChild(node_1);
var root_2 = node_1;
}
}
