using UnityEditor;
using UnityEngine;
using System.Linq;

public class TeamLogoAssigner
{
    [MenuItem("Tools/Assign Team Logos Automatically")]
    public static void AssignLogos()
    {
        string teamPath = "Assets/DATABASES/team_data/teams_info"; // Path to your Team SOs
        string logoPath = "Assets/DATABASES/team_data/logos"; // Path to logo sprites

        string[] teamGUIDs = AssetDatabase.FindAssets("t:Teams", new[] { teamPath });
        string[] spriteGUIDs = AssetDatabase.FindAssets("t:Sprite", new[] { logoPath });

        var allSprites = spriteGUIDs
            .Select(guid => AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToDictionary(s => s.name);

        int count = 0;

        foreach (string guid in teamGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var team = AssetDatabase.LoadAssetAtPath<Teams>(path);

            string key = team.ID.ToString(); // Or team.TeamName if matching by name

            if (allSprites.TryGetValue(key, out var sprite))
            {
                team.Logo = sprite;
                EditorUtility.SetDirty(team);
                count++;
            }
            else
            {
                Debug.LogWarning($"No matching logo found for team: {team.Name} (ID: {team.ID})");
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Assigned {count} logos to teams.");
    }
}
