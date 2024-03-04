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
		float positionErrrorMargin = 2;
		float rotationErrorMargin = 0.01f;
		float scaleErrorMargin = 0.05f;
		float skewErrorMargin = 0.005f;

        bool originMatches = shapeTransform.Origin.X >= this.originSolution.X - positionErrrorMargin
						  && shapeTransform.Origin.X <= this.originSolution.X + positionErrrorMargin
						  && shapeTransform.Origin.Y >= this.originSolution.Y - positionErrrorMargin
						  && shapeTransform.Origin.Y <= this.originSolution.Y + positionErrrorMargin;

		bool rotationMatches = shapeTransform.Rotation / Mathf.Tau >= this.rotationSolution - rotationErrorMargin
                            && shapeTransform.Rotation / Mathf.Tau <= this.rotationSolution + rotationErrorMargin;

		bool scaleMatches = shapeTransform.Scale.X >= this.scaleSolution.X - scaleErrorMargin
						 && shapeTransform.Scale.X <= this.scaleSolution.X + scaleErrorMargin
						 && shapeTransform.Scale.Y >= this.scaleSolution.Y - scaleErrorMargin
						 && shapeTransform.Scale.Y <= this.scaleSolution.Y + scaleErrorMargin;

		bool skewMatches = shapeTransform.Skew / Mathf.Tau >= this.skewSolution - skewErrorMargin
						&& shapeTransform.Skew / Mathf.Tau <= this.skewSolution + skewErrorMargin;

		return originMatches
			&& scaleMatches
			&& rotationMatches
			&& skewMatches;
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
