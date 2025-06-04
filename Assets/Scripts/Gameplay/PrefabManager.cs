using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public GameObject[] Opponents;
    public GameObject[] Defender;
    public Material[] Materials;
    public Texture OppTexture;
    public Texture DefTexture;
    // Start is called before the first frame update
    GameObject[] FindGameObjectsWithTags(params string[] tags)
    {
        var all = new List<GameObject>();

        foreach (string tag in tags)
        {
            all.AddRange(GameObject.FindGameObjectsWithTag(tag).ToList());
        }

        return all.ToArray();
    }
    public TeamDatabase teamDatabase;

    void Start()
    {
        Teams opponentTeam = null;

        if (SeasonModeUI.SeasonGame)
        {
            float oppID = PlayerPrefs.GetFloat("Opponent");
            var oppSeasonTeam = LeagueTableManager.LeagueTableList.Teams.FirstOrDefault(t => t.ID == (int)oppID);
            opponentTeam = oppSeasonTeam?.ReferenceTeamSO;
        }
        else
        {
            opponentTeam = OpponentSelectionScreen.SelectedTeam;
        }

        if (opponentTeam != null && opponentTeam.HomeKit != null)
        {
            Materials[0].SetTexture("_MainTex", opponentTeam.HomeKit);
        }

        // Load Defender (current user-selected team)
        Teams PlayerTeam = null;

        if (SeasonModeUI.SeasonGame)
        {
            int defID = PlayerPrefs.GetInt("SeasonSelectedTeam");
            PlayerTeam = teamDatabase.allTeams.FirstOrDefault(t => t.ID == defID);
        }
        else
        {
            PlayerTeam = SelectionScreen.SelectedTeam;
        }

        if (PlayerTeam != null && PlayerTeam.HomeKit != null)
        {
            Materials[1].SetTexture("_MainTex", PlayerTeam.HomeKit);
        }
    }


    // Update is called once per frame
    void Update()
    {
        Opponents = FindGameObjectsWithTags(new string[] { "Outfield1", "Outfield2" });
        if (Opponents == null)
        {

        }
        Defender = GameObject.FindGameObjectsWithTag("Defender");
        if (Defender == null)
        {

        }
    }

}
