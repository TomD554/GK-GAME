using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAchievement", menuName = "Achievement")]
public class Achievements : ScriptableObject
{
    [Header("Basic Info")]
    public string ID;
    public string Name;
    [TextArea] public string description;


    [Header("Tracking")]
    public AchievementType type;     // What stat are we tracking?
    public int requiredValue;

    [Header("Rewards")]
    public int coinReward;

    public enum AchievementType
    {
        TotalSaves,
        MatchesPlayed,
        CleanSheets,
        MatchesWon,
        // Add others as needed
    }
}



