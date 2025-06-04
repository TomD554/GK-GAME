using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    public AudioClip[] soundtrack;

    public static GameObject music;

    void Awake()
    {
        music = GameObject.FindGameObjectWithTag("Music").transform.GetChild(0).gameObject;
        if (PlayerPrefs.HasKey("Music"))
        {
            int on = 1;
            if (PlayerPrefs.GetInt("Music") == on)
            {
                music.SetActive(true);
            }
            else
            {
                music.SetActive(false);
            }
        }
            DontDestroyOnLoad(music.transform.parent);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            music.GetComponent<AudioSource>().Stop();
        }
        else
        {
            if (!music.GetComponent<AudioSource>().isPlaying)
            {
                music.GetComponent<AudioSource>().clip = soundtrack[Random.Range(0, soundtrack.Length)];
                music.GetComponent<AudioSource>().Play();
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        if (!music.GetComponent<AudioSource>().playOnAwake)
        {
            music.GetComponent<AudioSource>().clip = soundtrack[Random.Range(0, soundtrack.Length)];
            music.GetComponent<AudioSource>().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!music.GetComponent<AudioSource>().isPlaying)
        {
            if (music.activeInHierarchy == false)
            {
                music.GetComponent<AudioSource>().Stop();
            }
            else
            {
                music.GetComponent<AudioSource>().clip = soundtrack[Random.Range(0, soundtrack.Length)];
                music.GetComponent<AudioSource>().Play();
            }
        }
    }
}