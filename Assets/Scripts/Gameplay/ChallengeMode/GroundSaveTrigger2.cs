using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundSaveTrigger2 : MonoBehaviour
{
    public Transform glove; // Assign this in Inspector (your Glove GameObject)
    public float requiredDistance = 1.5f; // How close the glove must be to count as a save

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            float distanceToGlove = Vector3.Distance(other.transform.position, glove.position);

            if (distanceToGlove <= requiredDistance)
            {
                Debug.Log("Ground SAVE detected! Glove was close enough.");
                if (gamecontroller2.Saved == false)
                {
                    gamecontroller2.Saved = true;
                }
            }
            else
            {
                Debug.Log("Missed Ground Save - Glove too far!");
            }
        }
    }
}