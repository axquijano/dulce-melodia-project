using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VisualFeedback : MonoBehaviour
{
    [Header("Rewards sequence")]
    [SerializeField] private List<RewardData> rewards;

    private int currentRewardIndex = 0;
    private Image displayImage;

    void Awake()
    {
        displayImage = GetComponent<Image>();
        gameObject.SetActive(false); // Starts hidden
    }

    public void ShowNextReward()
    {
        if (rewards == null || rewards.Count == 0)
        {
            Debug.LogWarning("No rewards assigned to VisualFeedback");
            return;
        }
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PlayAnimation(rewards[currentRewardIndex]));

        if (currentRewardIndex < rewards.Count - 1)
        {
            currentRewardIndex++;
        }
    }

    public void ResetRewards()
    {
        currentRewardIndex = 0;
    }

    private IEnumerator PlayAnimation(RewardData reward)
    {
        gameObject.SetActive(true);

        foreach (Sprite frame in reward.frames)
        {
            displayImage.sprite = frame;
            yield return new WaitForSeconds(reward.animationSpeed);
        }

        gameObject.SetActive(false); // Hide after animation
    }
}
