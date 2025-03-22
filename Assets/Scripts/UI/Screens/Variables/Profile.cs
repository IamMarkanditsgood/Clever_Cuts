
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile : BasicScreen
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private Button _avatarButton;

    [Header("Player")]
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private AvatarManager _avatarManager;
    [SerializeField] private Button _backButton;

    [Header("Analitic")]
    [SerializeField] private TMP_Text _levelFinished;
    [SerializeField] private TMP_Text _hintsUtilized;
    [SerializeField] private TMP_Text _totalCoins;
    [SerializeField] private TMP_Text _totalScore;

    [Header("Achievements")]
    [SerializeField] private List<AchievementTypes> _achievementsList;
    [SerializeField] private Button[] _achievementButtons;
    [SerializeField] private Image[] _achievementsImage;
    [SerializeField] private Sprite[] _achievementSprites;

    [SerializeField] private AchievePopup[] _achievePopups;

    private List<AchievementTypes> _receivedAchievements = new List<AchievementTypes>();

    private string _name;
    private void Start()
    {
        if (SaveManager.PlayerPrefs.IsSaved(GameKeys.Name))
        {
            Debug.Log(SaveManager.PlayerPrefs.LoadString(GameKeys.Name));
            _name = SaveManager.PlayerPrefs.LoadString(GameKeys.Name);
            _nameInputField.text = SaveManager.PlayerPrefs.LoadString(GameKeys.Name);
        }
        else
        {
            _name = "UserName";
            _nameInputField.text = "UserName";
            SaveManager.PlayerPrefs.SaveString(GameKeys.Name,_name);

        }
        Subscribe();
    }
    private void OnDestroy()
    {
        UnSubscribe();
    }
    private void Update()
    {
        if(_name != _nameInputField.text)
        {
            SaveManager.PlayerPrefs.SaveString(GameKeys.Name, _nameInputField.text);
            _name = _nameInputField.text;
        }
    }
    private void Subscribe()
    {
        _backButton.onClick.AddListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Home));
        _avatarButton.onClick.AddListener(() => _avatarManager.PickFromGallery());

        for (int i = 0; i < _achievementButtons.Length; i++)
        {
            int index = i;
            _achievementButtons[index].onClick.AddListener(() => AchievePressed(index));
        }
    }

    private void UnSubscribe()
    {
        _backButton.onClick.RemoveListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Home));
        _avatarButton.onClick.RemoveListener(() => _avatarManager.PickFromGallery());

        for (int i = 0; i < _achievementButtons.Length; i++)
        {
            int index = i;
            _achievementButtons[index].onClick.RemoveListener(() => AchievePressed(index));
        }
    }

    public override void SetScreen()
    {
        SetText();
        SetPlayer();
        SetAnalitics();
        SetAchievements();
    }

    public override void ResetScreen()
    {
        
    }

    private void SetText()
    {
        TextManager textManager = new TextManager();

        int score = ResourcesManager.Instance.GetResource(ResourceTypes.Points);
        int coins = ResourcesManager.Instance.GetResource(ResourceTypes.Coins);

        textManager.SetText(score, _scoreText, true);
        textManager.SetText(coins, _coinsText);
    }
    private void SetPlayer()
    {
        _avatarManager.SetSavedPicture();
        if (SaveManager.PlayerPrefs.IsSaved(GameKeys.Name))
        {
            Debug.Log(SaveManager.PlayerPrefs.LoadString(GameKeys.Name));
            _nameInputField.text = SaveManager.PlayerPrefs.LoadString(GameKeys.Name);
        }
        else
        {
            _name = "UserName";
            _nameInputField.text = "UserName";
            SaveManager.PlayerPrefs.SaveString(GameKeys.Name, _name);

        }
    }
    private void SetAnalitics()
    {
        _levelFinished.text = SaveManager.PlayerPrefs.LoadInt(GameKeys.LevelFinished).ToString();
        _hintsUtilized.text = SaveManager.PlayerPrefs.LoadInt(GameKeys.HintsUtilized).ToString();
        _totalCoins.text = SaveManager.PlayerPrefs.LoadInt(GameKeys.TotalCoins).ToString();

        TextManager textManager = new TextManager();
        textManager.SetText(SaveManager.PlayerPrefs.LoadInt(GameKeys.TotalCoins), _totalScore, true);
    }
    private void SetAchievements()
    {
        _receivedAchievements = AchievementsManager.instance.GetReceivedAchievements();

        for (int i = 0; i < _achievementsList.Count; i++)
        {
            for (int j = 0; j < _receivedAchievements.Count; j++)
            {
                if (_achievementsList[i] == _receivedAchievements[j])
                {
                    _achievementsImage[i].sprite = _achievementSprites[i];
                }
            }
        }
    }

    private void AchievePressed(int index)
    {
        _achievePopups[index].Show();
    }
}
