using Godot;

public partial class MyContainer
{
	public MyContainer(Control place1)
	{
		this.Ready += () =>
		{
			this.editor1.ReplaceBy(place1);
		};
	}
}
