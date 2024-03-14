using Godot;

public partial class ShapeController : Polygon2D
{
    private float changeRate = 0.02f;
    private float positionXChangeRate = 0;
    private float positionYChangeRate = 0;
    private float rotationChangeRate = 0;
    private float scaleXChangeRate = 0;
    private float scaleYChangeRate = 0;
    private float skewXChangeRate = 0;
    private float skewYChangeRate = 0;

    public bool disableControls = false;

    public ShapeControls ShapeControls
    {
        get;
        set;
    }

    public override void _Ready()
	{
        ShapeControls = GetParent().GetNode<ShapeControls>("ShapeControls");
        ShapeControls.Ready += ShapeControls_Ready;
    }

    private void SetRotationPoint()
    {
        Vector2 averagePoint = new Vector2();

        foreach (Vector2 point in this.Polygon)
        {
            averagePoint += point;
        }

        averagePoint /=  -this.Polygon.Length;

        this.Offset = averagePoint;
        Transform2D transform = this.Transform;
        transform.Origin -= this.Offset;
        this.Transform = transform;
    }

    private void ResetChangeRates()
    {
        positionXChangeRate = 0;
        positionYChangeRate = 0;
        rotationChangeRate = 0;
        scaleXChangeRate = 0;
        scaleYChangeRate = 0;
        skewXChangeRate = 0;
        skewYChangeRate = 0;
    }

    public void ShapeControls_Ready()
    {
        ShapeControls.SldrPositionX.ValueChanged += SldrPositionX_ValueChanged;
        ShapeControls.SldrPositionY.ValueChanged += SldrPositionY_ValueChanged;

        ShapeControls.SldrRotation.ValueChanged += SldrRotation_ValueChanged;

        ShapeControls.SldrScaleX.ValueChanged += SldrScaleX_ValueChanged;
        ShapeControls.SldrScaleY.ValueChanged += SldrScaleY_ValueChanged;

        ShapeControls.SldrSkewX.ValueChanged += SldrSkewX_ValueChanged;
        ShapeControls.SldrSkewY.ValueChanged += SldrSkewY_ValueChanged;

        ShapeControls.SldrPositionX.DragEnded += Slider_DragEnded;
        ShapeControls.SldrPositionY.DragEnded += Slider_DragEnded;
        ShapeControls.SldrRotation.DragEnded += Slider_DragEnded;
        ShapeControls.SldrScaleX.DragEnded += Slider_DragEnded;
        ShapeControls.SldrScaleY.DragEnded += Slider_DragEnded;
        ShapeControls.SldrSkewX.DragEnded += Slider_DragEnded;
        ShapeControls.SldrSkewY.DragEnded += Slider_DragEnded;
    }

    private void Slider_DragEnded(bool valueChanged)
    {
        ResetChangeRates();
    }

    private void SldrSkewY_ValueChanged(double value)
    {
        if (ShapeControls.IsDragging)
        {
            skewYChangeRate = (float)value;
        }
    }

    private void SldrSkewX_ValueChanged(double value)
    {
        if (ShapeControls.IsDragging)
        {
            skewXChangeRate = (float)value;
        }
    }

    private void SldrScaleY_ValueChanged(double value)
    {
        if (ShapeControls.IsDragging)
        {
            scaleYChangeRate = 1 + (float)value / 100;
        }
    }

    private void SldrScaleX_ValueChanged(double value)
    {
        if (ShapeControls.IsDragging)
        {
            scaleXChangeRate = 1 + (float)value / 100;
        }
    }

    private void SldrRotation_ValueChanged(double value)
    {
        if (ShapeControls.IsDragging)
        {
            float steps = (float)ShapeControls.SldrRotation.MaxValue / (float)ShapeControls.SldrRotation.Step;
            float tauDenomination = Mathf.Lerp(2000 - Mathf.Abs((float)value) * steps * 4, 0, 0.25f);

            rotationChangeRate = value == 0 ? (float)value : Mathf.Tau / tauDenomination * Mathf.Sign(value);
        }
    }

    private void SldrPositionY_ValueChanged(double value)
    {
        if (ShapeControls.IsDragging)
        {
            positionYChangeRate = (float)value * 1.25f;
        }
    }

    private void SldrPositionX_ValueChanged(double value)
    {
        if (ShapeControls.IsDragging)
        {
            positionXChangeRate = (float)value * 1.25f;
        }
    }

    public override void _Process(double delta)
	{
        Transform2D transform = this.Transform;

        if (ShapeControls.IsDragging && !disableControls)
        {
            if (rotationChangeRate != 0 && !Mathf.IsInf(rotationChangeRate))
            {
                float rotation = transform.Rotation + rotationChangeRate;

                transform = new Transform2D(rotation, transform.Scale, transform.Skew, transform.Origin);
            }

            if (positionXChangeRate != 0)
            {
                transform.Origin.X += positionXChangeRate;
            }
            if (positionYChangeRate != 0)
            {
                transform.Origin.Y -= positionYChangeRate;
            }

            if (scaleXChangeRate != 0)
            {
                transform.X *= scaleXChangeRate;

            }
            if (scaleYChangeRate != 0)
            {
                transform.Y *= scaleYChangeRate;

            }

            if (skewXChangeRate != 0)
            {
                transform.Y.X += skewXChangeRate;
            }
            if (skewYChangeRate != 0)
            {
                transform.X.Y += skewYChangeRate;
            }
        }

        // Not exceeding boundaries
        float viewportWidth = GetViewportRect().Size.X;
        float viewportHeight = GetViewportRect().Size.Y;

        if (transform.Origin.X >= viewportWidth || transform.Origin.X <= 0 ||
            transform.Origin.Y >= viewportHeight || transform.Origin.Y <= 0)
        {
            this.disableControls = true;

            if (transform.Origin.X>= viewportWidth)
            {
                transform.Origin.X = Mathf.MoveToward(transform.Origin.X, viewportWidth, (float)delta * 1000);
            }
            if (transform.Origin.X <= 0)
            {
                transform.Origin.X = Mathf.MoveToward(transform.Origin.X, 0, (float)delta * 1000);
            }
            if (transform.Origin.Y >= viewportHeight)
            {
                transform.Origin.Y = Mathf.MoveToward(transform.Origin.Y, viewportHeight, (float)delta * 1000);
            }
            if (transform.Origin.Y <= 0)
            {
                transform.Origin.Y = Mathf.MoveToward(transform.Origin.Y, 0, (float)delta * 1000);
            }

            //transform = this.Transform;
        }


        if (false && !disableControls)
        {
            // Position
            if (Input.IsActionPressed("debug_pos_left"))
		    {
			    transform.Origin.X--;
		    }
            if (Input.IsActionPressed("debug_pos_right"))
            {
                transform.Origin.X++;
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
