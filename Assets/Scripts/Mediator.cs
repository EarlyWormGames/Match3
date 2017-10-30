using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mediator
{
    public static GameSettings Settings;
}

public class GameSettings
{
    /// <summary>
    /// How many potential chains will spawn at the start of the game
    /// </summary>
    public int RequiredChain = 4;

    /// <summary>
    /// <para>How likely the game will spawn a new tile based on the most popular colour.</para>
    /// -1 to make it more likely to spawn different colours
    /// </summary>
    public int ColourChance = 1;

    public int GridWidth = 7;
    public int GridHeight = 9;

    public int TargetScore = 100;
    public float DifficultyMult = 1;

    public int Turns = 15;
    public int Level = -1;
}
