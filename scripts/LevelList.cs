using Godot;
using System;
using System.Collections.Generic;

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

    public static Resource GetNextLevel(string currentLevelName)
    {
        int nextLevelIndex = Array.IndexOf(ORDER, currentLevelName) + 1;
        string nextLevel = $"{ORDER[nextLevelIndex]}.tscn";

        return GD.Load(nextLevel);
    }
}
