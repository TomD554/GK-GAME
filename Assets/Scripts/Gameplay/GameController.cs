using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // THIS SCRIPT IS STRICTLY GAME CONTROLLING - SUCH AS SCORELINES, STARTING OBJECT POSITIONS, KIT TEXTURE LOADS AND STORING THE INFORMATION
    public static int Team1Score;
    public static int Team2Score;
    private GameObject ball;
    public static GameObject Player;
    public GameObject PlayerObj;
    public GameObject ballObj;
    Vector3 BallPos;
    private bool GOAL;
    public static bool isSpawned = true;
    public Image img;
    public static int ShotsCount;
    public static int SaveCount;
    public static int BlockCount;
    public static int MaxShots;
    public static bool Saved;
    public static bool Catch;
    public static bool Blocked;
    public static PlayableDirector timeline;
    public static GameObject Opponent2;
    public GameObject Opp2Obj;
    private Vector3 Opp2Pos;
    private Quaternion Opp2Rot;
    public static GameObject Opponent1;
    public GameObject Opp1Obj;
    public static GameObject Defender;
    public GameObject DefObj;
    private Vector3 DefPos;
    private Quaternion DefRot;
    private Vector3 Opp1Pos;
    private Quaternion Opp1Rot;
    private float ShotCooldown;
    public static float RespawnCountdown;
    private float BackToMenuCountdown;
    public Texture OppTexture;
    public static Animator DefenderAnim;
    private Rigidbody rbDef;
    private Vector3 velocity = Vector3.forward;
    private Vector3 DefenderRunPos;
    AudioSource StartWhistle;
    public static AudioSource crowd;
    public static AudioSource crowdmiss;
    public static AudioSource crowdgoal;
    public AudioSource FullTime;
    int whistleplayed = 0;
    SpriteRenderer BadgeGloveL;
    SpriteRenderer BadgeGloveR;
    public static Animator celebration;
    public static Animator celebration2;
    public GameObject CornerFlag;
    public static GameObject cam;
    public GameObject Net;
    private ClothSphereColliderPair[] netcollider;
    private int defrandompos;
    private GameObject SHOT_PARENT;
    private GameObject SHOTPARENTPREFAB;
    public GameObject LEFT_SIDE_SHOT_PREFAB;
    public GameObject RIGHT_SIDE_SHOT_PREFAB;
    public static bool leftshot;
    public static bool rightshot;
    private int leftorright;
    private float ShotPos_x;
    private Vector3 SHOTPOS;
    private Vector3 offset;
    public CloudSaveManager Instance;
    public GameObject Fadeobj;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = CloudSaveManager.Instance;
        cam = GameObject.Find("Main Camera");
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, PlayerPrefs.GetFloat("CameraZoom"));
        StartWhistle = GetComponent<AudioSource>();
        crowdmiss = GameObject.Find("CrowdMiss").GetComponent<AudioSource>();
        crowdgoal = GameObject.Find("CrowdGoal").GetComponent<AudioSource>();
        crowd = GameObject.Find("Crowd").GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("Crowd"))
        {
            int on = 1;
            if (PlayerPrefs.GetInt("Crowd") == on)
            {
                crowd.gameObject.SetActive(true);
                crowdgoal.gameObject.SetActive(true);
                crowdmiss.gameObject.SetActive(true);
                crowd.Play();
            }
            else
            {
                crowd.gameObject.SetActive(false);
                crowdgoal.gameObject.SetActive(false);
                crowdmiss.gameObject.SetActive(false);
            }
        }
        ball = GameObject.FindGameObjectWithTag("Ball");
        Player = GameObject.FindGameObjectWithTag("Player");
        Opponent2 = GameObject.FindGameObjectWithTag("Outfield2");
        Opponent1 = GameObject.FindGameObjectWithTag("Outfield1");
        Defender = GameObject.FindGameObjectWithTag("Defender");
        SHOT_PARENT = GameObject.FindGameObjectWithTag("ShotParent");
        Team1Score = 0;
        Team2Score = 0;
        ShotsCount = 1;
        MaxShots = 9;
        SaveCount = 0;
        BlockCount = 0;
        Saved = false;
        Catch = false;
        Blocked = false;
        isSpawned = true;
    }
    void Start()
    {

        Fadeobj = GameObject.Find("FADE_IMAGE");
        img = Fadeobj.GetComponent<Image>();
        img.color = new Color(0, 0, 0, 1);
        Player = GameObject.FindGameObjectWithTag("Player");
        Player.transform.position = new Vector3(-0.246f, -0.271f, -4.47f);
        netcollider = Net.GetComponent<Cloth>().sphereColliders;
        ballObj.SetActive(false);
        PlayerObj.SetActive(false);
        Opp1Obj.SetActive(false);
        Opp2Obj.SetActive(false);
        DefObj.SetActive(false);
        leftshot = true;
        rightshot = false;
        if (crowd.gameObject.activeInHierarchy == true)
        {
            StartCoroutine(PlayCustomLoop(crowd, 2));
        }
        else
        {

        }
        StartCoroutine(StartGameDelay());
        // countdown to menu after full time
        BackToMenuCountdown = 4f;
        // cooldown for shot to begin fade
        ShotCooldown = 6.6f;
        // countdown for respawn to begin
        RespawnCountdown = 7.6f;
        // saves count
        SaveCount = 0;
        // shots count
        ShotsCount = 1;
        // max shots count
        MaxShots = 9;
        // block count
        BlockCount = 0;
        // set ball position
        BallPos = ballObj.transform.position;
        // Scorelines
        Team1Score = 0;
        Team2Score = 0;
        // No Goal has been scored upon start
        Debug.Log(OppTexture);
        GOAL = false;
        Saved = false;
        Catch = false;
        Opp2Pos = Opponent2.transform.position;
        Opp2Rot = Opponent2.transform.rotation;
        Opp1Pos = Opponent1.transform.position;
        Opp1Rot = Opponent1.transform.rotation;
        DefRot = Defender.transform.rotation;
        defrandompos = Random.Range(0, 2);
        if (defrandompos == 0)
        {
            Debug.Log("RANDOM POS = 0");
            DefPos.x = -3.5f;

        }
        else if (defrandompos == 1)
        {
            Debug.Log("RANDOM POS = 1");
            DefPos.x = 6.0f;
        }
        DefPos.y = Defender.transform.position.y;
        DefPos.z = Defender.transform.position.z;
        Defender.transform.position = DefPos;
        rbDef = Defender.GetComponent<Rigidbody>();
        DefenderRunPos.x = 0f;
        DefenderRunPos.y = -1.381f;
        DefenderRunPos.z = 4f;
    }

    private void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (ball != null && Player != null)
        {
            netcollider[0] = new ClothSphereColliderPair(ball.GetComponent<SphereCollider>(), null);
            Net.GetComponent<Cloth>().sphereColliders = netcollider;
        }
        else if (ball == null && Player == null)
        {

        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        // maintain continue search for these gameobjects in fixedupdate
        ball = GameObject.FindGameObjectWithTag("Ball");
        Player = GameObject.FindGameObjectWithTag("Player");
        Opponent2 = GameObject.FindGameObjectWithTag("Outfield2");
        Opponent1 = GameObject.FindGameObjectWithTag("Outfield1");
        Defender = GameObject.FindGameObjectWithTag("Defender");
        SHOT_PARENT = GameObject.FindGameObjectWithTag("ShotParent");
        // countdown respawn time in fixedupdate
        RespawnCountdown -= Time.deltaTime;
        // if ball and player do not exist
        if (ball == null && Player == null)
        {
            // if it is now time for the respawn to begin
            if (RespawnCountdown <= 0f)
            {


                Instantiate(PlayerObj, PlayerObj.transform.position, PlayerObj.transform.rotation);
                leftorright = Random.Range(0, 2);
                if (leftorright == 0)
                {
                    leftshot = true;
                    rightshot = false;
                }
                else if (leftorright == 1)
                {
                    rightshot = true;
                    leftshot = false;
                }
                if (leftshot)
                {
                    SHOTPARENTPREFAB = LEFT_SIDE_SHOT_PREFAB;
                    ShotPos_x = Random.Range(-8, 7) * 0.5f;
                    if (ShotPos_x <= -2f)
                    {
                        DefPos.x = Random.Range(3, 6);
                        Debug.Log("Defender POS X: " + DefPos.x);
                        
                    }
                    else if (ShotPos_x >= -1.9f)
                    {
                        DefPos.x = Random.Range(-5, -1);
                        Debug.Log("Defender POS X: " + DefPos.x);
                    }
                }
                else if (rightshot)
                {
                    SHOTPARENTPREFAB = RIGHT_SIDE_SHOT_PREFAB;
                    ShotPos_x = Random.Range(-26, -11) * 0.5f;
                    if (ShotPos_x <= -8.1f)
                    {
                        DefPos.x = Random.Range(3, 6);
                        Debug.Log("Defender POS X: " + DefPos.x);
                    }
                    else if (ShotPos_x >= -8f)
                    {
                        DefPos.x = Random.Range(-5, -1);
                        Debug.Log("Defender POS X: " + DefPos.x);

                    }
                }
                SHOTPOS = SHOTPARENTPREFAB.transform.position;
                SHOTPOS.x = ShotPos_x;
                Debug.Log($"SHOT X: {ShotPos_x}");
                Instantiate(SHOTPARENTPREFAB, SHOTPOS, SHOTPARENTPREFAB.transform.rotation);
                Instantiate(DefObj, DefPos, DefRot);
                // Set GOAL to false so collider re-enables for goals to count again
                GOAL = false;
                Saved = false;
                Catch = false;
                Blocked = false;
                crowdmiss.Stop();
                crowdgoal.Stop();
                // newly spawned ball is no longer in position for shot to trigger
                ballcontroller.inposition = false;
                // fade out
                isSpawned = true;
                RespawnCountdown = 7.6f;
                ShotCooldown = 6.6f;
                ballcontroller.ShotCooldown = 2f;
                ballcontroller.ShotCountdown = 2.6f;
            }
        }
        // if ball exists
        else if (ball != null && Player != null)
        {
            celebration = Opponent1.GetComponent<Animator>();
            celebration2 = Opponent2.GetComponent<Animator>();
            DefenderAnim = Defender.GetComponent<Animator>();
            if (ballcontroller.ShotTaken == false)
            {
               // Defender.transform.position = Vector3.SmoothDamp(Defender.transform.position, DefenderRunPos, ref velocity, 2.8f);
                if (DefPos.x > 0.0f && DefenderAnim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    DefenderAnim.SetBool("rightside", true);
                }
                if (DefPos.x < 0.0f && DefenderAnim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    DefenderAnim.SetBool("rightside", false);
                }
            }
            if (leftshot == true)
            {
                Opponent2.GetComponent<Animator>().SetBool("LeftShot", true);
                Opponent1.GetComponent<Animator>().SetBool("RightShot", false);
            }
            else if (rightshot == true)
            {
                Opponent2.GetComponent<Animator>().SetBool("LeftShot", false);
                Opponent1.GetComponent<Animator>().SetBool("RightShot", true);
            }
            if (ShotsCount < MaxShots)
            {
                if (Saved == true)
                {
                    Blocked = false;
                   
                    SavedFunction();

                }
                if (GOAL == true)
                {
                    GoalFunction();
                }
                // begin shot cooldown
                ShotCooldown -= Time.deltaTime;
                // if cooldown has completed, begin fade and destroy objects
                if (ShotCooldown <= 0f)
                {
                    SaveGoalBlockCheck();
                    StartCoroutine(FadeInOut());
                    if (ballcontroller.ShotTaken == true)
                    {
                        if (SaveCount >= 3)
                        {
                            StartCoroutine(ResetSaveCountDelayed(SaveCount > 3 ? 1 : 0));
                            Team1Score++;
                        }
                        if (BlockCount == 2)
                        {
                            MaxShots++;
                            BlockCount = 0;
                            Debug.Log("MAX SHOTS: " + MaxShots);
                        }
                        ShotsCount++;
                        // set back to false (beginning restart of play)
                        ballcontroller.ShotTaken = false;
                        // destroy the ball and the player, preparing for respawn and restart of play
                        DestroyPlayObjects();
                    }

                    Debug.Log(ShotsCount);
                }
            }
            // if shotscount = 9 that means it is now full time
            else if (ShotsCount == MaxShots)
            {
                if (Saved == true)
                {
                    Blocked = false;
                   
                    SavedFunction();

                }
                if (GOAL == true)
                {
                    GoalFunction();
                }
                ShotCooldown -= Time.deltaTime;
                if (ShotCooldown <= 0f)
                {
                    SaveGoalBlockCheck();
                    if (ballcontroller.ShotTaken == true)
                    {
                        if (SaveCount >= 3)
                        {
                            StartCoroutine(ResetSaveCountDelayed(SaveCount > 3 ? 1 : 0));
                            Team1Score++;
                        }
                        if (BlockCount == 2)
                        {
                            MaxShots++;
                            BlockCount = 0;
                            StartCoroutine(FadeInOut());
                        }

                        if (ShotsCount < MaxShots)
                        {
                            DestroyPlayObjects();
                            ShotsCount++;
                            ballcontroller.ShotTaken = false;
                        }
                        else
                        {
                            ShotsCount++;
                        }
                    }
                }
            }
            // if shots count is greater than maximum shots (full time), countdown back to the menu and reload menu
            else if (ShotsCount > MaxShots)
            {
                if (Saved == true)
                {
      
                    SavedFunction();
                }
                if (GOAL == true)
                {
                    GoalFunction();
                }
                FullTimeFunction();
            }
        }
    }

    private void LateUpdate()
    {
        if (ball != null && Catch == true)
        {
            CatchFunction();
        }
    }

    // USING GOAL LINE COLLIDER AS TRIGGER TO DETERMINE GOAL
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            // if a Goal has not been determined and ball enters trigger, goal will be given
            // (this is to avoid a second goal given if ball bounces through trigger a second time after it goes in)
            if (GOAL == false)
            {
                Team2Score++;
                GOAL = true;            
                DefenderAnim.SetBool("conceded", true);
                Debug.Log(Team2Score);
            }
      }
    }

    void CatchFunction()
    {
        offset = new Vector3(0f, 0f, 0.2f);
        ball.transform.position = Player.transform.position + offset;
        ball.transform.rotation = Player.transform.rotation;
    }

    void SavedFunction()
    {
        Opponent1.transform.rotation = Quaternion.Euler(0, 240, 0);
        Opponent2.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (!crowdmiss.isPlaying)
        {
            if (crowdmiss.gameObject.activeInHierarchy == true)
            {
                crowdmiss.Play();
            }
            else
            {

            }
        }
    }

    void GoalFunction()
    {
        Opponent1.transform.rotation = Quaternion.Euler(0, 240, 0);
        Opponent2.transform.rotation = Quaternion.Euler(0, 240, 0);
        celebration.SetBool("goal", true);
        celebration2.SetBool("goal", true);
        Saved = false;
        Blocked = false;
        if (!crowdgoal.isPlaying)
        {
            if (crowdgoal.gameObject.activeInHierarchy == true)
            {
                crowdgoal.time = 3;
                crowdgoal.Play();
            }
            else
            {

            }

        }
    }

    void FullTimeFunction()
    {
        BackToMenuCountdown -= Time.deltaTime;
        if (!FullTime.isPlaying && whistleplayed == 0)
        {
            if (MenuSFX.sfx == null)
            {

            }
            else if (MenuSFX.sfx != null)
            {
                if (MenuSFX.sfx.activeInHierarchy == false)
                {

                }
                else
                {
                    FullTime.Play();
                }

                whistleplayed++;
            }
            Instance.IncrementMatchesPlayed();
            if (Team2Score == 0)
            {
                Instance.IncrementCleanSheets();
            }
            if (Team1Score > Team2Score)
            {
                Instance.IncrementMatchesWon();
            }
        }
        if (BackToMenuCountdown <= 0)
        {
            if (SeasonModeUI.SeasonGame == false)
            {
                SceneManager.LoadScene("PostGameMenu");
            }
            else if (SeasonModeUI.SeasonGame == true)
            {
                SceneManager.LoadScene("PostSeasonGameMenu");
            }
        }
    }

    void SaveGoalBlockCheck()
    {
        if (Blocked == true)
        {
            BlockCount++;
            Debug.Log("BLOCKS: " + BlockCount);
        }
        if (Saved == true)
        {
            if (Catch == false)
            {
                SaveCount++;

            }
            else
            {
                SaveCount += 2;
            }
            Instance.IncrementTotalSaves();
        }
        if (GOAL == true)
        {
            Saved = false;
            SaveCount = 0;
        }
    }

    void DestroyPlayObjects()
    {
        Destroy(ball);
        Destroy(Player);
        Destroy(Opponent2);
        Destroy(Opponent1);
        Destroy(SHOT_PARENT);
        Destroy(Defender);
        isSpawned = false;
        Debug.Log("End of Chance: " + Time.time);
    }

    // fade in and out code
    IEnumerator FadeInOut()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, Mathf.Clamp01(i));
            yield return null;
        }
        img.color = new Color(0f, 0f, 0f, 1f);
        //Temp to Fade Out
        yield return new WaitForSeconds(0.5f);

        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, Mathf.Clamp01(i));
            yield return null;
        }
        img.color = new Color(0f, 0f, 0f, 0f);
    }

    IEnumerator StartGameDelay()
    {
        Time.timeScale = 0;
        // loop over 1 second backwards
        yield return null; // ensures the first frame renders

        for (float i = 1.5f; i >= 0; i -= Time.unscaledDeltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        // Play countdown via UI script
        UI ui = FindObjectOfType<UI>();
        if (ui != null)
        {
            yield return ui.PlayCountdown();
        }

        if (MenuSFX.sfx != null && MenuSFX.sfx.activeInHierarchy)
        {
            StartWhistle.Play();
        }
        ballObj.SetActive(true);
        PlayerObj.SetActive(true);
        Opp1Obj.SetActive(true);
        Opp2Obj.SetActive(true);
        DefObj.SetActive(true);
        GetComponent<UI>().InGame.gameObject.SetActive(true);
        GetComponent<UI>().PauseBtn.gameObject.SetActive(true);
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(0.5f);
        StopCoroutine(StartGameDelay());
    }
    private IEnumerator ResetSaveCountDelayed(int newValue)
    {
            GameController.SaveCount = 3; // Force the UI to show 3 saves
            yield return new WaitForSeconds(0.3f); // Let the UI catch up and show it
            GameController.SaveCount = newValue;
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
