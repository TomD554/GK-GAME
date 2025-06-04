using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class CreateAchievements
{
    private static string AchievementsCSVPath = "/DATABASES/achievements/Achievements.csv";

    [MenuItem("Utilities/Generate Achievements")]

    public static void GenerateAchievements()
    {
        string[] alllines = File.ReadAllLines(Application.dataPath + AchievementsCSVPath);

        for (int i = 1; i < alllines.Length; i++) // start from 1 to skip header
        {
            string s = alllines[i];
            string[] splitData = s.Split(',');

            Achievements achievement = ScriptableObject.CreateInstance<Achievements>();
            achievement.ID = splitData[0];
            achievement.Name = splitData[1];
            achievement.description = splitData[2];
            achievement.type = (Achievements.AchievementType)System.Enum.Parse(typeof(Achievements.AchievementType), splitData[3]);
            achievement.requiredValue = int.Parse(splitData[4]);
            achievement.coinReward = int.Parse(splitData[5]);

            AssetDatabase.CreateAsset(achievement, $"Assets/DATABASES/achievements/{achievement.type}/{achievement.ID}.asset");
        }
        AssetDatabase.SaveAssets();
    }

}