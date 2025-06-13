using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class PostSeasonController : MonoBehaviour
{
    public GameObject congrats;
    public GameObject unlucky;
    public Text finishedPosition;
    public GameObject Badge;
    public TeamDatabase teamDatabase; 


    // Start is called before the first frame update
    void Awake()
    {
        float SelectedTeam = PlayerPrefs.GetFloat("SeasonSelectedTeam");
        int selectedTeamID = (int)PlayerPrefs.GetFloat("SeasonSelectedTeam");
        Teams selectedTeam = teamDatabase.allTeams.FirstOrDefault(t => t.ID == selectedTeamID);

        if (selectedTeam != null && selectedTeam.Logo != null)
        {
            Badge.GetComponent<Image>().sprite = selectedTeam.Logo;
        }
        else
        {
            // Assign a fallback logo manually from the inspector or a default
            Badge.GetComponent<Image>().sprite = Resources.Load<Sprite>("no_image");
            Debug.LogWarning("No logo found for selected team, assigning fallback.");
            // Badge.GetComponent<Image>().sprite = fallbackLogo;
        }

        GetComponent<LeagueTableManager>().Load();
        int teamPosition = -1; 

        for (int i = 0; i < LeagueTableManager.LeagueTableList.Teams.Count; i++)
        {
            if (SelectedTeam == LeagueTableManager.LeagueTableList.Teams[i].ID)
            {
                teamPosition = i + 1; // Adding 1 to convert to a 1-based position
                break; // No need to continue searching
            }
        }

        if (teamPosition != -1)
        {
            finishedPosition.text = $"YOU FINISHED: {teamPosition}{GetPositionSuffix(teamPosition)}";

            if (teamPosition == 1)
            {
                congrats.SetActive(true);
                unlucky.SetActive(false);
            }
            else
            {
                congrats.SetActive(false);
                unlucky.SetActive(true);
            }
        }
        else
        {
            finishedPosition.text = "Team not found"; // Or any other appropriate message
        }
    }

    private void Start()
    {
        PlayerPrefs.DeleteKey("SeasonSelectedTeam");
        PlayerPrefs.DeleteKey("Round");
        PlayerPrefs.DeleteKey("SeasonModeStarted");
        PlayerPrefs.DeleteKey("SeasonTeamName");
        PlayerPrefs.Save();
    }

    string GetPositionSuffix(int position)
    {
        if (position >= 11 && position <= 13)
        {
            return "TH";
        }

        int lastDigit = position % 10;
        switch (lastDigit)
        {
            case 1:
                return "ST";
            case 2:
                return "ND";
            case 3:
                return "RD";
            default:
                return "TH";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueButton()
    {
        DeleteFiles();
        if (PlayerPrefs.HasKey("SeasonSelectedTeam"))
        {
            PlayerPrefs.DeleteKey("SeasonSelectedTeam");
            PlayerPrefs.DeleteKey("Round");
            PlayerPrefs.DeleteKey("SeasonModeStarted");
            PlayerPrefs.DeleteKey("SeasonTeamName");
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene("Menu");
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
