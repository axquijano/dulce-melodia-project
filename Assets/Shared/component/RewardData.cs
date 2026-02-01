using UnityEngine;

[CreateAssetMenu(fileName = "NewReward", menuName = "Game/Reward")]
public class RewardData : ScriptableObject
{
    [Tooltip("Frames used for the animation (3 to 5 sprites)")]
    public Sprite[] frames;

    [Tooltip("Time between frames")]
    public float animationSpeed = 0.1f;
}
