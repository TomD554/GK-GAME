using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSFX : MonoBehaviour
{
    public static GameObject sfx;

    void Awake()
    {
        if (PlayerPrefs.HasKey("SFX"))
        {
            int on = 1;
            if (PlayerPrefs.GetInt("SFX") == on)
            {
                GameObject SFXParent = GameObject.FindGameObjectWithTag("SFX");
                if (SFXParent == null)
                { }
                else
                 { 
                    sfx = SFXParent.transform.GetChild(0).gameObject;
                    Debug.Log(sfx.name);
                    sfx.SetActive(true);
                    DontDestroyOnLoad(SFXParent);
                }
            }
            else
            {
                GameObject SFXParent = GameObject.FindGameObjectWithTag("SFX");
                sfx = SFXParent.transform.GetChild(0).gameObject;
                sfx.SetActive(false);
                DontDestroyOnLoad(SFXParent);
            }
        }

    }

    public void OnBtnClick()
    {
        if (sfx == null)
        {

        }
        else if (sfx != null)
        {
            if (sfx.activeInHierarchy == false)
            {

            }
            else
            {
                sfx.GetComponent<AudioSource>().Play();
            }
        }
    }
}
