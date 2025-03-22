using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : BasicScreen
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private Button _backButton;
    [SerializeField] private Button _startButton;

    [SerializeField] private Button _revealImageButton;
    [SerializeField] private Button _pauseTimerButton;
    [SerializeField] private Button _autoSolveButton;

    [SerializeField] private TMP_Text _revealImageText;
    [SerializeField] private TMP_Text _pauseTimerText;
    [SerializeField] private TMP_Text _autoSolveText;

    [SerializeField] private SkillBuyPopup _skillPopup;

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
        _backButton.onClick.AddListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Levels));
        _startButton.onClick.AddListener(StartGame);
        _revealImageButton.onClick.AddListener(RevealImage);
        _pauseTimerButton.onClick.AddListener(PauseTimer);
        _autoSolveButton.onClick.AddListener(AutoSolve);
    }

    private void UnSubscribe()
    {
        _backButton.onClick.RemoveListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Levels));
        _startButton.onClick.RemoveListener(StartGame);
        _revealImageButton.onClick.RemoveListener(RevealImage);
        _pauseTimerButton.onClick.RemoveListener(PauseTimer);
        _autoSolveButton.onClick.RemoveListener(AutoSolve);
    }

    public override void SetScreen()
    {
        SetText();
        SetSkills();
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
    private void SetSkills()
    {
        _revealImageText.text = SaveManager.PlayerPrefs.LoadInt(GameKeys.RevealImage).ToString();
        _pauseTimerText.text = SaveManager.PlayerPrefs.LoadInt(GameKeys.PauseTimer).ToString();
        _autoSolveText.text = SaveManager.PlayerPrefs.LoadInt(GameKeys.AutoSolve).ToString();
    }

    private void StartGame()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game);
    }
    private void RevealImage()
    {
        _skillPopup.Init(SkillTypes.RevealImage);
        _skillPopup.Show();
    }

    private void PauseTimer()
    {
        _skillPopup.Init(SkillTypes.PauseTimer);
        _skillPopup.Show();
    }

    private void AutoSolve()
    {
        _skillPopup.Init(SkillTypes.AutoSolve);
        _skillPopup.Show();
    }
}
