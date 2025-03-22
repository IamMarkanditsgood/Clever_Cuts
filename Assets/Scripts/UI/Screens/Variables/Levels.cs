using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Levels : BasicScreen
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private Button _backButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _prevLevelButton;

    [SerializeField] private TMP_Text _levelName;
    [SerializeField] private string[] _levelNames;

    [SerializeField] private Image _levelBack;

    [SerializeField] private Sprite _openLevel;
    [SerializeField] private Sprite _closeLevel;

    [SerializeField] private Image _bg;
    [SerializeField] private Sprite[] _bgSprites;

    private int _currentLevel;

    private void Start()
    {
      
        Subscribe();
    }
    private void OnDestroy()
    {
        UnSubscribe();
    }
    private void Subscribe()
    {
        _backButton.onClick.AddListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Home));
        _playButton.onClick.AddListener(Play);
        _nextLevelButton.onClick.AddListener(NextLevel);
        _prevLevelButton.onClick.AddListener(PreviousLevel);
    }

    private void UnSubscribe()
    {
        _backButton.onClick.RemoveListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Home));
        _playButton.onClick.RemoveListener(Play);
        _nextLevelButton.onClick.RemoveListener(NextLevel);
        _prevLevelButton.onClick.RemoveListener(PreviousLevel);
    }

    public override void SetScreen()
    {
       
        SetText();
        SetLevel();
    }

    public override void ResetScreen()
    {
        _currentLevel = 0;
    }
 
    private void SetText()
    {
        TextManager textManager = new TextManager();

        int score = ResourcesManager.Instance.GetResource(ResourceTypes.Points);
        int coins = ResourcesManager.Instance.GetResource(ResourceTypes.Coins);

        textManager.SetText(score, _scoreText, true);
        textManager.SetText(coins, _coinsText);
    }
    private void SetLevel()
    {
        
        _bg.sprite = _bgSprites[_currentLevel];
        _levelName.text = _levelNames[_currentLevel];

        if (_currentLevel <= SaveManager.PlayerPrefs.LoadInt(GameKeys.PassedLevels))
        {
            _levelBack.sprite = _openLevel;
        }
        else
        {
            _levelBack.sprite = _closeLevel;
        }
    }

    private void NextLevel()
    {
        _currentLevel++;
        if (_currentLevel == _levelNames.Length)
        {
            _currentLevel = 0;
        }
        SetLevel();
    }
    private void PreviousLevel()
    {
        _currentLevel--;
        if(_currentLevel < 0)
        {
            _currentLevel = _levelNames.Length - 1;
        }
        SetLevel();
    }
    private void Play()
    {
        
        if (_currentLevel <= SaveManager.PlayerPrefs.LoadInt(GameKeys.PassedLevels))
        {
            GameEvents.SentCurrentLevel(_currentLevel);
            UIManager.Instance.ShowScreen(ScreenTypes.Shop);
        }    
    }
}
