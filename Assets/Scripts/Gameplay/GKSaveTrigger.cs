using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GKSaveTrigger : MonoBehaviour
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
        if (other.tag == "Ball")
        {
            //IF BALL HITS GLOVES, BALL = SAVED
            GameController.Saved = true;
            Debug.Log("ShotTaken: " + Time.time);
        }
    }
}
