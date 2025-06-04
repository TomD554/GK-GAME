using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSettings2 : MonoBehaviour
{
    public Toggle SFX;
    public Toggle Crowd;
    public Slider CameraZoom;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("CameraZoom"))
        {
            PlayerPrefs.SetFloat("CameraZoom", -10f);
        }
        else
        {
            float zoom = PlayerPrefs.GetFloat("CameraZoom");
            CameraZoom.value = zoom;
        }
        if (!PlayerPrefs.HasKey("Crowd"))
        {
            PlayerPrefs.SetInt("Crowd", 1);
        }
        else
        {
            int on = 1;
            if (PlayerPrefs.GetInt("Crowd") == on)
            {
                Crowd.isOn = true;
            }
            else
            {
                Crowd.isOn = false;
            }
        }
        if (!PlayerPrefs.HasKey("SFX"))
        {
            PlayerPrefs.SetInt("SFX", 1);
        }
        else
        {
            int on = 1;
            if (PlayerPrefs.GetInt("SFX") == on)
            {
                SFX.isOn = true;
            }
            else
            {
                SFX.isOn = false;
            }
        }
    }
    public void CameraZoomChange()
    {
        float zoom = CameraZoom.value;
        PlayerPrefs.SetFloat("CameraZoom", zoom);
        gamecontroller2.cam.transform.position = new Vector3(gamecontroller2.cam.transform.position.x, gamecontroller2.cam.transform.position.y, PlayerPrefs.GetFloat("CameraZoom"));
        PlayerPrefs.Save();
    }

    public void SFXOnOff()
    {
        if (SFX.isOn)
        {
            PlayerPrefs.SetInt("SFX", 1);
            MenuSFX.sfx.SetActive(true);
        }
        else if (!SFX.isOn)
        {
            PlayerPrefs.SetInt("SFX", 0);
            if (MenuSFX.sfx == null)
            {

            }
            else if (MenuSFX.sfx != null)
            {
                MenuSFX.sfx.SetActive(false);
            }
        }
    }
    public void CrowdOnOff()
    {
        if (Crowd.isOn)
        {
            PlayerPrefs.SetInt("Crowd", 1);
            gamecontroller2.crowd.gameObject.SetActive(true);
            StartCoroutine(GetComponent<gamecontroller2>().PlayCustomLoop(gamecontroller2.crowd, 2));
            gamecontroller2.crowd.Pause();
        }
        else if (!Crowd.isOn)
        {
            PlayerPrefs.SetInt("Crowd", 0);
            gamecontroller2.crowd.gameObject.SetActive(false);

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
