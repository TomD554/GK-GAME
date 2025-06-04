using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PostGameMenu : MonoBehaviour
{
    public Text ScoreTeam1;
    public Text ScoreTeam2;
    public Text Team1;
    public Text Team2;
    // Start is called before the first frame update
    void Start()
    {
            ScoreTeam1.text = GameController.Team1Score.ToString();
            ScoreTeam2.text = GameController.Team2Score.ToString();
            Team1.text = SelectionScreen.SelectedTeam.Name;
            Team2.text = OpponentSelectionScreen.SelectedTeam.Name;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
