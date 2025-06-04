using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class misscollider : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            GameController.celebration.SetBool("miss", true);
            GameController.celebration2.SetBool("miss", true);
            GameController.DefenderAnim.SetBool("saved", true);
            GameController.Opponent2.transform.rotation = Quaternion.Euler(0, 240, 0);
        }
    }
}
