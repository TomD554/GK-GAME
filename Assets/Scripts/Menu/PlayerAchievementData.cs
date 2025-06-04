using System;
using System.Collections.Generic;

[Serializable]
public class PlayerAchievementData
{
    public int totalSaves;
    public int matchesPlayed;
    public int cleanSheets;
    public int matchesWon;

    public List<AchievementStatus> achievementsClaimed = new();
}

[Serializable]
public class AchievementStatus
{
    public string id;
    public bool claimed;
}
