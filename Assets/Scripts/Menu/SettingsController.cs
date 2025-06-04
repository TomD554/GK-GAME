using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Toggle SFX;
    public Toggle Music;
    public Toggle Crowd;
    public Slider CameraZoom;
    public Scrollbar Difficulty;
    public Text DifficultyText;

    void Awake()
    {
        SetDefaultsIfFirstLaunch();
    }

    void Start()
    {
        // Load Difficulty
        int difficultyLevel = PlayerPrefs.GetInt("Difficulty");
        switch (difficultyLevel)
        {
            case 0:
                Difficulty.value = 0f;
                DifficultyText.text = "Easy";
                break;
            case 1:
                Difficulty.value = 0.39f;
                DifficultyText.text = "Normal";
                break;
            case 2:
                Difficulty.value = 0.51f;
                DifficultyText.text = "Hard";
                break;
            case 3:
                Difficulty.value = 1f;
                DifficultyText.text = "Legendary";
                break;
        }

        // Load Camera Zoom
        CameraZoom.value = PlayerPrefs.GetFloat("CameraZoom");

        // Load Toggles
        Crowd.isOn = PlayerPrefs.GetInt("Crowd") == 1;
        SFX.isOn = PlayerPrefs.GetInt("SFX") == 1;
        Music.isOn = PlayerPrefs.GetInt("Music") == 1;
    }

    void SetDefaultsIfFirstLaunch()
    {
        if (!PlayerPrefs.HasKey("HasLaunched"))
        {
            PlayerPrefs.SetInt("SFX", 1);
            PlayerPrefs.SetInt("Music", 1);
            PlayerPrefs.SetInt("Crowd", 1);
            PlayerPrefs.SetFloat("CameraZoom", -8.6f);
            PlayerPrefs.SetInt("Difficulty", 0);
            PlayerPrefs.SetInt("HasLaunched", 1);
            PlayerPrefs.Save();
        }
    }

    public void CameraZoomChange()
    {
        PlayerPrefs.SetFloat("CameraZoom", CameraZoom.value);
        PlayerPrefs.Save();
    }

    public void SFXOnOff()
    {
        int on = SFX.isOn ? 1 : 0;
        PlayerPrefs.SetInt("SFX", on);
        PlayerPrefs.Save();

        if (MenuSFX.sfx != null)
            MenuSFX.sfx.SetActive(SFX.isOn);
    }

    public void MusicOnOff()
    {
        int on = Music.isOn ? 1 : 0;
        PlayerPrefs.SetInt("Music", on);
        PlayerPrefs.Save();

        if (MenuMusic.music != null)
            MenuMusic.music.SetActive(Music.isOn);
    }

    public void CrowdOnOff()
    {
        int on = Crowd.isOn ? 1 : 0;
        PlayerPrefs.SetInt("Crowd", on);
        PlayerPrefs.Save();
    }

    public void DifficultyChange()
    {
        float val = Difficulty.value;

        if (val >= 0f && val <= 0.15f)
        {
            PlayerPrefs.SetInt("Difficulty", 0);
            DifficultyText.text = "Easy";
        }
        else if (val > 0.15f && val <= 0.49f)
        {
            PlayerPrefs.SetInt("Difficulty", 1);
            DifficultyText.text = "Normal";
        }
        else if (val > 0.5f && val <= 0.84f)
        {
            PlayerPrefs.SetInt("Difficulty", 2);
            DifficultyText.text = "Hard";
        }
        else if (val > 0.85f && val <= 1f)
        {
            PlayerPrefs.SetInt("Difficulty", 3);
            DifficultyText.text = "Legendary";
        }

        PlayerPrefs.Save();
    }
}
