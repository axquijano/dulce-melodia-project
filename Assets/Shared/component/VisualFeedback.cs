using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VisualFeedback : MonoBehaviour
{
    [Header("Rewards sequence")]
    [SerializeField] private List<RewardData> rewards;

    private Image displayImage;

    private int lastRewardIndex = -1;

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

        int index;

        do
        {
            index = Random.Range(0, rewards.Count);
        }
        while (index == lastRewardIndex && rewards.Count > 1);

        lastRewardIndex = index;

        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PlayAnimation(rewards[index]));
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
