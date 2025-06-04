using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class ballcontroller : MonoBehaviour
{
    // THIS SCRIPT IS STRICTLY THE BALL MOVEMENT CONTROLLER
    public GameObject ball;
    public GameObject Player;
    public static bool ShotTaken;
    public GameObject BallPos;
    private Vector3 velocity = Vector3.zero;
    private Vector3 BallPosVector;
    public static bool inposition;
    public static Animator pass;
    public static Animator shot;
    private GameObject Opponent2;
    private GameObject Opponent1;
    public static float ShotCooldown;
    public static float ShotCountdown;
    public GameObject BallCoords;
    public GameObject Attackrunpos;
    private Vector3 AttackRunVector;
    public AudioSource kick;
    public Transform goalTopLeft;     
    public Transform goalBottomRight;
    int kicked = 0;
    // Start is called before the first frame update
    void Start()
    {
        ShotCountdown = 2.6f;
        ShotCooldown = 2f;
        // Setting both bools to false to start, as they will be required to determine the restart of the scene
        ShotTaken = false;
        inposition = false;
        BallPosVector.x = 0.03f;
        BallPosVector.y = -1.381f;
        BallPosVector.z = 8.23f;
        AttackRunVector.x = 0.11f;
        AttackRunVector.y = -1.381f;
        AttackRunVector.z = 8.32f;
        // Start the function to wait for the pass upon loading the scene
        // StartCoroutine(WaitForPass());
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        ball = GameObject.FindGameObjectWithTag("Ball");
        Player = GameObject.FindGameObjectWithTag("Player");
        Opponent2 = GameObject.FindGameObjectWithTag("Outfield2");
        Opponent1 = GameObject.FindGameObjectWithTag("Outfield1");
        if (ball != null && ball.GetComponent<Rigidbody>().isKinematic)
            return; // Skip logic if ball is frozen
        if (ball == null)
        {
            kicked = 0;
        }

        // if ball exists
        else if (ball != null)
        {
            pass = Opponent2.GetComponent<Animator>();
            shot = Opponent1.GetComponent<Animator>();
            if (GameController.ShotsCount <= GameController.MaxShots)
            {
                // shot cooldown is 2.6 seconds, count down to zero
                ShotCooldown -= Time.deltaTime;
                // shot countdown is 2 seconds, count down to zero
                ShotCountdown -= Time.deltaTime;
                if (ShotTaken == false && inposition == false && GameController.isSpawned == true && ShotCooldown > 0 && ShotCooldown <= 2f)
                {
                    //Opponent1.transform.position = Vector3.SmoothDamp(Opponent1.transform.position, Attackrunpos.transform.position, ref velocity, 1.5f);
                    //Opponent2.transform.position = Vector3.SmoothDamp(Opponent2.transform.position, BallCoords.transform.position, ref velocity, 1.5f);

                }
                if (ShotCooldown > 0 && ShotCooldown <= 0.3f)
                {
                    pass.SetBool("pass", true);
                }
                if (ShotTaken == false && inposition == false && GameController.isSpawned == true && ShotCooldown <= 0f)
                {
                    //ball.transform.position = Vector3.SmoothDamp(ball.transform.position, BallPosVector, ref velocity, 0.4f);
                    PassBall(BallPosVector);
                    ball.GetComponent<Rigidbody>().drag = 0.5f;
                    if (!kick.isPlaying && kicked == 0)
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
                                kick.Play();
                            }

                            kicked++;
                        }
                    }
                }
                if (ShotCountdown <= 0f)
                {
                   
                }
                // if ball is in position, shot will be taken
                if (ShotTaken == false && inposition == true)
                {
                    
                    BallMovement();
                    if (!kick.isPlaying && kicked == 1)
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
                                kick.Play();
                            }
                            kicked++;
                        }
                    }

                }
            }
        }

        // function for the ball movement, called in FixedUpdate
        void BallMovement()
        {
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // Pick a random point inside the goalmouth rectangle
            float targetX = Random.Range(goalTopLeft.position.x, goalBottomRight.position.x);
            //  float random01 = Random.Range(0f, 1f);
            float targetY = Random.Range((int)(goalBottomRight.position.y * 4), (int)(goalTopLeft.position.y + 1) * 4) / 4f;
            float targetZ = goalTopLeft.position.z; // Assume goal is a flat Z plane

            Vector3 targetPoint = new Vector3(targetX, targetY, targetZ);

            // Now find the direction vector from ball to target
            Vector3 direction = (targetPoint - ball.transform.position).normalized;

            // Set the shot speed based on difficulty
            int difficulty = PlayerPrefs.GetInt("Difficulty");
            float shotPower = 0f;

            if (difficulty == 0) shotPower = 24f;
            else if (difficulty == 1) shotPower = 28f;
            else if (difficulty == 2) shotPower = 30f;
            else if (difficulty == 3) shotPower = 36f;

            // Apply force toward the random goal point
            ball.GetComponent<Rigidbody>().AddForce(direction * shotPower, ForceMode.Impulse);

            Debug.Log("Ball Shot Toward: " + targetPoint + " with Power: " + shotPower);
            ShotTaken = true;
            pass.SetBool("pass", false);
            shot.SetBool("shot", false);

        }

    }
    public void PassBall(Vector3 BallPosVector)
    {
        if (GameController.leftshot == true)
        {
            // Assign the direction for the ball's movement
            this.BallPosVector = new Vector3(-15, 0.0f, 2f);
        }
        else if (GameController.rightshot == true)
        {
            this.BallPosVector = new Vector3(15, 0.0f, 2f);
        }
        // Apply force to the Rigidbody for a more natural motion
        ball.GetComponent<Rigidbody>().velocity = BallPosVector.normalized * 10f;

        Vector3 axisOfRotation = Vector3.Cross(ball.GetComponent<Rigidbody>().velocity, Vector3.up); // Spin based on velocity
        ball.GetComponent<Rigidbody>().angularVelocity = axisOfRotation * -10f;
    }
}
