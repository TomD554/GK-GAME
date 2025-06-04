using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class CreateTeamsfromCSV
{
    private static string TeamsCSVPath = "/DATABASES/team_data/Teams.csv";

    [MenuItem("Utilities/Generate Teams")]

    public static void GenerateTeams()
    {
        string[] alllines = File.ReadAllLines(Application.dataPath + TeamsCSVPath);

        foreach (string s in alllines)
        {
            string[] splitData = s.Split(',');

            Teams team = ScriptableObject.CreateInstance<Teams>();
            team.ID = int.Parse(splitData[0]);
            team.Name = splitData[1];
            team.Rating = float.Parse(splitData[2]);
            team.HomeKitID = int.Parse(splitData[3]);
            team.AwayKitID = int.Parse(splitData[3]);
            team.Nation = splitData[4];
            team.Logo = null;

            AssetDatabase.CreateAsset(team, $"Assets/DATABASES/team_data/teams_info/{team.ID}.asset");
        }
        AssetDatabase.SaveAssets();
    }

}