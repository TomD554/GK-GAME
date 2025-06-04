using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundSaveTrigger : MonoBehaviour
{
    public Transform glove; // Assign this in Inspector (your Glove GameObject)
    public float requiredDistance = 1.5f; // How close the glove must be to count as a save

    private void Start()
    {
        glove = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
       
        if (GameController.Player == null)
        {

        }
        else 
        {
            glove = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (glove != null)
        {
            if (other.CompareTag("Ball"))
            {
                float distanceToGlove = Vector3.Distance(other.transform.position, glove.position);

                if (distanceToGlove <= requiredDistance)
                {
                    Debug.Log("Ground SAVE detected! Glove was close enough.");
                    if (GameController.Saved == false)
                    {
                        GameController.Saved = true;
                    }
                }
                else
                {
                    Debug.Log("Missed Ground Save - Glove too far!");
                }
            }
        }
    }
}