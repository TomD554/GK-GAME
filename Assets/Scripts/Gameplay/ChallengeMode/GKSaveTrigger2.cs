using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GKSaveTrigger2 : MonoBehaviour
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
            if (gamecontroller2.Saved == false)
            {
                Debug.Log("Ball entered glove trigger at: " + Time.time);
                gamecontroller2.Saved = true;
            }
        }
    }
}
