using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class MenuController : MonoBehaviour
{
    public static int Team1Selection;
    public static int Team2Selection;
    public GameObject SelectionScreenMenu;
    public GameObject MainMenu;
    public GameObject SeasonModeStartMenu;
    public GameObject BeginSeason;
    public GameObject DialogueBox;
    public GameObject Settings;
    public GameObject CameraZoom;
    public GameObject Achievements;
    public static bool ChallengeMode;
    public GameObject Fadeobj;


        // Start is called before the first frame update
        private void Awake()
    {
    }
    void Start()
    {
        Debug.Log("Safe area: " + Screen.safeArea);
        Fadeobj = GameObject.Find("FADE_IMAGE");
        ChallengeMode = false;
        Application.targetFrameRate = 60;
        CameraZoom.SetActive(false);
        DialogueBox.SetActive(false);
        SelectionScreenMenu.SetActive(false);
        MainMenu.SetActive(true);
        SeasonModeStartMenu.SetActive(false);
        BeginSeason.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<OpponentSelectionScreen>().enabled = false;
        BeginSeason.SetActive(false);
        Team2Selection = 2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChallengeButton()
    {
        StartCoroutine(Fadeobj.GetComponent<FadeObj>().FadeIn("ChallengeScene"));
        ChallengeMode = true;
    }

    public void SettingsButton()
    {
        DialogueBox.SetActive(false);
        SelectionScreenMenu.SetActive(false);
        SeasonModeStartMenu.SetActive(false);
        BeginSeason.SetActive(false);
        MainMenu.SetActive(false);
        Settings.SetActive(true);
        BeginSeason.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<OpponentSelectionScreen>().enabled = false;
    }

    public void AchievementsButton()
    {
        AchievementDisplay achievementDisplay = GetComponent<AchievementDisplay>();
        achievementDisplay.SetupAchievements();
        Achievements.SetActive(true);
        foreach (var btn in GameObject.FindGameObjectsWithTag("Button"))
        {
            var button = btn.GetComponent<Button>();
            if (button != null)
                button.interactable = false;
        }

    }

    public void CloseAchievements()
    {
        AchievementDisplay achievementDisplay = GetComponent<AchievementDisplay>();
        achievementDisplay.SetupAchievements();
        Achievements.SetActive(false);
        foreach (var btn in GameObject.FindGameObjectsWithTag("Button"))
        {
            var button = btn.GetComponent<Button>();
            if (button != null)
                button.interactable = true;
        }

    }

    public void BackButtonClicked()
    {
        Achievements.SetActive(false);
        CameraZoom.SetActive(false);
        DialogueBox.SetActive(false);
        SelectionScreenMenu.SetActive(false);
        SeasonModeStartMenu.SetActive(false);
        BeginSeason.SetActive(false);
        MainMenu.SetActive(true);
        Settings.SetActive(false);
        BeginSeason.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<OpponentSelectionScreen>().enabled = false;
        foreach (var btn in GameObject.FindGameObjectsWithTag("Button"))
        {
            var button = btn.GetComponent<Button>();
            if (button != null)
                button.interactable = true;
        }
    }
    public void FriendlyGameClicked()
    {
        CameraZoom.SetActive(true);
        CameraZoom.GetComponent<Slider>().value = PlayerPrefs.GetFloat("CameraZoom");
        DialogueBox.SetActive(false);
        SelectionScreenMenu.SetActive(true);
        MainMenu.SetActive(false);
        BeginSeason.SetActive(false);
        SeasonModeStartMenu.SetActive(false);
        Settings.SetActive(false);
        BeginSeason.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<SelectionScreen>().enabled = true;
        SelectionScreenMenu.GetComponent<OpponentSelectionScreen>().enabled = true;

    }
    public void PlayButtonClicked()
    {
        SeasonModeUI.SeasonGame = false;
        PlayerPrefs.SetInt("OpponentTeamID", OpponentSelectionScreen.SelectedTeam.ID);
        PlayerPrefs.SetInt("OpponentKitID", OpponentSelectionScreen.SelectedTeam.HomeKitID);
        PlayerPrefs.Save();
        StartCoroutine(Fadeobj.GetComponent<FadeObj>().FadeIn("GameScene")) ;
    }
    public void SeasonModeClicked()
    {
        CameraZoom.SetActive(false);
        DialogueBox.SetActive(false);
        SeasonModeStartMenu.SetActive(true);
        BeginSeason.SetActive(false);
        MainMenu.SetActive(false);
        Settings.SetActive(false);
        BeginSeason.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<OpponentSelectionScreen>().enabled = false;
    }
    public void BeginSeasonModeClicked()
    {
        if (PlayerPrefs.HasKey("SeasonModeStarted") == true)
        {
            DialogueBox.SetActive(true);
        }
        else
        {
            BeginSeason.SetActive(true);
            SeasonModeStartMenu.SetActive(false);
            MainMenu.SetActive(false);
            BeginSeason.GetComponent<SelectionScreen>().enabled = true;
            SelectionScreenMenu.GetComponent<SelectionScreen>().enabled = false;
            SelectionScreenMenu.GetComponent<OpponentSelectionScreen>().enabled = false;
        }
    }
    public void TeamSelectedSeason()
    {
        PlayerPrefs.SetInt("SeasonSelectedTeam", SelectionScreen.SelectedTeam.ID);
        Debug.Log("SeasonSelectedTeam:" + PlayerPrefs.GetFloat("SeasonSelectedTeam"));
        PlayerPrefs.SetString("SeasonTeamName", SelectionScreen.SelectedTeam.Name);
        PlayerPrefs.SetFloat("SeasonSelectedTeamKit", SelectionScreen.SelectedTeam.HomeKitID);
        PlayerPrefs.SetString("SeasonModeStarted", "true");
        Debug.Log(PlayerPrefs.GetFloat("SeasonSelectedTeam"));
        PlayerPrefs.SetInt("Round", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("SeasonMainMenu");
    }
    public void ContinueSeasonModeClicked()
    {
        if (PlayerPrefs.HasKey("SeasonModeStarted") == true)
        {
            SceneManager.LoadScene("SeasonMainMenu");
        }
        else
        {
            BeginSeason.SetActive(true);
            SeasonModeStartMenu.SetActive(false);
            MainMenu.SetActive(false);
            BeginSeason.GetComponent<SelectionScreen>().enabled = true;
            SelectionScreenMenu.GetComponent<SelectionScreen>().enabled = false;
            SelectionScreenMenu.GetComponent<OpponentSelectionScreen>().enabled = false;
        }
    }
    public void DialogueYes()
    {
        DeleteFiles();
        PlayerPrefs.DeleteKey("SeasonSelectedTeam");
        PlayerPrefs.DeleteKey("Round");
        PlayerPrefs.DeleteKey("SeasonModeStarted");
        CameraZoom.SetActive(false);
        DialogueBox.SetActive(false);
        BeginSeason.SetActive(true);
        SeasonModeStartMenu.SetActive(false);
        MainMenu.SetActive(false);
        Settings.SetActive(false);
        BeginSeason.GetComponent<SelectionScreen>().enabled = true;
        SelectionScreenMenu.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<OpponentSelectionScreen>().enabled = false;
    }
    public void DialogueNo()
    {
        CameraZoom.SetActive(false);
        DialogueBox.SetActive(false);
        SeasonModeStartMenu.SetActive(true);
        BeginSeason.SetActive(false);
        MainMenu.SetActive(false);
        Settings.SetActive(false);
        BeginSeason.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<SelectionScreen>().enabled = false;
        SelectionScreenMenu.GetComponent<OpponentSelectionScreen>().enabled = false;
    }
    public void DeleteFiles()
    {
        string DeleteTableFilePath;
        DeleteTableFilePath = Application.persistentDataPath + "/LeagueTable.json";
        if (File.Exists(DeleteTableFilePath))
        {
            File.Delete(DeleteTableFilePath);
            Debug.Log("File deleted: " + DeleteTableFilePath);
        }
        else
        {
            Debug.LogWarning("File not found: " + DeleteTableFilePath);
        }
        string DeleteFixturesFilePath;
        DeleteFixturesFilePath = Application.persistentDataPath + "/Fixtures.json";
        if (File.Exists(DeleteFixturesFilePath))
        {
            File.Delete(DeleteFixturesFilePath);
            Debug.Log("File deleted: " + DeleteFixturesFilePath);
        }
        else
        {
            Debug.LogWarning("File not found: " + DeleteFixturesFilePath);
        }
    }
}

