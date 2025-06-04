using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PostChallengeMenu : MonoBehaviour
{
    public Text ShotsCount;
    public Text SavesCount;
    public Text Percentage;
    public Text HighScore;
    private int HighScoreTarget;
    private int SavesCountTarget;
    private int ShotsCountTarget;
    private int PercentageTarget;
   // public Text RecordPercentage;
   // public Text RecordSaveCount;
    // Start is called before the first frame update
    void Start()
    {
        HighScoreTarget = PlayerPrefs.GetInt("Percentage");
        SavesCountTarget = gamecontroller2.savecount;
        ShotsCountTarget = gamecontroller2.ShotsCount;
        PercentageTarget = gamecontroller2.percentage;
        StartCoroutine(SavesIncrement(SavesCount, SavesCountTarget));
        if (gamecontroller2.NewHighScore)
        {
            HighScore.text = "";
        }
        else
        {
            HighScore.text = PlayerPrefs.GetInt("Percentage").ToString() + "%";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBtnClick()
    {
        SceneManager.LoadScene("Menu");
    }

   IEnumerator NewHighScoreIncrement(Text text, int target)
    {
        int num = 0;
        while (num <= target)
        {
            text.text = num.ToString() + "%";
            num++;
            yield return new WaitForSeconds(0.05f);
        }
       
    }

    IEnumerator SavesIncrement(Text text, int target)
    {
        int num = 0;
        while (num <= target)
        {
            text.text = num.ToString();
            num++;
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(ShotsIncrement(ShotsCount, ShotsCountTarget));
    }

    IEnumerator ShotsIncrement(Text text, int target)
    {
        int num = 0;
        while (num <= target)
        {
            text.text = num.ToString();
            num++;
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(PercentageIncrement(Percentage, PercentageTarget));
    }
    IEnumerator PercentageIncrement(Text text, int target)
    {
        if (gamecontroller2.NewHighScore)
        {
            StartCoroutine(NewHighScoreIncrement(HighScore, HighScoreTarget));
        }
        int num = 0;
        while (num <= target)
        {
            text.text = num.ToString() + "%";
            num++;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
