using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeObj : MonoBehaviour
{
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator FadeIn(string scene)
    {
        for (float i = 0; i <= 1f; i += Time.unscaledDeltaTime)
        {
            img.color = new Color(0, 0, 0, Mathf.Clamp01(i));
            yield return null;
        }
        img.color = new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }


    public IEnumerator FadeOut()
    {
        // loop over 1 second backwards
        for (float i = 1f; i >= 0; i -= Time.unscaledDeltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, Mathf.Clamp01(i));
            yield return null;
        }
        img.color = new Color(0f, 0f, 0f, 0f);
    }
}
