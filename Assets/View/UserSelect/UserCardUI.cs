using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserCardUI : MonoBehaviour
{
    [Header("UI")]
    public Image avatarImage;
    public TMP_Text nameText;

    [Header("Medals")]
    public Image frogMedal;
    public Image balloonsMedal;
    public Image specialMedal;

    [Header("Sprites")]
    public Sprite emptyMedalSprite;
    public Sprite frogMedalSprite;
    public Sprite balloonsMedalSprite;
    public Sprite specialMedalSprite;

    private ChildProfile profile;

    public void Setup(
        ChildProfile profile,
        StudentAvatarData avatar,
        UserSelectUI controller
    )
    {
        this.profile = profile;

        nameText.text = profile.childName;
        avatarImage.sprite = avatar.avatarSprite;

        UpdateMedals(profile);

        GetComponent<Button>().onClick.AddListener(() =>
        {
            controller.SelectUser(profile.childName);
        });
    }

    void UpdateMedals(ChildProfile profile)
    {
        // 🐸 Rana → actividad 0
        SetMedal(
            frogMedal,
            profile.activities[0],
            frogMedalSprite
        );

        // 🎈 Globos → actividad 1
        SetMedal(
            balloonsMedal,
            profile.activities[1],
            balloonsMedalSprite
        );

        // ⭐ Especial → última actividad
        SetMedal(
            specialMedal,
            profile.activities[profile.activities.Count - 1],
            specialMedalSprite
        );
    }

    void SetMedal(Image medalImage, ActivityEntry activity, Sprite unlockedSprite)
    {
        if (ProfilesManager.Instance.IsActivityCompleted(activity))
        {
            medalImage.sprite = unlockedSprite;
            medalImage.color = Color.white;
        }
        else
        {
            medalImage.sprite = emptyMedalSprite;
           
        }
    }
}
