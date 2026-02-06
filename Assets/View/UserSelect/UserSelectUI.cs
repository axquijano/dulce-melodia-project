using UnityEngine;
using System.Collections.Generic;

public class UserSelectUI : MonoBehaviour
{
    [Header("UI")]
    public Transform cardContainer;
    public GameObject userCardPrefab;

    [Header("Navigation")]
    public int cardsPerPage = 3;

    [Header("Avatars")]
    public StudentAvatarDatabase avatarDatabase;

    private List<ChildProfile> profiles;
    private int currentPage = 0;

    void Start()
    {
        profiles = ProfilesManager.Instance.GetAllProfiles();
        RefreshPage();
    }

    void RefreshPage()
    {
        foreach (Transform t in cardContainer)
            Destroy(t.gameObject);

        int start = currentPage * cardsPerPage;
        int end = Mathf.Min(start + cardsPerPage, profiles.Count);

        for (int i = start; i < end; i++)
        {
            var profile = profiles[i];
            var avatar = avatarDatabase.GetById(profile.avatarId);

            var card = Instantiate(userCardPrefab, cardContainer);
            card.GetComponent<UserCardUI>()
                .Setup(profile, avatar, this);
        }
    }

    public void NextPage()
    {
        if ((currentPage + 1) * cardsPerPage < profiles.Count)
        {
            currentPage++;
            RefreshPage();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            RefreshPage();
        }
    }

    public void SelectUser(string name)
    {
        ProfilesManager.Instance.SetCurrentProfile(name);
        SceneLoader.Instance.LoadScene("MapActivity");
    }

    public void Back()
    {
        SceneLoader.Instance.LoadScene("MainMenu");
    }
}
