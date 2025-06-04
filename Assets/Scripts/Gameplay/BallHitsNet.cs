using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitsNet : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Ball")
        {
            collision.gameObject.GetComponent<SphereCollider>().material = null;
        }
    }
}
