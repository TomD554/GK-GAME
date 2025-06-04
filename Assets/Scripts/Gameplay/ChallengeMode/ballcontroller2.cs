using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballcontroller2 : MonoBehaviour
{
public Transform goalTopLeft;     // Top Left corner of goal (assign in Inspector)
public Transform goalBottomRight; // Bottom Right corner of goal (assign in Inspector)

public void BallMovement(GameObject ball)
{
    // Pick a random point inside the goalmouth rectangle
        float targetX = Random.Range(goalTopLeft.position.x, goalBottomRight.position.x);
        float targetY = Random.Range((int)(goalBottomRight.position.y * 4), (int)(goalTopLeft.position.y + 1) * 4) / 4f;
        float targetZ = goalTopLeft.position.z; // Assume goal is a flat Z plane

    Vector3 targetPoint = new Vector3(targetX, targetY, targetZ);

    // Now find the direction vector from ball to target
    Vector3 direction = (targetPoint - ball.transform.position).normalized;

    // Set the shot speed based on difficulty
    int difficulty = PlayerPrefs.GetInt("Difficulty");
    float shotPower = 0f;

    if (difficulty == 0) shotPower = 24f;
    else if (difficulty == 1) shotPower = 28f;
    else if (difficulty == 2) shotPower = 30f;
    else if (difficulty == 3) shotPower = 35f;

    // Apply force toward the random goal point
    ball.GetComponent<Rigidbody>().AddForce(direction * shotPower, ForceMode.Impulse);

    Debug.Log("Ball Shot Toward: " + targetPoint + " with Power: " + shotPower);
}

}
