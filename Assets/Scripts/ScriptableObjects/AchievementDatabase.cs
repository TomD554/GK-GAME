using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AchievementDatabase", menuName = "Game/Achievement Database")]
public class AchievementDatabase : ScriptableObject
{
    public List<Achievements> allAchievements;
}
