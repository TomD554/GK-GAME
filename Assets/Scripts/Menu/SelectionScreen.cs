using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectionScreen : MonoBehaviour
{
    public TeamDatabase teamDatabase;
    public NationDatabase nationDatabase;
    private List<Teams> filteredTeams = new();
    public int TeamsCount;
    public int SelectedOptionFromArray;
    public Teams[] teams;
    public static Teams SelectedTeam;
    public Text Name;
    public GameObject Badge;
    public GameObject NationBadge;
    public Nations[] nations;
    public Nations SelectedNation;
    public int SelectedNationFromArray;
    public int NationsCount;
    public Text NationName;
    // Start is called before the first frame update

    private void Awake()
    {
      
    }
    void Start()
    {
        teams = teamDatabase.allTeams.OrderBy(t => t.ID).ToArray();
        nations = nationDatabase.allNations.OrderBy(n => n.ID).ToArray();

        TeamsCount = teams.Length;
        NationsCount = nations.Length;

        SelectedNationFromArray = 0;
        SetSelectedNation(nations[SelectedNationFromArray]); // Clean start point
    }

    private Sprite LoadTeamLogo(Sprite id)
    {
        var sprite = SelectedTeam.Logo;
        return sprite != null ? sprite : Resources.Load<Sprite>("no_image");
    }

    private Sprite LoadNationLogo(Sprite id)
    {
        var sprite = SelectedNation.Logo;
        return sprite != null ? sprite : Resources.Load<Sprite>("no_image");
    }

    public void NextButtonTeam()
    {
        SelectedOptionFromArray++;
        if (SelectedOptionFromArray >= filteredTeams.Count)
            SelectedOptionFromArray = 0;

        SelectedTeam = filteredTeams[SelectedOptionFromArray];
        Name.text = SelectedTeam.Name;
        Badge.GetComponent<Image>().sprite = LoadTeamLogo(SelectedTeam.Logo);
    }

    public void BackButtonTeam()
    {
        SelectedOptionFromArray--;
        if (SelectedOptionFromArray < 0)
            SelectedOptionFromArray = filteredTeams.Count - 1;

        SelectedTeam = filteredTeams[SelectedOptionFromArray];
        Name.text = SelectedTeam.Name;
        Badge.GetComponent<Image>().sprite = LoadTeamLogo(SelectedTeam.Logo);
    }

    public void NextButtonNation()
    {
        SelectedNationFromArray++;
        if (SelectedNationFromArray >= NationsCount)
            SelectedNationFromArray = 0;

        SetSelectedNation(nations[SelectedNationFromArray]);
    }

    public void BackButtonNation()
    {
        SelectedNationFromArray--;
        if (SelectedNationFromArray < 0)
            SelectedNationFromArray = NationsCount - 1;

        SetSelectedNation(nations[SelectedNationFromArray]);
    }

    private void SetSelectedNation(Nations nation)
    {
        SelectedNation = nation;
        NationName.text = nation.Name;
        NationBadge.GetComponent<Image>().sprite = LoadNationLogo(nation.Logo);

        UpdateTeamListForNation(); // Refresh filtered team list for the selected nation
    }


    private void UpdateTeamListForNation()
    {
        filteredTeams = teams.Where(t => t.Nation == SelectedNation.Name).OrderBy(t => t.ID).ToList();
        SelectedOptionFromArray = 0;

        if (filteredTeams.Count > 0)
        {
            SelectedTeam = filteredTeams[SelectedOptionFromArray];
            Name.text = SelectedTeam.Name;
            Badge.GetComponent<Image>().sprite = LoadTeamLogo(SelectedTeam.Logo);
        }
        else
        {
            Debug.LogWarning($"No teams found for nation: {SelectedNation.Name}");
        }
    }

}
