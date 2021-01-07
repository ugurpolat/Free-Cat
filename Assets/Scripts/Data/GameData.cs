using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// The Data Model for platform game data
/// </summary>
[Serializable]
public class GameData 
{
    public int coinCount;
    public int score;
    public int lives;
    public bool[] keyFound;
    public LevelData[] levelData; //for tracking level data like level unlocked, stars awarded, level number
    public bool isFirstBoot;

    public bool playSound;
    public bool playMusic;
}
