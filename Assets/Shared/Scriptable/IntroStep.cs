using UnityEngine;

[CreateAssetMenu(fileName = "IntroStep", menuName = "Game/Intro Step")]
public class IntroStep : ScriptableObject
{
    [TextArea(2, 4)]
    public string text;        // Texto del paso
    public string animationName; // Nombre EXACTO del state en el Animator
}
