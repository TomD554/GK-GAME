using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

[System.Serializable]
public class SeasonTeams
{
    public string Name;
    public int ID;
    public int HomeKitID;
    public int AwayKitID;
    public float Rating;
    public Teams ReferenceTeamSO; // <-- Add this

    public int Points;
    public int MatchesPlayed;
    public int MatchesWon;
    public int MatchesDrawn;
    public int MatchesLost;
    public int GoalDifference;
}

[System.Serializable]
public class LeagueTable
{

    public List<SeasonTeams> Teams = new List<SeasonTeams>();
    public void SortByPoints()
    {
        Teams.Sort((team1, team2) =>
        {
            // First, compare points
            int pointsComparison = team2.Points.CompareTo(team1.Points);

            // If points are equal, compare goal difference
            if (pointsComparison == 0)
            {
                int goalDifferenceComparison = team2.GoalDifference.CompareTo(team1.GoalDifference);

                // If goal difference is also equal, compare by team name
                if (goalDifferenceComparison == 0)
                {
                    return string.Compare(team1.Name, team2.Name, StringComparison.Ordinal);
                }
                return goalDifferenceComparison;
            }

            return pointsComparison;
        });
    }
    public void SortByName()
    {
        Teams.Sort((team1, team2) => team1.Name.CompareTo(team2.Name));
    }
}

[System.Serializable]
public class LeagueTableManager : MonoBehaviour
{
    public TeamDatabase teamDatabase; // assign via inspector
    [SerializeField] public static LeagueTable LeagueTableList = new LeagueTable();
    public static List<Teams> TeamsListForLeague = new List<Teams>();
    public int FilterTeams;
    public Teams CurrentTeam;

    private void Awake()
    {
		Debug.Log(Application.persistentDataPath);
	}

    // Start is called before the first frame update
    private void Start()
    {
		Load();
    }
    
    public void InitializeLeagueTable()
    {
        TeamsListForLeague = teamDatabase.allTeams
    .Where(t => t.Nation == SeasonModeController.SelectedTeam.Nation)
    .ToList();
        LeagueTableList.Teams = new List<SeasonTeams>();

        foreach (var team in TeamsListForLeague)
        {
            var seasonTeam = new SeasonTeams
            {
                Name = team.Name,
                ID = team.ID,
                HomeKitID = team.HomeKitID,
                AwayKitID = team.AwayKitID,
                Rating = team.Rating,
                Points = 0,
                MatchesPlayed = 0,
                MatchesWon = 0,
                MatchesDrawn = 0,
                MatchesLost = 0,
                GoalDifference = 0,
                ReferenceTeamSO = team // <-- Add this line
            };

            LeagueTableList.Teams.Add(seasonTeam);
            Debug.Log($"{seasonTeam.Name} added to league table.");
        }
        bool allTeamsHaveZeroPoints = LeagueTableList.Teams.All(team => team.Points == 0);

        if (allTeamsHaveZeroPoints)
        {
            // Sort the league table by team names
            LeagueTableList.SortByName();
        }
        else
        {
            LeagueTableList.SortByPoints();
        }
        Save();
        }

    public void Save()
    {
		// Convert the league table object to a JSON string
		string leagueTableData = JsonUtility.ToJson(LeagueTableList);

		// Define a file path where you want to save the data
		string filePath = Application.persistentDataPath + "/LeagueTable.json";

        // Write the JSON data to a file
        Debug.Log(leagueTableData);
		File.WriteAllText(filePath, leagueTableData);
	}
     public void Load()
     {
		// Define the file path where the data is stored
		string filePath = Application.persistentDataPath + "/LeagueTable.json";

		// Check if the file exists
		if (File.Exists(filePath))
		{
			// Read the JSON data from the file
			string leagueTableData = File.ReadAllText(filePath);

			// Deserialize the JSON data into a LeagueTable object
			LeagueTableList = JsonUtility.FromJson<LeagueTable>(leagueTableData);
            // Re-assign ScriptableObject references using the IDs
            foreach (var seasonTeam in LeagueTableList.Teams)
            {
                seasonTeam.ReferenceTeamSO = teamDatabase.allTeams.FirstOrDefault(t => t.ID == seasonTeam.ID);
            }


            bool allTeamsHaveZeroPoints = LeagueTableList.Teams.All(team => team.Points == 0);

            if (allTeamsHaveZeroPoints)
            {
                // Sort the league table by team names
                LeagueTableList.SortByName();
            }
            else
            {
                LeagueTableList.SortByPoints();
            }
            Debug.Log("Successfully Loaded");
		}
		else
		{
			// If the file doesn't exist, create a new league table
			InitializeLeagueTable();
		}
	 }


    // Update is called once per frame



}