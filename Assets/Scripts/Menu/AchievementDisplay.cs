using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using System.Threading.Tasks;
using System.Linq;

public class AchievementDisplay : MonoBehaviour
{
    [Header("References")]
    public AchievementDatabase database; // Drag in your auto-generated asset
    public GameObject achievementItemPrefab; // Drag in your prefab
    public Transform contentParent; // Scroll View Content object
    public static bool claimed;


    void Start()
    {
        if (CloudSaveManager.Instance != null)
        {
            
            CloudSaveManager.Instance.OnPlayerDataLoaded += SetupAchievements;


            if (CloudSaveManager.Instance.playerData != null)
            {
                SetupAchievements();
            }

        }
    }

    private void OnDestroy()
    {
        if (CloudSaveManager.Instance != null)
        {
            CloudSaveManager.Instance.OnPlayerDataLoaded -= SetupAchievements;
        }
    }


   public void SetupAchievements()
    {

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        var currentStats = new Dictionary<Achievements.AchievementType, int>
    {
        { Achievements.AchievementType.TotalSaves, CloudSaveManager.Instance.playerData.totalSaves },
        { Achievements.AchievementType.MatchesPlayed, CloudSaveManager.Instance.playerData.matchesPlayed },
        { Achievements.AchievementType.CleanSheets, CloudSaveManager.Instance.playerData.cleanSheets },
        { Achievements.AchievementType.MatchesWon, CloudSaveManager.Instance.playerData.matchesWon }
    };

        // Copy the list to avoid modifying the original database
        var achievementsList = new List<Achievements>(database.allAchievements);

        // Sort by state:
        achievementsList = achievementsList
            .OrderBy(a => GetAchievementSortOrder(a, currentStats))
            .ToList();

        foreach (var achievement in achievementsList)
        {
            GameObject item = Instantiate(achievementItemPrefab, contentParent);
            var ui = item.GetComponent<AchievementItemUI>();

            ui.cloudSaveManager = CloudSaveManager.Instance;

            int currentValue = currentStats.TryGetValue(achievement.type, out var value) ? value : 0;
            ui.Setup(achievement, currentValue);
        }
    }

    private int GetAchievementSortOrder(Achievements achievement, Dictionary<Achievements.AchievementType, int> currentStats)
    {
        var playerData = CloudSaveManager.Instance.playerData;

        // Check if the achievement has been claimed
        bool claimed = playerData.achievementsClaimed.Any(a => a.id == achievement.ID && a.claimed);

        // Check current progress toward the achievement
        int currentValue = currentStats.TryGetValue(achievement.type, out var value) ? value : 0;

        if (!claimed && currentValue >= achievement.requiredValue)
        {
            // Unlocked but not yet claimed
            return 0;
        }
        else if (claimed)
        {
            // Already claimed
            return 2;
        }
        else
        {
            // Locked (incomplete)
            return 1;
        }
    }

}

