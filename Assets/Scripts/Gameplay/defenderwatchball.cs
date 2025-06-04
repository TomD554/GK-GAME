using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defenderwatchball : MonoBehaviour
{
    private GameObject ballattackpos;
    // Start is called before the first frame update
    void Start()
    {
        ballattackpos = GameObject.Find("BallAttackPosition");
    }

    // Update is called once per frame
    void Update()
    {
        if (ballattackpos != null)
        {


            if (ballcontroller.ShotTaken == false && transform.position.x > 0.0f)
            {
                transform.LookAt(new Vector3(ballattackpos.transform.position.x, transform.position.y, ballattackpos.transform.position.z));
            }
            else if (ballcontroller.ShotTaken == false && transform.position.x < 0.0f)
            {
                transform.LookAt(new Vector3(ballattackpos.transform.position.x + 0.5f, transform.position.y, ballattackpos.transform.position.z));
            }

        }
    }
}
