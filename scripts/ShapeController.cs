using Godot;
using System;
using System.Transactions;

public partial class ShapeController : Polygon2D
{
    private float changeRate = 0.02f;

    public bool disableControls = false;

    public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
        Transform2D transform = this.Transform;

        if (!disableControls)
        {
            // Position
            if (Input.IsActionPressed("debug_pos_left"))
		    {
			    transform.Origin.X--;
                //GD.Print(transform.Origin);
		    }
            if (Input.IsActionPressed("debug_pos_right"))
            {
                transform.Origin.X++;
                //GD.Print(transform.Origin);
            }
            if (Input.IsActionPressed("debug_pos_up"))
            {
                transform.Origin.Y--;
            }
            if (Input.IsActionPressed("debug_pos_down"))
            {
                transform.Origin.Y++;
            }

            // Rotation
            if (Input.IsActionPressed("debug_rot_clockwise"))
            {
                transform = this.RotateShape(transform, true);
            }
            if (Input.IsActionPressed("debug_rot_counter"))
            {
                transform = this.RotateShape(transform, false);
            }
        
            // Scale
            if (Input.IsActionPressed("debug_scale_x_up"))
            {
                transform = this.ScaleShape(transform, true, true);
            }
            if (Input.IsActionPressed("debug_scale_x_down"))
            {
                transform = this.ScaleShape(transform, true, false);
            }
            if (Input.IsActionPressed("debug_scale_y_up"))
            {
                transform = this.ScaleShape(transform, false, true);
            }
            if (Input.IsActionPressed("debug_scale_y_down"))
            {
                transform = this.ScaleShape(transform, false, false);
            }

            // Skew
            if (Input.IsActionPressed("debug_skew_x_up"))
            {
                transform = this.SkewShape(transform, true, true);
            }
            if (Input.IsActionPressed("debug_skew_x_down"))
            {
                transform = this.SkewShape(transform, true, false);
            }
            if (Input.IsActionPressed("debug_skew_y_up"))
            {
                transform = this.SkewShape(transform, false, true);
            }
            if (Input.IsActionPressed("debug_skew_y_down"))
            {
                transform = this.SkewShape(transform, false, false);
            }
        }

        this.Transform = transform;
    }

    private Transform2D RotateShape(Transform2D transform, bool clockwise)
    {
        float rotation = clockwise ? transform.Rotation + Mathf.Tau / 750 : transform.Rotation - Mathf.Tau / 750;

        Transform2D tempTransform = new Transform2D(rotation, transform.Scale, transform.Skew, transform.Origin);

        return tempTransform;
    }

    private Transform2D ScaleShape(Transform2D transform, bool scaleX, bool scaleUp)
    {
        // TO-DO: Figure out how to dynamically clamp vectors

        float scaleRate = 1 + this.changeRate * (scaleUp ? 1 : -1);

        if (scaleX)
        {
            transform.X *= scaleRate;
        }
        else
        {
            transform.Y *= scaleRate;
        }

        return transform;
    }

    private Transform2D SkewShape(Transform2D transform, bool skewX, bool skewUp)
    {
        float skewRate = this.changeRate * (skewUp ? 1 : -1);

        if (skewX)
        {
            transform.Y.X += skewRate;
        }
        else
        {
            transform.X.Y += skewRate;
        }

        return transform;
    }
}
