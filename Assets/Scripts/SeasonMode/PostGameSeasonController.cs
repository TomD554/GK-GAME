using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostGameSeasonController : MonoBehaviour
{
    public Text ScoreTeam1;
    public Text ScoreTeam2;
    public Text Team1;
    public Text Team2;
    public GameObject FinalScore;
    public GameObject Table;
    public GameObject Results;
    public GameObject Continue;
    public GameObject[] fixtureTexts;
    public GameObject[] teamNameTexts;
    public int RoundInt;
    // Start is called before the first frame update
    void Start()
    {
        RoundInt = PlayerPrefs.GetInt("Round");
        if (SeasonModeUI.IsAwayTeam == false)
        {
            ScoreTeam1.text = GameController.Team1Score.ToString();
            ScoreTeam2.text = GameController.Team2Score.ToString();
            Team1.text = SeasonModeController.SelectedTeam.Name;
            Team2.text = PlayerPrefs.GetString("OppName");
        }
        else
        {
            ScoreTeam2.text = GameController.Team1Score.ToString();
            ScoreTeam1.text = GameController.Team2Score.ToString();
            Team2.text = SeasonModeController.SelectedTeam.Name;
            Team1.text = PlayerPrefs.GetString("OppName");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextButton()
    {
        FinalScore.SetActive(false);
        Table.SetActive(true);
        Results.SetActive(true);
        Continue.SetActive(true);
        GenerateResults();
    }

    public void UpdateLeagueTable()
    {
        for (int i = 0; i < LeagueTableManager.LeagueTableList.Teams.Count; i++)
        {
            teamNameTexts[i] = GameObject.Find("Team" + (i + 1));
        }
        UpdateUITable();
    }

    void UpdateUITable()
    {
        GetComponent<LeagueTableManager>().Load();
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

    public void GenerateResults()
    {
        for (int j = 0; j < 10; j++)
        {
            fixtureTexts[j] = GameObject.Find("Fixture" + (j + 1));
        }
            for (int i = 0; i < fixtureTexts.Length; i++)
            {
                if (FixtureGenerator.fixtures.Matches[i].round == RoundInt)
                {
                    SeasonTeams seasonTeam1;
                    SeasonTeams seasonTeam2;
                    int Team1Score;
                    int Team2Score;
                    seasonTeam1 = LeagueTableManager.LeagueTableList.Teams.Find(x => x.Name == FixtureGenerator.fixtures.Matches[i].Team1.Name);
                    seasonTeam2 = LeagueTableManager.LeagueTableList.Teams.Find(x => x.Name == FixtureGenerator.fixtures.Matches[i].Team2.Name);
                    if (FixtureGenerator.fixtures.Matches[i].Team1.Name == SeasonModeController.SelectedTeam.Name || FixtureGenerator.fixtures.Matches[i].Team2.Name == SeasonModeController.SelectedTeam.Name)
                    {
                        if (SeasonModeUI.IsAwayTeam == false)
                        {
                            Team1Score = GameController.Team1Score;
                            Team2Score = GameController.Team2Score;
                            if (Team1Score > Team2Score)
                            {
                                FixtureGenerator.fixtures.Matches[i].Team1.Points += 3;
                                seasonTeam1.Points += FixtureGenerator.fixtures.Matches[i].Team1.Points;
                                FixtureGenerator.fixtures.Matches[i].Team1.MatchesWon++;
                                seasonTeam1.MatchesWon += FixtureGenerator.fixtures.Matches[i].Team1.MatchesWon;
                                FixtureGenerator.fixtures.Matches[i].Team2.MatchesLost++;
                                seasonTeam2.MatchesLost += FixtureGenerator.fixtures.Matches[i].Team2.MatchesLost;
                            }
                            else if (Team1Score < Team2Score)
                            {
                                FixtureGenerator.fixtures.Matches[i].Team2.Points += 3;
                                seasonTeam2.Points += FixtureGenerator.fixtures.Matches[i].Team2.Points;
                                FixtureGenerator.fixtures.Matches[i].Team2.MatchesWon++;
                                seasonTeam2.MatchesWon += FixtureGenerator.fixtures.Matches[i].Team2.MatchesWon;
                                FixtureGenerator.fixtures.Matches[i].Team1.MatchesLost++;
                                seasonTeam1.MatchesLost += FixtureGenerator.fixtures.Matches[i].Team1.MatchesLost;
                            }
                            else if (Team1Score == Team2Score)
                            {
                                FixtureGenerator.fixtures.Matches[i].Team1.Points += 1;
                                FixtureGenerator.fixtures.Matches[i].Team2.Points += 1;
                                seasonTeam1.Points += FixtureGenerator.fixtures.Matches[i].Team1.Points;
                                seasonTeam2.Points += FixtureGenerator.fixtures.Matches[i].Team2.Points;
                                FixtureGenerator.fixtures.Matches[i].Team1.MatchesDrawn++;
                                FixtureGenerator.fixtures.Matches[i].Team2.MatchesDrawn++;
                                seasonTeam1.MatchesDrawn += FixtureGenerator.fixtures.Matches[i].Team1.MatchesDrawn;
                                seasonTeam2.MatchesDrawn += FixtureGenerator.fixtures.Matches[i].Team2.MatchesDrawn;
                            }
                            FixtureGenerator.fixtures.Matches[i].Team1.MatchesPlayed++;
                            FixtureGenerator.fixtures.Matches[i].Team2.MatchesPlayed++;
                            seasonTeam1.MatchesPlayed += FixtureGenerator.fixtures.Matches[i].Team1.MatchesPlayed;
                            seasonTeam2.MatchesPlayed += FixtureGenerator.fixtures.Matches[i].Team2.MatchesPlayed;
                            FixtureGenerator.fixtures.Matches[i].Team1.GoalDifference += (Team1Score - Team2Score);
                            FixtureGenerator.fixtures.Matches[i].Team2.GoalDifference += (Team2Score - Team1Score);
                            seasonTeam1.GoalDifference += FixtureGenerator.fixtures.Matches[i].Team1.GoalDifference;
                            seasonTeam2.GoalDifference += FixtureGenerator.fixtures.Matches[i].Team2.GoalDifference;
                            fixtureTexts[i].GetComponent<Text>().text = FixtureGenerator.fixtures.Matches[i].Team1.Name + " " + Team1Score + " - " + Team2Score + " " + FixtureGenerator.fixtures.Matches[i].Team2.Name;
                            fixtureTexts[i].GetComponent<Text>().fontStyle = FontStyle.Bold;
                        }
                        else if (SeasonModeUI.IsAwayTeam == true)
                        {
                            Team2Score = GameController.Team1Score;
                            Team1Score = GameController.Team2Score;
                            if (Team1Score > Team2Score)
                            {
                                FixtureGenerator.fixtures.Matches[i].Team2.Points += 3;
                                seasonTeam1.Points += FixtureGenerator.fixtures.Matches[i].Team2.Points;
                                FixtureGenerator.fixtures.Matches[i].Team2.MatchesWon++;
                                seasonTeam1.MatchesWon += FixtureGenerator.fixtures.Matches[i].Team2.MatchesWon;
                                FixtureGenerator.fixtures.Matches[i].Team1.MatchesLost++;
                                seasonTeam2.MatchesLost += FixtureGenerator.fixtures.Matches[i].Team1.MatchesLost;
                            }
                            else if (Team1Score < Team2Score)
                            {
                                FixtureGenerator.fixtures.Matches[i].Team1.Points += 3;
                                seasonTeam2.Points += FixtureGenerator.fixtures.Matches[i].Team1.Points;
                                FixtureGenerator.fixtures.Matches[i].Team1.MatchesWon++;
                                seasonTeam2.MatchesWon += FixtureGenerator.fixtures.Matches[i].Team1.MatchesWon;
                                FixtureGenerator.fixtures.Matches[i].Team2.MatchesLost++;
                                seasonTeam1.MatchesLost += FixtureGenerator.fixtures.Matches[i].Team2.MatchesLost;
                            }
                            else if (Team1Score == Team2Score)
                            {
                                FixtureGenerator.fixtures.Matches[i].Team1.Points += 1;
                                FixtureGenerator.fixtures.Matches[i].Team2.Points += 1;
                                seasonTeam1.Points += FixtureGenerator.fixtures.Matches[i].Team1.Points;
                                seasonTeam2.Points += FixtureGenerator.fixtures.Matches[i].Team2.Points;
                                FixtureGenerator.fixtures.Matches[i].Team1.MatchesDrawn++;
                                FixtureGenerator.fixtures.Matches[i].Team2.MatchesDrawn++;
                                seasonTeam1.MatchesDrawn += FixtureGenerator.fixtures.Matches[i].Team1.MatchesDrawn;
                                seasonTeam2.MatchesDrawn += FixtureGenerator.fixtures.Matches[i].Team2.MatchesDrawn;
                            }
                            FixtureGenerator.fixtures.Matches[i].Team1.MatchesPlayed++;
                            FixtureGenerator.fixtures.Matches[i].Team2.MatchesPlayed++;
                            seasonTeam1.MatchesPlayed += FixtureGenerator.fixtures.Matches[i].Team1.MatchesPlayed;
                            seasonTeam2.MatchesPlayed += FixtureGenerator.fixtures.Matches[i].Team2.MatchesPlayed;
                            FixtureGenerator.fixtures.Matches[i].Team2.GoalDifference += (Team2Score - Team1Score);
                            FixtureGenerator.fixtures.Matches[i].Team1.GoalDifference += (Team1Score - Team2Score);
                            seasonTeam1.GoalDifference += FixtureGenerator.fixtures.Matches[i].Team1.GoalDifference;
                            seasonTeam2.GoalDifference += FixtureGenerator.fixtures.Matches[i].Team2.GoalDifference;
                            fixtureTexts[i].GetComponent<Text>().text = FixtureGenerator.fixtures.Matches[i].Team1.Name + " " + Team1Score + " - " + Team2Score + " " + FixtureGenerator.fixtures.Matches[i].Team2.Name;
                            fixtureTexts[i].GetComponent<Text>().fontStyle = FontStyle.Bold;
                        }
                    }
                    else
                    {

                    Team1Score = SimulateGoals(seasonTeam1.Rating, seasonTeam2.Rating);
                    Team2Score = SimulateGoals(seasonTeam2.Rating, seasonTeam1.Rating);

                    int SimulateGoals(float teamRating, float opponentRating)
                    {
                        // Adjust the strength factor based on team's star rating and opponent's strength
                        float strengthFactor = Random.Range(0.8f, 1.2f) * (teamRating / opponentRating);

                        // Simulate the number of goals based on team strength
                        int goals = Mathf.RoundToInt(Random.Range(0f, 4f) * strengthFactor);

                        return goals;
                    }
                    if (Team1Score > Team2Score)
                        {
                            FixtureGenerator.fixtures.Matches[i].Team1.Points += 3;
                            seasonTeam1.Points += FixtureGenerator.fixtures.Matches[i].Team1.Points;
                            FixtureGenerator.fixtures.Matches[i].Team1.MatchesWon++;
                            seasonTeam1.MatchesWon += FixtureGenerator.fixtures.Matches[i].Team1.MatchesWon;
                            FixtureGenerator.fixtures.Matches[i].Team2.MatchesLost++;
                            seasonTeam2.MatchesLost += FixtureGenerator.fixtures.Matches[i].Team2.MatchesLost;
                        }
                        else if (Team1Score < Team2Score)
                        {
                            FixtureGenerator.fixtures.Matches[i].Team2.Points += 3;
                            seasonTeam2.Points += FixtureGenerator.fixtures.Matches[i].Team2.Points;
                            FixtureGenerator.fixtures.Matches[i].Team2.MatchesWon++;
                            seasonTeam2.MatchesWon += FixtureGenerator.fixtures.Matches[i].Team2.MatchesWon;
                            FixtureGenerator.fixtures.Matches[i].Team1.MatchesLost++;
                            seasonTeam1.MatchesLost += FixtureGenerator.fixtures.Matches[i].Team1.MatchesLost;
                        }
                        else if (Team1Score == Team2Score)
                        {
                            FixtureGenerator.fixtures.Matches[i].Team1.Points += 1;
                            FixtureGenerator.fixtures.Matches[i].Team2.Points += 1;
                            seasonTeam1.Points += FixtureGenerator.fixtures.Matches[i].Team1.Points;
                            seasonTeam2.Points += FixtureGenerator.fixtures.Matches[i].Team2.Points;
                            FixtureGenerator.fixtures.Matches[i].Team1.MatchesDrawn++;
                            FixtureGenerator.fixtures.Matches[i].Team2.MatchesDrawn++;
                            seasonTeam1.MatchesDrawn += FixtureGenerator.fixtures.Matches[i].Team1.MatchesDrawn;
                            seasonTeam2.MatchesDrawn += FixtureGenerator.fixtures.Matches[i].Team2.MatchesDrawn;
                        }
                        FixtureGenerator.fixtures.Matches[i].Team1.MatchesPlayed++;
                        FixtureGenerator.fixtures.Matches[i].Team2.MatchesPlayed++;
                        seasonTeam1.MatchesPlayed += FixtureGenerator.fixtures.Matches[i].Team1.MatchesPlayed;
                        seasonTeam2.MatchesPlayed += FixtureGenerator.fixtures.Matches[i].Team2.MatchesPlayed;
                        FixtureGenerator.fixtures.Matches[i].Team1.GoalDifference += (Team1Score - Team2Score);
                        FixtureGenerator.fixtures.Matches[i].Team2.GoalDifference += (Team2Score - Team1Score);
                        seasonTeam1.GoalDifference += FixtureGenerator.fixtures.Matches[i].Team1.GoalDifference;
                        seasonTeam2.GoalDifference += FixtureGenerator.fixtures.Matches[i].Team2.GoalDifference;
                        fixtureTexts[i].GetComponent<Text>().text = FixtureGenerator.fixtures.Matches[i].Team1.Name + " " + Team1Score + " - " + Team2Score + " " + FixtureGenerator.fixtures.Matches[i].Team2.Name;
                    }
                }
            }
        GetComponent<FixtureGenerator>().Save();
        GetComponent<LeagueTableManager>().Save();
        UpdateLeagueTable();
        if (RoundInt <= 19)
        {
            // First, remove fixtures
            if (FixtureGenerator.fixtures.Matches.Count >= 10)
            {
                FixtureGenerator.fixtures.Matches.RemoveRange(0, 10);
            }
            else
            {
                FixtureGenerator.fixtures.Matches.Clear(); // just clear the rest safely
            }
            GetComponent<FixtureGenerator>().Save();

            // Then increment for the next round AFTER fixtures were used
            RoundInt = PlayerPrefs.GetInt("Round") + 1;
            PlayerPrefs.SetInt("Round", RoundInt);
        }
    }
    public void ContinueButton()
    {
        if (RoundInt <= 19)
        {
            Debug.Log("Round: " + PlayerPrefs.GetInt("Round"));
            SceneManager.LoadScene("SeasonMainMenu");
        }
        else if (RoundInt > 19)
        {
            SceneManager.LoadScene("FinishedSeasonScene");
        }
    }
}
