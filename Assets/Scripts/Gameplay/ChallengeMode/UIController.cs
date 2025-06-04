using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Text Timer;
    public Text FullTime;
    public Button PauseBtn;
    public GameObject InGame;
    public GameObject PauseMenu;
    public GameObject Settings;
    public GameObject DialogueBox;
    public Text CountdownText;
    public Text SaveCount;
    // Start is called before the first frame update
    void Start()
    {
            PauseBtn.gameObject.SetActive(false);
        InGame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SaveCount.text = gamecontroller2.savecount.ToString();
        if (GetComponent<gamecontroller2>().gameState == gamecontroller2.GameStates.FullTime)
        {
            Timer.GetComponent<Text>().enabled = false;
            FullTime.GetComponent<Text>().enabled = true;
            
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        InGame.SetActive(false);
        Settings.SetActive(false);
        DialogueBox.SetActive(false);
        gamecontroller2.crowd.Pause();
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
                gamecontroller2.crowd.gameObject.SetActive(true);
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
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
    }
    public void DialogueNo()
    {
        PauseMenu.SetActive(true);
        InGame.SetActive(false);
        Settings.SetActive(false);
        DialogueBox.SetActive(false);
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
}
