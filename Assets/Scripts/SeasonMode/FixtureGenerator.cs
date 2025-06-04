using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FixturesList
{
public List<Match> Matches = new List<Match>();
}
[System.Serializable]
public class FixtureGenerator : MonoBehaviour
{
    [SerializeField] public static FixturesList fixtures = new FixturesList();
    [SerializeField] public static FixturesList playedMatches = new FixturesList();
    public List<SeasonTeams> Teams;

    private void Awake()
    {
       
    }
    private void Start()
    {
        Load();
    }

    public void GenerateFixtures()
    {
        List<Match> generatedMatches = new List<Match>();
        foreach(SeasonTeams team in LeagueTableManager.LeagueTableList.Teams)
        {
            Teams.Add(team);
        }
        ShuffleTeams();
        int numTeams = Teams.Count;

        if (LeagueTableManager.LeagueTableList.Teams.Count % 2 != 0)
        {
            SeasonTeams dummy = new SeasonTeams();
            dummy.Name = "DummyTeam";
            Teams.Add(dummy);
        }

        int totalRounds = numTeams - 1;

        for (int round = 1; round <= totalRounds; round++)
        {
            for (int i = 0; i < numTeams / 2; i++)
            {
                int team1Index = i;
                int team2Index = numTeams - 1 - i;

                SeasonTeams team1 = Teams[team1Index];
                SeasonTeams team2 = Teams[team2Index];

                // Ensure that teams do not play against themselves (e.g., DummyTeam)
                if (team1.Name != "DummyTeam" && team2.Name != "DummyTeam")
                {
                    Match match = new Match(team1, team2);
                    match.round = round;
                    generatedMatches.Add(match);
                    Debug.Log($"Round {round} - Match: {team1.Name} vs {team2.Name}");
                }
            }

            // Rotate the teams in the list for the next round
            RotateTeamList(Teams);
        }

        // Remove the dummy team if it was added
        if (numTeams > LeagueTableManager.LeagueTableList.Teams.Count)
        {
            LeagueTableManager.LeagueTableList.Teams.RemoveAt(numTeams - 1);
        }

        fixtures.Matches = generatedMatches;
        Save();
    }

    public void ShuffleTeams()
    {
        int n = Teams.Count;
        System.Random random = new System.Random();

        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            SeasonTeams temp = Teams[i];
            Teams[i] = Teams[j];
            Teams[j] = temp;
        }
    }

    private void RotateTeamList(List<SeasonTeams> teams)
    {
        SeasonTeams temp = teams[1];
        for (int i = 1; i < teams.Count - 1; i++)
        {
            teams[i] = teams[i + 1];
        }
        teams[teams.Count - 1] = temp;
    }

    public void Save()
    {
        // Convert the league table object to a JSON string
        string fixtureslist = JsonUtility.ToJson(fixtures);

        // Define a file path where you want to save the data
        string filePath = Application.persistentDataPath + "/Fixtures.json";

        // Write the JSON data to a file
        Debug.Log(fixtureslist);
        File.WriteAllText(filePath, fixtureslist);
    }
    public void Load()
    {
        // Define the file path where the data is stored
        string filePath = Application.persistentDataPath + "/Fixtures.json";

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Read the JSON data from the file
            string fixtureslist = File.ReadAllText(filePath);

            // Deserialize the JSON data into a FixtureList object
            fixtures = JsonUtility.FromJson<FixturesList>(fixtureslist);
            Debug.Log(fixtures.Matches.Count);
            Debug.Log("Successfully Loaded");
        }
        else
        {
            // If the file doesn't exist, create a new fixture list
            
            File.Delete(Application.persistentDataPath + "/Fixtures.json");
            GenerateFixtures();

        }
    }
}
[System.Serializable]
public class Match
{

    public SeasonTeams Team1;
    public SeasonTeams Team2;
    public int round;

    public Match(SeasonTeams team1, SeasonTeams team2)
    {
        Team1 = team1;
        Team2 = team2;
    }
}