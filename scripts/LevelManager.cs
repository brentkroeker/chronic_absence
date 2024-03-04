using Godot;
using Godot.Collections;
using System;

public partial class LevelManager : Node2D
{
	// Get current level name
	// Get level solution
	private Vector2 originSolution;
	private Vector2 scaleSolution;
	private float rotationSolution;
	private float skewSolution;

	// Get level object
	private Polygon2D shape;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadSolution();
		shape = GetNode<Polygon2D>("Shape_User");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (FoundSolution())
		{
			GD.Print("Solution Found");
			((ShapeController)shape).disableControls = true;
		}
	}

	private bool FoundSolution()
	{
		Transform2D shapeTransform = this.shape.Transform;

		return shapeTransform.Origin == this.originSolution
				&& shapeTransform.Scale == this.scaleSolution
				&& shapeTransform.Rotation == this.rotationSolution
				&& shapeTransform.Skew == this.skewSolution;

    }

	private void LoadSolution()
	{
		string jsonText = FileAccess.GetFileAsString("res://solutions.json");
		var solutions = Json.ParseString(jsonText);

		if (solutions.GetType() is not null)
		{
			Dictionary solution = (Dictionary)((Dictionary)solutions)[this.Name];
			Dictionary origin = (Dictionary)solution["origin"];
			Dictionary scale = (Dictionary)solution["scale"];

			this.originSolution = new Vector2((float)origin["x"], (float)origin["y"]);
			this.scaleSolution = new Vector2((float)scale["x"], (float)scale["y"]);
			this.rotationSolution = (float)solution["scale"];
			this.skewSolution = (float)solution["skew"];
		}
		else
		{
			throw new Exception("LevelManager::GetSolution()::JSON Parse Error");
		}
	}
}
