using UnityEngine;
using UnityEditor;
using System.Linq;

public class CreateAchievementDatabase
{
    [MenuItem("Tools/Generate Achievement Database")]
    public static void Generate()
    {
        var achievements = AssetDatabase.FindAssets("t:Achievements")
            .Select(guid => AssetDatabase.LoadAssetAtPath<Achievements>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToList();

        AchievementDatabase db = ScriptableObject.CreateInstance<AchievementDatabase>();
        db.allAchievements = achievements;

        string path = "Assets/DATABASES/AchievementDatabase.asset";
        AssetDatabase.CreateAsset(db, path);
        AssetDatabase.SaveAssets();

        Debug.Log($"Achievement Database generated with {achievements.Count} entries at: {path}");
        Selection.activeObject = db;
    }
}
