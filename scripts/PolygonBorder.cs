using Godot;
using System;

public partial class PolygonBorder : Polygon2D
{
	[Export]
	public Color BorderColor = Colors.Black;
	[Export]
	public float BorderWidth = 15.0f;

    public override void _Ready()
    {
        SetRotationPoint();
    }

    public override void _Draw()
    {
        //SetRotationPoint();

		Vector2 offset = new Vector2(-BorderWidth / 2, -BorderWidth / 2);
		float scale = 1 + BorderWidth / 100;

		Vector2[] points = new Vector2[this.Polygon.Length + 1];
        for (int i = 0; i < this.Polygon.Length; i++)
		{
			points[i] = this.Polygon[i] * scale  + offset + this.Offset;

            points[i].X = Mathf.Lerp(points[i].X, this.Polygon[i].X + this.Offset.X, 0.27f);
            points[i].Y = Mathf.Lerp(points[i].Y, this.Polygon[i].Y + this.Offset.Y, 0.27f);
        }
		points[this.Polygon.Length] = points[0];
		DrawPolyline(points, BorderColor, BorderWidth, true);
	}

    private void SetRotationPoint()
    {
        Vector2 averagePoint = new Vector2();

        foreach (Vector2 point in this.Polygon)
        {
            averagePoint += point;
        }

        averagePoint /= -this.Polygon.Length;

        this.Offset = averagePoint;
        this.Position -= this.Offset;

        QueueRedraw();

        GD.Print("Offset: " + this.Offset);
    }
}
