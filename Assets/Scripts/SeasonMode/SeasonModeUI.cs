using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SeasonModeUI : MonoBehaviour
{
    public GameObject[] teamNameTexts;
    public GameObject[] fixtureTexts;
    public static bool SeasonGame;
    public static bool IsAwayTeam;
    public static int Round;
    public GameObject Fadeobj;

    private void Awake()
    {

    }
    void Start()
    {
        Fadeobj = GameObject.Find("FADE_IMAGE");
        Round = PlayerPrefs.GetInt("Round");
        Debug.Log("Round: " + Round);
        // Reference the league table manager
        Debug.Log(LeagueTableManager.LeagueTableList.Teams.Count);
        for (int i = 0; i < LeagueTableManager.LeagueTableList.Teams.Count; i++)
        {
            teamNameTexts[i] = GameObject.Find("Team" + (i + 1));
        }

        for (int j = 0; j < 10; j++)
        {
            fixtureTexts[j] = GameObject.Find("Fixture" + (j + 1));
        }

        // Update the UI with the league table data
        UpdateUITable();
        UpdateFixtureList();
    }

    public void UpdateFixtureList()
    {
        Debug.Log(FixtureGenerator.fixtures.Matches.Count);

        for (int i = 0; i < fixtureTexts.Length; i++)
        {
            // Check if the fixture is for round 1
            if (FixtureGenerator.fixtures.Matches[i].round == Round)
            {
                fixtureTexts[i].GetComponent<Text>().text = FixtureGenerator.fixtures.Matches[i].Team1.Name + " vs " + FixtureGenerator.fixtures.Matches[i].Team2.Name;

                // Check if the selected team is involved in the fixture
                if (FixtureGenerator.fixtures.Matches[i].Team1.Name == SeasonModeController.SelectedTeam.Name ||
                    FixtureGenerator.fixtures.Matches[i].Team2.Name == SeasonModeController.SelectedTeam.Name)
                {
                    fixtureTexts[i].GetComponent<Text>().fontStyle = FontStyle.Bold;
                }
            }
            else
            {
                // If not round 1, clear the text and style
                fixtureTexts[i].GetComponent<Text>().text = "";
                fixtureTexts[i].GetComponent<Text>().fontStyle = FontStyle.Normal;
            }
        }
    }
    void UpdateUITable()
    {
        for (int i = 0; i < teamNameTexts.Length; i++)
        {
            if (i < LeagueTableManager.LeagueTableList.Teams.Count)
            {
                teamNameTexts[i].transform.GetChild(0).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].Name;
                teamNameTexts[i].transform.GetChild(1).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].MatchesPlayed.ToString();
                teamNameTexts[i].transform.GetChild(2).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].MatchesWon.ToString();
                teamNameTexts[i].transform.GetChild(3).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].MatchesDrawn.ToString();
                teamNameTexts[i].transform.GetChild(4).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].MatchesLost.ToString();
                teamNameTexts[i].transform.GetChild(5).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].GoalDifference.ToString();
                teamNameTexts[i].transform.GetChild(6).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].Points.ToString();
                if (LeagueTableManager.LeagueTableList.Teams[i].Name == SeasonModeController.SelectedTeam.Name)
                {
                    teamNameTexts[i].transform.GetChild(0).GetComponent<Text>().fontStyle = FontStyle.Bold;
                    teamNameTexts[i].transform.GetChild(1).GetComponent<Text>().fontStyle = FontStyle.Bold;
                    teamNameTexts[i].transform.GetChild(2).GetComponent<Text>().fontStyle = FontStyle.Bold;
                    teamNameTexts[i].transform.GetChild(3).GetComponent<Text>().fontStyle = FontStyle.Bold;
                    teamNameTexts[i].transform.GetChild(4).GetComponent<Text>().fontStyle = FontStyle.Bold;
                    teamNameTexts[i].transform.GetChild(5).GetComponent<Text>().fontStyle = FontStyle.Bold;
                    teamNameTexts[i].transform.GetChild(6).GetComponent<Text>().fontStyle = FontStyle.Bold;
                }
            }
            else
            {
                // Hide extra UI elements if there are fewer than 20 teams
                teamNameTexts[i].transform.GetChild(0).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].Name;
                teamNameTexts[i].transform.GetChild(1).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].MatchesPlayed.ToString();
                teamNameTexts[i].transform.GetChild(2).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].MatchesWon.ToString();
                teamNameTexts[i].transform.GetChild(3).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].MatchesDrawn.ToString();
                teamNameTexts[i].transform.GetChild(4).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].MatchesLost.ToString();
                teamNameTexts[i].transform.GetChild(5).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].GoalDifference.ToString();
                teamNameTexts[i].transform.GetChild(6).GetComponent<Text>().text = LeagueTableManager.LeagueTableList.Teams[i].Points.ToString();
            }
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene("Menu");
    }
    public void PlayButton()
    {
        for (int i = 0; i < fixtureTexts.Length; i++)
        {
            // Check if the fixture is for round 1
            if (FixtureGenerator.fixtures.Matches[i].round == Round)
            {
                if (FixtureGenerator.fixtures.Matches[i].Team1.Name == SeasonModeController.SelectedTeam.Name)
                {
                    PlayerPrefs.SetFloat("Opponent", FixtureGenerator.fixtures.Matches[i].Team2.ID);
                    PlayerPrefs.SetString("OppName", FixtureGenerator.fixtures.Matches[i].Team2.Name);
                    PlayerPrefs.SetFloat("OpponentKit", FixtureGenerator.fixtures.Matches[i].Team2.HomeKitID);
                    PlayerPrefs.Save();
                    IsAwayTeam = false;
                }
                else if (FixtureGenerator.fixtures.Matches[i].Team2.Name == SeasonModeController.SelectedTeam.Name)
                {
                    PlayerPrefs.SetFloat("Opponent", FixtureGenerator.fixtures.Matches[i].Team1.ID);
                    PlayerPrefs.SetString("OppName", FixtureGenerator.fixtures.Matches[i].Team1.Name);
                    PlayerPrefs.SetFloat("OpponentKit", FixtureGenerator.fixtures.Matches[i].Team1.HomeKitID);
                    PlayerPrefs.Save();
                    IsAwayTeam = true;
                }
            }
        }
        Debug.Log(PlayerPrefs.GetString("OppName"));
        SeasonGame = true;
        Debug.Log("Season Game: " + SeasonGame);
        Debug.Log("Away Game: " + IsAwayTeam);
        if (Fadeobj != null && Fadeobj.GetComponent<FadeObj>() != null)
        {
            StartCoroutine(Fadeobj.GetComponent<FadeObj>().FadeIn("GameScene"));
        }
        else
        {
            Debug.LogError("Fadeobj is null or missing its FadeObj script.");
            //   SceneManager.LoadSceneAsync("GameScene"); // fallback if fade fails
        }
    }
}
