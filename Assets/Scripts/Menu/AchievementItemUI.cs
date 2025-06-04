using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class AchievementItemUI : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;
    public Text progressText;
    public Image icon;
    public GameObject lockOverlay;
    public Sprite Locked;
    public Sprite Unlocked;
    public Button claimBtn;
    public Text claimBtnText;
    public Text coinReward;
    public CloudSaveManager cloudSaveManager;

    public void Setup(Achievements data, int currentValue)
    {
        cloudSaveManager = CloudSaveManager.Instance;
        if (cloudSaveManager != null)
        { 
            nameText.text = data.Name;
            descriptionText.text = data.description;
            progressText.text = $"{Mathf.Min(currentValue, data.requiredValue)} / {data.requiredValue}";
            coinReward.text = data.coinReward.ToString();

            // Icon state
            if (currentValue < data.requiredValue)
            {
                icon.sprite = Locked;
            }
            else
            {
                icon.sprite = Unlocked;
            }

           
            bool claimed = cloudSaveManager.playerData.achievementsClaimed
                .Any(a => a.id == data.ID && a.claimed);

            if (claimed)
            {

                claimBtn.interactable = false;
                claimBtnText.text = "CLAIMED";

            }
            else if (currentValue >= data.requiredValue)
            {
                
                claimBtn.gameObject.SetActive(true);
                claimBtn.onClick.RemoveAllListeners(); 
                claimBtn.onClick.AddListener(() =>
                {
                    cloudSaveManager.ClaimAchievement(data.ID);
                    claimBtn.interactable = false;
                    claimBtnText.text = "CLAIMED";
                });
            }
            else
            {
               
                claimBtn.gameObject.SetActive(false);
            }
        }
    }

}