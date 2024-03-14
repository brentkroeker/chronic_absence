using Godot;
using Godot.Collections;
using System;

public partial class Level : Node2D
{
	[Signal]
	public delegate void SolutionFoundEventHandler();

	private Polygon2D shape;
	private Polygon2D shapeAbsence;
	private Polygon2D shapeBackground;
	private CircleWipe circleShape;

	private float circleRadius = 0;
	private int animationStage = 0;

	private bool solutionSkewed;
	private bool levelWon;

	public Vector2 OriginSolution
	{
		get;
		private set;
	}

	public Vector2 XSolution
	{
		get;
		private set;
	}

	public Vector2 YSolution
	{
		get;
		private set;
	}

	public bool AnimatingEnding
	{
		get;
		private set;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shape = GetNode<Polygon2D>("Shape_User");
		shapeAbsence = GetNode<Polygon2D>("Shape_Absence");
		shapeBackground = GetNode<Polygon2D>("Shape_Background");

		solutionSkewed = shapeAbsence.Skew != 0;

		SetRotationPoint(shape);

		if (!solutionSkewed)
		{
			SetRotationPoint(shapeAbsence);
			SetRotationPoint(shapeBackground);
		}

		LoadSolution();

		AnimatingEnding = false;
		levelWon = false;

        this.SolutionFound += Level_SolutionFound;
    }

    private void Level_SolutionFound()
    {
        ((ShapeController)shape).disableControls = true;
        AnimatingEnding = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (animationStage == 0)
		{
			FoundSolution();
		}

		if (AnimatingEnding)
		{
			AnimateLevelEnding(delta);
		}
	}

    private void FoundSolution()
	{
		Transform2D shapeTransform = this.shape.Transform;
		float originErrorMargin = solutionSkewed ? 32 : 2;
		float errorMargin = 0.05f;

		float shapeOriginX = this.shape.Transform.Origin.X;
        float shapeOriginY = this.shape.Transform.Origin.Y;

		float shapeXX = this.shape.Transform.X.X;
        float shapeXY = this.shape.Transform.X.Y;

        float shapeYX = this.shape.Transform.Y.X;
        float shapeYY = this.shape.Transform.Y.Y;

		if (solutionSkewed)
		{
			shapeOriginX = Mathf.Abs(shapeOriginX);
            shapeOriginY = Mathf.Abs(shapeOriginY);
        }
		shapeXX = Mathf.Abs(shapeXX);
		shapeXY = Mathf.Abs(shapeXY);

		shapeYX = Mathf.Abs(shapeYX);
		shapeYY = Mathf.Abs(shapeYY);

		bool originMatches = shapeOriginX >= OriginSolution.X - originErrorMargin
                          && shapeOriginX <= OriginSolution.X + originErrorMargin
                          && shapeOriginY >= OriginSolution.Y - originErrorMargin
                          && shapeOriginY <= OriginSolution.Y + originErrorMargin;

		bool xMatches = shapeXX >= XSolution.X - errorMargin
					 && shapeXX <= XSolution.X + errorMargin
					 && shapeXY >= XSolution.Y - errorMargin
					 && shapeXY <= XSolution.Y + errorMargin;

		bool yMatches = shapeYX >= YSolution.X - errorMargin
					 && shapeYX <= YSolution.X + errorMargin
					 && shapeYY >= YSolution.Y - errorMargin
					 && shapeYY <= YSolution.Y + errorMargin;

		if (originMatches && xMatches && yMatches && !levelWon)
		{
			EmitSignal(SignalName.SolutionFound);
			levelWon = true;
		}
    }

