using UnityEditor;
using UnityEngine;
using System.Linq;

public class TeamKitAssigner
{
    [MenuItem("Tools/Assign Team Kits Automatically")]
    public static void AssignKits()
    {
        string teamPath = "Assets/DATABASES/team_data/teams_info"; // Path to your Team SOs
        string kitPath = "Assets/DATABASES/kittextures"; // Path to logo sprites

        string[] teamGUIDs = AssetDatabase.FindAssets("t:Teams", new[] { teamPath });
        string[] textureGUIDs = AssetDatabase.FindAssets("t:Texture", new[] { kitPath });

        var allSprites = textureGUIDs
            .Select(guid => AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToDictionary(s => s.name);

        int count = 0;

        foreach (string guid in teamGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var team = AssetDatabase.LoadAssetAtPath<Teams>(path);

            string key = team.HomeKitID.ToString();

            if (allSprites.TryGetValue(key, out var kittexture))
            {
                team.HomeKit = kittexture;
                EditorUtility.SetDirty(team);
                count++;
            }
            else
            {
                Debug.LogWarning($"No matching kit found for team: {team.Name} (ID: {team.ID})");
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Assigned {count} kits to teams.");
    }
}
