using Godot;
using System;

public partial class CircleWipe : Node2D
{
    public Vector2 position;
    public Color color;
    public float radius;
    public bool drawCircleWipe;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        drawCircleWipe = false;
        radius = 0;
	}

    public override void _Draw()
    {
        if (drawCircleWipe)
        {
            DrawCircle(position, radius, color);
        }
    }
}
