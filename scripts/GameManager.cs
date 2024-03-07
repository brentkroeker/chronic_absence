using Godot;

public partial class GameManager : Node
{
	LevelManager currentLevel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentLevel = LevelList.GetLevel("level_01");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
