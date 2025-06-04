using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.CloudSave.Models.Data.Player;
using SaveOptions = Unity.Services.CloudSave.Models.Data.Player.SaveOptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using System;

public class CloudSaveManager : MonoBehaviour
{
    public PlayerAchievementData playerData = new PlayerAchievementData();
    public static CloudSaveManager Instance;
    public AchievementDatabase database;
    public event System.Action OnPlayerDataLoaded;


    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        await UnityServices.InitializeAsync();


    }

    public async void SaveData(PlayerAchievementData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        var dataToSave = new Dictionary<string, object>
    {
        { "playerAchievementData", jsonData }
    };

        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);
            Debug.Log("Player data saved.");
        }
        catch (CloudSaveException ex)
        {
            Debug.LogError($"Cloud Save Error: {ex.Message}");
        }
    }


    public async Task LoadData()
    {
        var playerDataDict = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "playerAchievementData" });

        if (playerDataDict.TryGetValue("playerAchievementData", out var jsonData))
        {
            string jsonString = jsonData.Value.GetAs<string>();
            PlayerAchievementData loadedData = JsonUtility.FromJson<PlayerAchievementData>(jsonString);
            playerData = loadedData;

            Debug.Log($"Total Saves: {loadedData.totalSaves}");
            Debug.Log($"Achievements Claimed: {loadedData.achievementsClaimed.Count}");
        }
        else
        {
            Debug.Log("No existing player data found. Starting fresh.");
            playerData = new PlayerAchievementData();
        }

        // Notify listeners
        OnPlayerDataLoaded?.Invoke();
    }


    public void IncrementTotalSaves()
    {
        playerData.totalSaves++;
        SaveData(playerData);
    }

    public void IncrementMatchesPlayed()
    {
        playerData.matchesPlayed++;
        SaveData(playerData);
    }

    public void IncrementCleanSheets()
    {
        playerData.cleanSheets++;
        SaveData(playerData);
    }

    public void IncrementMatchesWon()
    {
        playerData.matchesWon++;
        SaveData(playerData);
    }

    public void ClaimAchievement(string achievementID)
    {
        // Check if achievement already exists
        AchievementStatus status = playerData.achievementsClaimed.Find(a => a.id == achievementID);
        Achievements achievement = FindAchievementByID(achievementID);

        if (status != null)
        {
            if (!status.claimed)
            {
                status.claimed = true;
                Debug.Log($"Achievement claimed: {achievementID}");
            }
            else
            {
                Debug.Log($"Achievement already claimed: {achievementID}");
            }
        }
        else
        {
            // New achievement entry
            playerData.achievementsClaimed.Add(new AchievementStatus { id = achievementID, claimed = true });
            Debug.Log($"Achievement claimed and added: {achievementID}");
            AddCoins(achievement.coinReward);
        }

        SaveData(playerData);
    }

    private Achievements FindAchievementByID(string achievementID)
    {
        return database.allAchievements
            .FirstOrDefault(a => a.ID == achievementID);
    }

    private async void AddCoins(int amount)
    {
        try
        {
            await EconomyManager.Instance.IncrementPlayerBalance("COINS", amount);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error adding coins via Economy Service: {ex.Message}");
        }
    }


}