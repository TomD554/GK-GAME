using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCountdown : MonoBehaviour
{
    public Text Timer; // Reference to your UI text element
    public float countdownTime = 60.0f; // 1 minute in seconds

    void Start()
    {
    }

   public IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(1f);
        while (countdownTime > 0.0f)
        {
            // Update minutes and seconds
            int minutes = Mathf.FloorToInt(countdownTime / 60.0f);
            int seconds = Mathf.FloorToInt(countdownTime % 60.0f);

            // Update UI text with the current countdown time
            Timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Wait for the next frame
            yield return null;

            // Decrease countdownTime by Time.deltaTime
            countdownTime -= Time.deltaTime;
        }

        GetComponent<gamecontroller2>().gameState = gamecontroller2.GameStates.FullTime;
    }
}
