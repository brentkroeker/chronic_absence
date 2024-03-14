using Godot;

public partial class ShapeControls : Control
{
	private Slider sliderResetting;

	public HSlider SldrPositionX { get; private set; }
	public VSlider SldrPositionY { get; private set; }

    public HSlider SldrRotation { get; private set; }

    public HSlider SldrScaleX { get; private set; }
    public VSlider SldrScaleY { get; private set; }

    public HSlider SldrSkewX { get; private set; }
    public VSlider SldrSkewY { get; private set; }

    public bool IsDragging { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		SldrPositionX = GetNode<HSlider>("pnlPosition/sldrPositionX");
		SldrPositionY = GetNode<VSlider>("pnlPosition/sldrPositionY");

		SldrRotation = GetNode<HSlider>("pnlRotation/sldrRotation");

		SldrScaleX = GetNode<HSlider>("pnlScale/sldrScaleX");
        SldrScaleY = GetNode<VSlider>("pnlScale/sldrScaleY");

        SldrSkewX = GetNode<HSlider>("pnlSkew/sldrSkewX");
        SldrSkewY = GetNode<VSlider>("pnlSkew/sldrSkewY");

        ResetSliders();

        IsDragging = false;

        SldrPositionX.DragStarted += Slider_DragStarted;
        SldrPositionY.DragStarted += Slider_DragStarted;
        SldrRotation.DragStarted += Slider_DragStarted;
        SldrScaleX.DragStarted += Slider_DragStarted;
        SldrScaleY.DragStarted += Slider_DragStarted;
        SldrSkewX.DragStarted += Slider_DragStarted;
        SldrSkewY.DragStarted += Slider_DragStarted;

        SldrPositionX.DragEnded += Slider_DragEnded;
        SldrPositionY.DragEnded += Slider_DragEnded;
        SldrRotation.DragEnded += Slider_DragEnded;
        SldrScaleX.DragEnded += Slider_DragEnded;
        SldrScaleY.DragEnded += Slider_DragEnded;
        SldrSkewX.DragEnded += Slider_DragEnded;
        SldrSkewY.DragEnded += Slider_DragEnded;

        SldrPositionX.ValueChanged += Slider_ValueChanged;
        SldrPositionY.ValueChanged += Slider_ValueChanged;
        SldrRotation.ValueChanged += Slider_ValueChanged;
        SldrScaleX.ValueChanged += Slider_ValueChanged;
        SldrScaleY.ValueChanged += Slider_ValueChanged;
        SldrSkewX.ValueChanged += Slider_ValueChanged;
        SldrSkewY.ValueChanged += Slider_ValueChanged;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (sliderResetting is not null)
        {
            sliderResetting.ReleaseFocus();

            //sliderResetting.SetValueNoSignal(Mathf.MoveToward(sliderResetting.Value, 0, delta * 10));
            sliderResetting.Value = Mathf.MoveToward(sliderResetting.Value, 0, delta * 10);

            if (sliderResetting.Value == 0)
            {
                sliderResetting = null;
                IsDragging = false;
            }
        }
    }

    public void ResetSliders()
    {
        SldrPositionX.Value = 0;
        SldrPositionY.Value = 0;
        SldrRotation.Value = 0;
        SldrScaleX.Value = 0;
        SldrScaleY.Value = 0;
        SldrSkewX.Value = 0;
        SldrSkewY.Value = 0;
    }

    public void ToggleDisableSliders()
    {
        MouseFilterEnum mouseFilter = SldrPositionX.Editable ? MouseFilterEnum.Ignore : MouseFilterEnum.Stop;

        SldrPositionX.MouseFilter = mouseFilter;
        SldrPositionY.MouseFilter = mouseFilter;
        SldrRotation.MouseFilter = mouseFilter;
        SldrScaleX.MouseFilter = mouseFilter;
        SldrScaleY.MouseFilter = mouseFilter;
        SldrSkewX.MouseFilter = mouseFilter;
        SldrSkewY.MouseFilter = mouseFilter;

        SldrPositionX.Editable = !SldrPositionX.Editable;
        SldrPositionY.Editable = !SldrPositionY.Editable;
        SldrRotation.Editable = !SldrRotation.Editable;
        SldrScaleX.Editable = !SldrScaleX.Editable;
        SldrScaleY.Editable = !SldrScaleY.Editable;
        SldrSkewX.Editable = !SldrSkewX.Editable;
        SldrSkewY.Editable = !SldrSkewY.Editable;
    }

    private void Slider_ValueChanged(double value)
    {
        if (!IsDragging)
        {
            ResetSliders();
        }
    }

    private void Slider_DragEnded(bool valueChanged)
    {
        if (valueChanged)
        {
            if (SldrPositionX.Value != 0)
            {
                sliderResetting = SldrPositionX;
            }
            else if (SldrPositionY.Value != 0)
            {
                sliderResetting = SldrPositionY;
            }
            else if (SldrRotation.Value != 0)
            {
                sliderResetting = SldrRotation;
            }
            else if (SldrScaleX.Value != 0)
            {
                sliderResetting = SldrScaleX;
            }
            else if (SldrScaleY.Value != 0)
            {
                sliderResetting = SldrScaleY;
            }
            else if (SldrSkewX.Value != 0)
            {
                sliderResetting = SldrSkewX;
            }
            else if (SldrSkewY.Value != 0)
            {
                sliderResetting = SldrSkewY;
            }
        }
        else
        {
        }
            IsDragging = false;
    }

    private void Slider_DragStarted()
    {
        IsDragging = true;
    }
}
