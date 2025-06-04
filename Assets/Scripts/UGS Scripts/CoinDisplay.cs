using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinDisplay : MonoBehaviour
{
    public Text coinText;
    private Coroutine animateCoroutine;

    void OnEnable()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnCoinsChanged += AnimateCoinChange;
            UpdateUI(EconomyManager.Instance.CurrentCoins); // show current immediately
        }
    }

    void OnDisable()
    {
        if (EconomyManager.Instance != null)
            EconomyManager.Instance.OnCoinsChanged -= AnimateCoinChange;
    }

    void UpdateUI(int newAmount)
    {
        coinText.text = newAmount.ToString();
    }

    void AnimateCoinChange(int targetAmount)
    {
        if (animateCoroutine != null)
            StopCoroutine(animateCoroutine);

        animateCoroutine = StartCoroutine(AnimateCoinsCoroutine(targetAmount));
    }

    IEnumerator AnimateCoinsCoroutine(int targetAmount)
    {
        int displayedAmount = int.Parse(coinText.text);
        float duration = 0.5f; // seconds to animate
        float elapsed = 0f;
        int startAmount = displayedAmount;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int newAmount = Mathf.RoundToInt(Mathf.Lerp(startAmount, targetAmount, t));
            coinText.text = newAmount.ToString();
            yield return null;
        }

        coinText.text = targetAmount.ToString();
    }
}
