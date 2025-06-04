using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class gamecontroller2 : MonoBehaviour
{
    public Image img;
    public static GameObject cam;
    public GameObject ball;
    public GameObject Ball2Kill;
    public GameObject Player;
    private float countdownTime = 3.0f;
    public Text countdown;
    public enum GameStates {PreGame, InGame, FullTime}
    public GameStates gameState;
    AudioSource StartWhistle;
    public static AudioSource crowd;
    private TimerCountdown countdownTimerScript;
    public static bool Saved;
    public static bool ShotTaken;
    public static bool GOAL;
    public GameObject Net;
    private ClothSphereColliderPair[] netcollider;
    public static int savecount;
    public GameObject BallPrefab;
    private GameObject NewBall;
    public Transform SpawnPoint;
    public static int ShotsCount;
    public static int percentage;
    public static bool NewHighScore;
    // Start is called before the first frame update
    private void Awake()
    {
        ShotsCount = 0;
        NewHighScore = false;
    }
    void Start()
    {
        StartCoroutine(StartGameDelay());
        netcollider = new ClothSphereColliderPair[1];
        MenuController.ChallengeMode = true;
        ShotsCount = 0;
        cam = GameObject.Find("Main Camera");
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, PlayerPrefs.GetFloat("CameraZoom"));
        ShotTaken = false;
        ball = GameObject.FindGameObjectWithTag("Ball");
        countdownTimerScript = GetComponent<TimerCountdown>();
        gameState = GameStates.PreGame;
        crowd = GameObject.Find("Crowd").GetComponent<AudioSource>();
        StartCoroutine(PlayCustomLoop(crowd, 2));
        StartWhistle = GetComponent<AudioSource>();
        savecount = 0;
        StartCoroutine(BallSpawn());
     
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (ball == null)
        {
            GameObject foundBall = GameObject.FindGameObjectWithTag("Ball");
            if (foundBall != null)
                ball = foundBall;
        }
        if (ball != null && Player != null)
        {
            SphereCollider ballCollider = ball.GetComponent<SphereCollider>();
            if (ballCollider != null)
            {
                netcollider[0] = new ClothSphereColliderPair(ballCollider, null);
                Net.GetComponent<Cloth>().sphereColliders = netcollider;
            }
        }
        if (gameState == GameStates.FullTime)
        {
            Destroy(ball);
            StartCoroutine(FullTimeCountdown());
            
        }
        netcollider = Net.GetComponent<Cloth>().sphereColliders;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            // if a Goal has not been determined and ball enters trigger, goal will be given
            // (this is to avoid a second goal given if ball bounces through trigger a second time after it goes in)
            if (GOAL == false)
            {
               GOAL = true;
            }
        }
    }

    IEnumerator BallSpawn()
    {
        yield return new WaitUntil(() => GetComponent<gamecontroller2>().gameState == GameStates.InGame);

        // Add this line to delay the first shot by 1 second
        yield return new WaitForSeconds(1f);

        while (GetComponent<gamecontroller2>().gameState == GameStates.InGame && GetComponent<TimerCountdown>().countdownTime > 1.0f) 
        {
            if (ShotTaken == false)
            {
                GetComponent<ballcontroller2>().BallMovement(ball);
                ShotTaken = true;
                ShotsCount++;
                Debug.Log("Shots Taken: " + ShotsCount);
                StartCoroutine(DestroyAfterSeconds(ball,5f));
            }
            yield return new
            WaitForSeconds(1f);
            NewBall = Instantiate(BallPrefab, new Vector3(Random.Range(-8.1f, 8.1f), SpawnPoint.position.y, SpawnPoint.position.z), SpawnPoint.rotation);
            // Give time for all physics triggers to fire
            yield return new WaitForSeconds(0.3f);

            // Evaluate the result now
            if (Saved && !GOAL)
            {
                savecount++;
                Debug.Log("✅ Save counted");
            }
            else if (GOAL)
            {
                Debug.Log("❌ Goal scored");
            }
            else
            {
                Debug.Log("➖ Missed or no contact");
            }

            // Cleanup/reset
            ball = NewBall;
            ShotTaken = false;
            Saved = false;
            GOAL = false;

        }
    }

    IEnumerator DestroyAfterSeconds(GameObject ballToDestroy, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (ballToDestroy != null)
        {
            Destroy(ballToDestroy);
        }
    }

    IEnumerator StartGameDelay()
    {
        Time.timeScale = 0;
        // loop over 1 second backwards
        for (float i = 1.5f; i >= 0; i -= Time.unscaledDeltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        // Play countdown via UI script
        UIController ui = FindObjectOfType<UIController>();
        if (ui != null)
        {
            yield return ui.PlayCountdown();
        }

        if (MenuSFX.sfx != null && MenuSFX.sfx.activeInHierarchy)
        {
            StartWhistle.Play();
        }
        Player.SetActive(true);
        ball.SetActive(true);
        Time.timeScale = 1;
        GetComponent<UIController>().PauseBtn.gameObject.SetActive(true);
        GetComponent<UIController>().InGame.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        gameState = GameStates.InGame;
        countdownTimerScript.StartCoroutine(countdownTimerScript.StartCountdown());
        StopCoroutine(StartGameDelay());
    }

    IEnumerator FullTimeCountdown()
    {
        PlayerPrefs.SetInt("ChallengeSaves", savecount);
        percentage = Mathf.RoundToInt(((float)savecount / ShotsCount) * 100);
        if (percentage > PlayerPrefs.GetInt("Percentage"))
        {
            PlayerPrefs.SetInt("Percentage", percentage);
            NewHighScore = true;
            PlayerPrefs.Save();
        }
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("PostChallengeMenu");
    }

    public IEnumerator PlayCustomLoop(AudioSource sound, float endIntro)
    {
        if (PlayerPrefs.HasKey("Crowd"))
        {
            int on = 1;
            if (PlayerPrefs.GetInt("Crowd") == on)
            {
                sound.loop = false;
                float l = sound.clip.length - 4;
                int t = 0;
                sound.Play();
                yield return new WaitForSeconds(endIntro);
                t = sound.timeSamples;
                yield return new WaitForSeconds(l - endIntro);
            LOOP:
                sound.timeSamples = t;
                sound.Play();
                yield return new WaitForSeconds(l - endIntro);
                goto LOOP;
            }

        }

    }
}