    private void AnimateLevelEnding(double delta)
    {
		switch (animationStage)
		{
			case 0:
                Transform2D transform = shape.Transform;
				float morphSpeed = (float)delta * 5;

				if(!solutionSkewed)
				{
					transform.Origin.X = Mathf.MoveToward(transform.Origin.X, OriginSolution.X, morphSpeed);
					transform.Origin.Y = Mathf.MoveToward(transform.Origin.Y, OriginSolution.Y, morphSpeed);
				}

				if (transform.X.X < 0 && XSolution.X < 0 || transform.X.X > 0 && XSolution.X > 0)
				{
					transform.X.X = Mathf.MoveToward(transform.X.X, XSolution.X, morphSpeed);
				}
				else if (transform.X.X < 0)
				{
					transform.X.X = Mathf.MoveToward(transform.X.X, XSolution.X * -1, morphSpeed);
				}
				else
				{
                    transform.X.X = Mathf.MoveToward(transform.X.X, Mathf.Abs(XSolution.X), morphSpeed);
                }

				//
                if (transform.X.Y < 0 && XSolution.Y < 0 || transform.X.Y > 0 && XSolution.Y > 0)
                {
                    transform.X.Y = Mathf.MoveToward(transform.X.Y, XSolution.Y, morphSpeed);
                }
                else if (transform.X.Y < 0)
                {
                    transform.X.Y = Mathf.MoveToward(transform.X.Y, XSolution.Y * -1, morphSpeed);
                }
                else
                {
                    transform.X.Y = Mathf.MoveToward(transform.X.Y, Mathf.Abs(XSolution.Y), morphSpeed);
                }

				//
                if (transform.Y.X < 0 && YSolution.X < 0 || transform.Y.X > 0 && YSolution.X > 0)
                {
                    transform.Y.X = Mathf.MoveToward(transform.Y.X, YSolution.X, morphSpeed);
                }
                else if (Mathf.Sign(transform.Y.X) == -1)
                {
                    transform.Y.X = Mathf.MoveToward(transform.Y.X, YSolution.X * -1, morphSpeed);
                }
                else
                {
                    transform.Y.X = Mathf.MoveToward(transform.Y.X, Mathf.Abs(YSolution.X), morphSpeed);
                }

				//
                if (transform.Y.Y < 0 && YSolution.Y < 0 || transform.Y.Y > 0 && YSolution.Y > 0)
                {
                    transform.Y.Y = Mathf.MoveToward(transform.Y.Y, YSolution.Y, morphSpeed);
                }
                else if (Mathf.Sign(transform.Y.X) == -1)
                {
                    transform.Y.Y = Mathf.MoveToward(transform.Y.Y, Mathf.Abs(YSolution.Y), morphSpeed);
                }
                else
                {
                    transform.Y.Y = Mathf.MoveToward(transform.Y.Y, YSolution.Y * -1, morphSpeed);
                }

				shape.Transform = transform;

				if ((shape.Transform.Origin.X == OriginSolution.X &&
					shape.Transform.Origin.Y == OriginSolution.Y || XSolution.Y != 0|| YSolution.X != 0) &&
					Mathf.Abs(shape.Transform.X.X) == XSolution.X &&
                    Mathf.Abs(shape.Transform.X.Y) == XSolution.Y &&
                    Mathf.Abs(shape.Transform.Y.X) == YSolution.X &&
                    Mathf.Abs(shape.Transform.Y.Y) == YSolution.Y)
				{
					animationStage++;
					circleShape = GetNode<CircleWipe>("Circle_Wipe");
					circleShape.drawCircleWipe = true;
					circleShape.Position = shapeAbsence.Position;
					circleShape.color = shape.Color;
				}
				break;
			case 1:
				if (circleShape.radius != GetViewportRect().Size.X)
				{
					circleShape.radius = Mathf.MoveToward(circleShape.radius, GetViewportRect().Size.X, (float)delta * 750);
					circleShape.QueueRedraw();
				}
				else
				{
                    EmitSignal(SignalName.SolutionFound);
                    circleShape.drawCircleWipe = false;
					AnimatingEnding = false;
				}
				break;
		}
    }

	private void LoadSolution()
	{
		OriginSolution = new Vector2(Mathf.Abs(shapeAbsence.Transform.Origin.X), Mathf.Abs(shapeAbsence.Transform.Origin.Y));
		XSolution = new Vector2(Mathf.Abs(shapeAbsence.Transform.X.X), Mathf.Abs(shapeAbsence.Transform.X.Y));
		YSolution = new Vector2(Mathf.Abs(shapeAbsence.Transform.Y.X), Mathf.Abs(shapeAbsence.Transform.Y.Y));
	}

    private void SetRotationPoint(Polygon2D shape)
    {
        Vector2 averagePoint = new Vector2();
		Transform2D transform = new Transform2D(0, shape.Transform.Scale, shape.Transform.Skew, shape.Transform.Origin);

		float xSkew = shape.Transform.Y.X;
		float ySkew = shape.Transform.X.Y;

		// Apply rotation:
		transform.X.X = Mathf.Cos(shape.Rotation);
		transform.X.Y = -Mathf.Sin(shape.Rotation);
		transform.Y.X = Mathf.Sin(shape.Rotation);
		transform.Y.Y = Mathf.Cos(shape.Rotation);

        foreach (Vector2 point in shape.Polygon)
        {
            averagePoint += point;
        }

		averagePoint /= -shape.Polygon.Length;
        shape.Offset = averagePoint;
		transform.Origin -= shape.Offset * shape.Transform.Scale;

		transform.X *= shape.Transform.Scale.X;
		transform.Y *= shape.Transform.Scale.Y;

		transform.Y.X = xSkew;
		transform.X.Y = ySkew;

		shape.Transform = transform;

        QueueRedraw();
    }
}
