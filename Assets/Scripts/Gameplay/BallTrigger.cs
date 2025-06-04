using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        // if ball enters area then it triggers the shot on goal to happen
        if (other.tag == "Ball")
        {
            ballcontroller.inposition = true;
            Debug.Log("ShotTaken: " + Time.time);
        }
    }
}
