using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonModeController : MonoBehaviour
{
    public Text SelectedText;
    public static Teams SelectedTeam;
    private int i;
    public TeamDatabase teamDatabase;


 private void Awake()
{
    // Get the team ID that was saved in PlayerPrefs
    int selectedTeamID = PlayerPrefs.HasKey("SeasonSelectedTeam")
        ? (int)PlayerPrefs.GetFloat("SeasonSelectedTeam") 
        : -1;

    if (selectedTeamID == -1)
    {
        Debug.LogError("No team ID found in PlayerPrefs.");
        return;
    }

    // Look for the team in the TeamDatabase by ID
    SelectedTeam = teamDatabase.allTeams.Find(team => team.ID == selectedTeamID);

    if (SelectedTeam == null)
    {
        Debug.LogError($"Team with ID {selectedTeamID} not found in TeamDatabase.");
        return;
    }

    Debug.Log("Loaded team: " + SelectedTeam.Name);
  //  SelectedText.text = SelectedTeam.Name;
}

    // Start is called before the first frame update
    void Start()
    {
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
