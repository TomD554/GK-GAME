using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GKController : MonoBehaviour
{
    private Collider Coll;
    public GameObject Plane;
    public GameObject Player;
    public Sprite SetSprite;
    public Sprite scoop;
    public Sprite caught;

    private Vector3 velocity = Vector3.zero;
    private Vector3 ControlPos;
    private Vector3 neutralPos; // reference starting position
    private bool allowInput = false;
    private float inputDelay = 0.2f;

    public float maxTiltAngle = 95f; // max rotation in either direction
    public float tiltSensitivity = 25f; // higher = less sensitive

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            Vector3 startPos = new Vector3(-0.246f, -0.271f, -4.47f);
            Player.transform.position = startPos;
            neutralPos = startPos;

            Coll = Plane.GetComponent<Collider>();
        }

        StartCoroutine(EnableInputAfterDelay());
    }

    void FixedUpdate()
    {
        if (!allowInput || Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            Vector3 mousePos = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(castPoint, out hit, float.PositiveInfinity))
            {
                if (Coll.Raycast(castPoint, out hit, 100.0f))
                {
                    // Move hand
                    ControlPos = Vector3.SmoothDamp(Player.transform.position, hit.point, ref velocity, 0.01f);
                    ControlPos.x = Mathf.Clamp(ControlPos.x, -3.4f, 3.1f);
                    ControlPos.y = Mathf.Clamp(ControlPos.y, -1f, 1f);
                    ControlPos.z = -4.42f;
                    Player.transform.position = ControlPos;

                    // ROTATE GLOVES BASED ON OFFSET FROM NEUTRAL POSITION
                    float xOffset = ControlPos.x - neutralPos.x; // how far left/right
                    float yOffset = ControlPos.y - neutralPos.y; // how far up/down (optional)

                    float zRotation = Mathf.Clamp(xOffset * tiltSensitivity, -maxTiltAngle, maxTiltAngle);
                    Player.transform.rotation = Quaternion.Euler(0f, 0f, -zRotation); // - to mirror glove lean

                    // SPRITE CONTROL
                    SpriteRenderer sr = Player.GetComponent<SpriteRenderer>();

                    // Check scoop zone first
                    if (hit.point.x > -0.8f && hit.point.x < 0.8f && hit.point.y < -0.8f)
                    {
                        sr.sprite = scoop;
                        Player.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    }
                    else
                    {
                        // All other zones get SetSprite or Caught
                        if (hit.point.x < -0.8f && hit.point.y < -0.8f)
                        {
                            Player.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                        }
                        else if (hit.point.x > 0.8f && hit.point.y < -0.8f)
                        {
                            Player.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                        }

                        // Set default sprite
                        sr.sprite = SetSprite;

                        // Override with caught only if not scoop zone
                        if (GameController.Catch)
                        {
                            sr.sprite = caught;
                        }
                    }
                }
            }

            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
        }
    }

    IEnumerator EnableInputAfterDelay()
    {
        allowInput = false;
        yield return new WaitForSeconds(inputDelay);
        allowInput = true;
    }
}
