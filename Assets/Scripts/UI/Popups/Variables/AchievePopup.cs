using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievePopup : BasicPopup
{
    [SerializeField] private AchievementTypes _achiveType;

    [SerializeField] private int _revard;
    [SerializeField] private Button _revardButton;
    [SerializeField] private Button _backButton;


    private void Start()
    {
        _revardButton.onClick.AddListener(GetRevardAchieve);
        _backButton.onClick.AddListener(Hide);
    }
    private void OnDestroy()
    {
        _revardButton.onClick.RemoveListener(GetRevardAchieve);
        _backButton.onClick.RemoveListener(Hide);
    }

    public override void ResetPopup()
    {
        _revardButton.gameObject.SetActive(false);
    }

    public override void SetPopup()
    {
        SetRevardButton();
    }

    private void SetRevardButton()
    {
        List<AchievementTypes> _receivedAchievements = new List<AchievementTypes>();
        _receivedAchievements = AchievementsManager.instance.GetReceivedAchievements();
        List<AchievementTypes> _collectedAchievements = new List<AchievementTypes>();
        _collectedAchievements = SaveManager.PlayerPrefs.LoadAchievementList(GameKeys.CollectedAchievements);

        for(int i = 0; i < _receivedAchievements.Count; i++)
        {
            if(_achiveType == _receivedAchievements[i])
            {
                for(int j = 0; j < _collectedAchievements.Count; j++)
                {
                    if(_achiveType == _collectedAchievements[j])
                    {
                        return;
                    }
                }
                _revardButton.gameObject.SetActive(true);
            }
        }
    }
    public void GetRevardAchieve()
    {
        List<AchievementTypes> achievements = SaveManager.PlayerPrefs.LoadAchievementList(GameKeys.CollectedAchievements);
        for (int i = 0; i < achievements.Count; i++)
        {
            if (achievements[i] == _achiveType)
            {
                return;
            }
        }
        GetRevard();
        achievements.Add(_achiveType);
        SaveManager.PlayerPrefs.SaveAchievementList(GameKeys.CollectedAchievements, achievements);
    }
    private void GetRevard()
    {
        _revardButton.gameObject.SetActive(false);
        ResourcesManager.Instance.ModifyResource(ResourceTypes.Coins, _revard);
    }
}
