using Godot;
using System;

public partial class LevelUtility : Node
{
    private static bool loadingResource = false;
    private static string lastResourcePath = "";

    public static readonly string[] ORDER =
    {
        "level_01",
        "level_02",
        "level_03",
        "level_04",
        "level_05"
    };

    private static string GetLevelPath(string levelName)
    {
        return $"res://levels/{levelName}.tscn";
    }

    public static Level LoadLevel(string levelName)
    {
        return (Level)GD.Load<PackedScene>(GetLevelPath(levelName)).Instantiate();
    }

    public static bool LoadThreadedLevel(string levelName)
    {
        bool successful = false;

        if (!loadingResource && GetLoadStatus() != ResourceLoader.ThreadLoadStatus.InProgress)
        {
            string levelPath = GetLevelPath(levelName);

            ResourceLoader.LoadThreadedRequest(levelPath, "", true);

            successful = true;
            loadingResource = true;
            lastResourcePath = levelPath;
        }

        return successful;
    }

    public static Level GetLoadedLevel()
    {
        Level level;

        if (GetLoadStatus() == ResourceLoader.ThreadLoadStatus.Loaded)
        {
            level = (Level)((PackedScene)ResourceLoader.LoadThreadedGet(lastResourcePath)).Instantiate();
            lastResourcePath = "";
        }
        else
        {
            level = new Level();
        }

        loadingResource = false;

        return level;
    }

    public static string GetNextLevelName(string currentLevelName)
    {
        int nextLevelIndex = Array.IndexOf(ORDER, currentLevelName) + 1;

        return nextLevelIndex == ORDER.Length ? "N/A" : ORDER[nextLevelIndex];
    }

    public static ResourceLoader.ThreadLoadStatus GetLoadStatus()
    {
        return lastResourcePath != "" ? ResourceLoader.LoadThreadedGetStatus(lastResourcePath)
                                      : ResourceLoader.ThreadLoadStatus.InvalidResource;
    }
}
