using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTrigger2 : MonoBehaviour
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
            if (gamecontroller2.Saved == false)
            {
                gamecontroller2.Saved = true;
            }
           
        }
    }
}
