using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    // THIS SCRIPT IS STRICTLY THE UI CONTROLLER
    // Initiate UI
    public Text HomeTeamName;
    public Text AwayTeamName;
    public Text HomeTeamScore;
    public Text AwayTeamScore;
    public Text ShotsTaken;
    public Text FullTime;
    public GameObject Saves;
    public GameObject Blocks;
    public Image save_1;
    public Image save_2;
    public Image save_3;
    public Image SaveBG;
    public Image BlockBG;
    public Image block_1;
    public Image block_2;
    public Button PauseBtn;
    public GameObject InGame;
    public GameObject PauseMenu;
    public GameObject Settings;
    public GameObject DialogueBox;
    public Text CountdownText;
    public TeamDatabase teamDatabase;
    // Start is called before the first frame update
    private void Awake()
    {
        ShotsTaken.text = GameController.ShotsCount.ToString() + " / " + GameController.MaxShots.ToString();
        if (GameController.SaveCount == 0 && GameController.ShotsCount <= GameController.MaxShots)
        {
            save_1.enabled = false;
            save_2.enabled = false;
            save_3.enabled = false;
            block_1.enabled = false;
            block_2.enabled = false;
        }
    }
    void Start()
    {
        if (SeasonModeUI.SeasonGame)
        {

            if (SeasonModeUI.IsAwayTeam)
            {
                HomeTeamName.text = PlayerPrefs.GetString("OppName");
                AwayTeamName.text = PlayerPrefs.GetString("SeasonTeamName");
                HomeTeamScore.text = GameController.Team2Score.ToString();
                AwayTeamScore.text = GameController.Team1Score.ToString();
            }
            else
            {
                HomeTeamName.text = PlayerPrefs.GetString("SeasonTeamName");
                AwayTeamName.text = PlayerPrefs.GetString("OppName");
                HomeTeamScore.text = GameController.Team1Score.ToString();
                AwayTeamScore.text = GameController.Team2Score.ToString();
            }
        }
        else
        {
            HomeTeamName.text = SelectionScreen.SelectedTeam.Name;
            AwayTeamName.text = OpponentSelectionScreen.SelectedTeam.Name;
            HomeTeamScore.text = GameController.Team1Score.ToString();
            AwayTeamScore.text = GameController.Team2Score.ToString();
        }
        InGame.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SeasonModeUI.SeasonGame)
        {

            if (SeasonModeUI.IsAwayTeam)
            {
                HomeTeamScore.text = GameController.Team2Score.ToString();
                AwayTeamScore.text = GameController.Team1Score.ToString();
            }
            else
            {
                HomeTeamScore.text = GameController.Team1Score.ToString();
                AwayTeamScore.text = GameController.Team2Score.ToString();
            }
        }
        else
        {
            HomeTeamName.text = SelectionScreen.SelectedTeam.Name;
            AwayTeamName.text = OpponentSelectionScreen.SelectedTeam.Name;
            HomeTeamScore.text = GameController.Team1Score.ToString();
            AwayTeamScore.text = GameController.Team2Score.ToString();
        }
        ShotsTaken.text = GameController.ShotsCount.ToString() + " / " + GameController.MaxShots.ToString();
        if (GameController.ShotsCount > GameController.MaxShots)
        {
            FullTime.enabled = true;
            ShotsTaken.enabled = false;
            save_1.enabled = false;
            save_2.enabled = false;
            save_3.enabled = false;
            SaveBG.enabled = false;
            block_1.enabled = false;
            block_2.enabled = false;
            BlockBG.enabled = false;
            PauseBtn.gameObject.SetActive(false); 
        }
        if (GameController.SaveCount == 0 && GameController.ShotsCount <= GameController.MaxShots)
        {
            save_1.enabled = false;
            save_2.enabled = false;
            save_3.enabled = false;
        }
        else if (GameController.SaveCount == 1 && GameController.ShotsCount <= GameController.MaxShots)
        {
            save_1.enabled = true;
            save_2.enabled = false;
            save_3.enabled = false;
        }
        else if (GameController.SaveCount == 2 && GameController.ShotsCount <= GameController.MaxShots)
        {
            save_1.enabled = true;
            save_2.enabled = true;
            save_3.enabled = false;
        }
        else if (GameController.SaveCount == 3 && GameController.ShotsCount <= GameController.MaxShots)
        {
            save_1.enabled = true;
            save_2.enabled = true;
            save_3.enabled = true;
        }
        if (GameController.BlockCount == 0 && GameController.ShotsCount <= GameController.MaxShots)
        {
            block_1.enabled = false;
            block_2.enabled = false;
        }
        else if (GameController.BlockCount == 1 && GameController.ShotsCount <= GameController.MaxShots)
        {
            block_1.enabled = true;
            block_2.enabled = false;
        }
        if (GameController.BlockCount == 2 && GameController.ShotsCount <= GameController.MaxShots)
        {
            block_1.enabled = false;
            block_2.enabled = true;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        InGame.SetActive(false);
        Settings.SetActive(false);
        DialogueBox.SetActive(false);
        GameController.crowd.Pause();
        if (GameController.crowdmiss.isPlaying)
        {
            GameController.crowdmiss.Pause();
        }
        if (GameController.crowdgoal.isPlaying)
        {
            GameController.crowdgoal.Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        InGame.SetActive(true);
        Settings.SetActive(false);
        if (PlayerPrefs.HasKey("Crowd"))
        {
            int on = 1;
            if (PlayerPrefs.GetInt("Crowd") == on)
            {
                GameController.crowd.gameObject.SetActive(true);
                GameController.crowdgoal.gameObject.SetActive(true);
                GameController.crowdmiss.gameObject.SetActive(true);
                GameController.crowd.UnPause();
            }
        }

    }

    public void Back()
    {
        Settings.SetActive(false);
        PauseMenu.SetActive(true);
        InGame.SetActive(false);
        DialogueBox.SetActive(false);
    }

    public void SettingsBtn()
    {
        Settings.SetActive(true);
        PauseMenu.SetActive(false);
        InGame.SetActive(false);
        DialogueBox.SetActive(false);
    }

    public void QuitBtn()
    {
            DialogueBox.SetActive(true);
    }

    public void DialogueYes()
    {
        if (SeasonModeUI.SeasonGame == false)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }
        else if (SeasonModeUI.SeasonGame == true)
        {
            GameController.Team1Score = 0;
            GameController.Team2Score = 3;
            SceneManager.LoadScene("PostSeasonGameMenu");
        }
    }

    public IEnumerator PlayCountdown()
    {
        CountdownText.gameObject.SetActive(true);

        CountdownText.text = "3";
        yield return new WaitForSecondsRealtime(0.5f);

        CountdownText.text = "2";
        yield return new WaitForSecondsRealtime(1f);

        CountdownText.text = "1";
        yield return new WaitForSecondsRealtime(1f);

        CountdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f);

        CountdownText.gameObject.SetActive(false);
    }

    public void DialogueNo()
    {
        PauseMenu.SetActive(true);
        InGame.SetActive(false);
        Settings.SetActive(false);
        DialogueBox.SetActive(false);
    }
}
