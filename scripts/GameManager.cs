using Godot;

public partial class GameManager : Node
{
	private Level currentLevel;
	private ShapeControls shapeControls;

	public ShapeControls ShapeControls
	{
		get => shapeControls;
		private set => shapeControls = value;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentLevel = LevelUtility.LoadLevel("level_01");
		shapeControls = (ShapeControls)GD.Load<PackedScene>("res://shape_controls.tscn").Instantiate();

		this.AddChild(currentLevel);
		this.AddChild(shapeControls);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
