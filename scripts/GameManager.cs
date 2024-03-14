using Godot;

public partial class GameManager : Node
{
	private Level currentLevel;
	private ShapeControls shapeControls;
	private bool loadingNextLevel;
	private bool gameWon;

	Polygon2D shapeAbsence;
    Polygon2D shapeBackground;
    Polygon2D shapeBorder;
    Polygon2D shapeUser;

    public ShapeControls ShapeControls
	{
		get => shapeControls;
		private set => shapeControls = value;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameWon = false;
        loadingNextLevel = false;

		Button beginButton = GetNode<Button>("menu_main/Begin_Button");

        beginButton.ButtonUp += BeginButton_ButtonUp;
    }

    private void BeginButton_ButtonUp()
    {
		BeginGame();
    }

    private void CurrentLevel_SolutionFound()
    {
		string nextLevelName = LevelUtility.GetNextLevelName(currentLevel.Name);

		GD.Print("Last Level Name: " + LevelUtility.ORDER[LevelUtility.ORDER.Length - 1]);

		if (nextLevelName != "N/A" && currentLevel.Name != LevelUtility.ORDER[LevelUtility.ORDER.Length - 1])
		{
			bool threadedLoadSuccessful = LevelUtility.LoadThreadedLevel(nextLevelName);

			loadingNextLevel = true;
		}
		else
		{
			gameWon = true;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (gameWon && currentLevel != null && !currentLevel.AnimatingEnding)
		{
			Control endScreen = (Control)GD.Load<PackedScene>("res://end_screen.tscn").Instantiate();
			AddChild(endScreen);
			DisposePreviousLevel(currentLevel);

            endScreen.GetNode<Button>("Begin_Button").ButtonUp += BeginButton_ButtonUp;
            endScreen.GetNode<Button>("Exit_Button").ButtonUp += ExitButton_ButtonUp;

            gameWon = false;
        }

		if (loadingNextLevel && LevelUtility.GetLoadStatus() == ResourceLoader.ThreadLoadStatus.Loaded &&
			!currentLevel.AnimatingEnding)
		{
			SetupNextLevel();
        }
	}

    public override void _UnhandledInput(InputEvent @event)
    {
		if (@event.IsActionPressed("ui_cancel"))
		{
			GetTree().Quit();
		}
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
		{
			GetTree().Quit();
		}
    }

    private void ExitButton_ButtonUp()
    {
		GetTree().Quit();
    }

    private void BeginGame()
	{
        currentLevel = LevelUtility.LoadLevel(LevelUtility.ORDER[0]);
        ShapeControls = currentLevel.GetNode<ShapeControls>("ShapeControls");

        GetCurrentLevelShapes();

        this.AddChild(currentLevel);
        currentLevel.SolutionFound += CurrentLevel_SolutionFound;
    }

	private void SetupNextLevel()
	{
		Color previousColor = shapeUser.Color;
		Color previousBackgroundColor = shapeBackground.Color;
		Level previousLevel = currentLevel;

        ShapeControls = currentLevel.GetNode<ShapeControls>("ShapeControls");
        currentLevel = LevelUtility.GetLoadedLevel();

		GetCurrentLevelShapes();

		shapeBackground.Color = previousColor;
		shapeAbsence.Color = previousBackgroundColor;

		currentLevel.SolutionFound += CurrentLevel_SolutionFound;

		this.AddChild(currentLevel);
		DisposePreviousLevel(previousLevel);

		loadingNextLevel = false;
	}

	private void DisposePreviousLevel(Level previousLevel)
	{
        this.RemoveChild(previousLevel);
        previousLevel.QueueFree();
    }

	private void GetCurrentLevelShapes()
	{
		shapeAbsence = currentLevel.GetNode<Polygon2D>("Shape_Absence");
        shapeBackground = currentLevel.GetNode<Polygon2D>("Shape_Background");
        shapeUser = currentLevel.GetNode<Polygon2D>("Shape_User");
    }
}
