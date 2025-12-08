using UnityEngine;

[CreateAssetMenu(fileName = "IntroStepsCollection", menuName = "Game/Intro Steps Collection")]
public class IntroStepsCollection : ScriptableObject
{
    public IntroStep[] steps;
}
