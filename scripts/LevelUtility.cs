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
        "level_05",
        "level_06",
        "level_07",
        "level_08",
        "level_09",
        "level_10"
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

    public static Resource GetLoadedLevel()
    {
        Resource level;

        if (GetLoadStatus() == ResourceLoader.ThreadLoadStatus.Loaded)
        {
            level = ResourceLoader.LoadThreadedGet(lastResourcePath);
            lastResourcePath = "";
        }
        else
        {
            level = new Resource();
        }

        return level;
    }

    public static string GetNextLevelName(string currentLevelName)
    {
        int nextLevelIndex = Array.IndexOf(ORDER, currentLevelName) + 1;
        
        return ORDER[nextLevelIndex];
    }

    public static ResourceLoader.ThreadLoadStatus GetLoadStatus()
    {
        return lastResourcePath != "" ? ResourceLoader.LoadThreadedGetStatus(lastResourcePath)
                                      : ResourceLoader.ThreadLoadStatus.InvalidResource;
    }


    //////////////
    public static Level GetNextLevel(string currentLevelName)
    {
        int nextLevelIndex = Array.IndexOf(ORDER, currentLevelName) + 1;
        string nextLevel = $"res://levels/{ORDER[nextLevelIndex]}.tscn";

        return GD.Load<Level>(nextLevel);
    }

    public static Level GetLevel(string levelName)
    {
        string level = $"res://levels/{levelName}.tscn";

        return GD.Load<Level>(level);
    }
}
