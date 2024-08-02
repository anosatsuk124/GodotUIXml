using Godot;

public partial class Main : Control
{
	public override void _Ready()
	{
		var container = new MyContainer(new MyEditor());

		AddChild(container);
	}
}
