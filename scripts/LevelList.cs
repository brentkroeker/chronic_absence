using Godot;
using System;

public partial class LevelList : Node
{
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

    public static LevelManager GetNextLevel(string currentLevelName)
    {
        int nextLevelIndex = Array.IndexOf(ORDER, currentLevelName) + 1;
        string nextLevel = $"{ORDER[nextLevelIndex]}.tscn";

        return GD.Load<LevelManager>(nextLevel);
    }

    public static LevelManager GetLevel(string levelName)
    {
        int levelIndex = Array.IndexOf(ORDER, levelName);
        string level = $"{ORDER[levelIndex]}.tscn";

        return GD.Load<LevelManager>(level);
    }
}
