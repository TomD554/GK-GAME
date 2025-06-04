using UnityEditor;
using UnityEngine;
using System.Linq;

public class DatabasePopulator
{
    [MenuItem("Tools/Populate Team Database")]
    public static void PopulateTeamDatabase()
    {
        var database = AssetDatabase.LoadAssetAtPath<TeamDatabase>("Assets/DATABASES/TeamDatabase.asset");
        if (database == null)
        {
            Debug.LogError("TeamDatabase.asset not found at Assets/Data/");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Teams", new[] { "Assets/DATABASES/team_data/teams_info" }); // Your folder path
        database.allTeams = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<Teams>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(team => team != null)
            .ToList();

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();
        Debug.Log("TeamDatabase populated with " + database.allTeams.Count + " teams.");
    }

    [MenuItem("Tools/Populate Nations Database")]
    public static void PopulateNationsDatabase()
    {
        var database = AssetDatabase.LoadAssetAtPath<NationDatabase>("Assets/DATABASES/NationDatabase.asset");
        if (database == null)
        {
            Debug.LogError("TeamDatabase.asset not found at Assets/Data/");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Nations", new[] { "Assets/DATABASES/team_data/nations" }); // Your folder path
        database.allNations = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<Nations>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(nation => nation != null)
            .ToList();

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();
        Debug.Log("NationDatabase populated with " + database.allNations.Count + " nations.");
    }
}
