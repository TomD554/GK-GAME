using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggershot : MonoBehaviour
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
            ballcontroller.shot.SetBool("shot", true);
        }
    }
}
