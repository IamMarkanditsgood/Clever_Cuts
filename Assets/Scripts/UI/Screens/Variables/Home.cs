using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Home : BasicScreen
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private Button _levelsButton;
    [SerializeField] private Button _profileButton;
    [SerializeField] private Button _leadersButton;
    [SerializeField] private Button _infoButton;

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
        _levelsButton.onClick.AddListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Levels));
        _profileButton.onClick.AddListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Profile));
        _leadersButton.onClick.AddListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Leaders));
        _infoButton.onClick.AddListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Info));
    }

    private void UnSubscribe()
    {
        _levelsButton.onClick.RemoveListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Levels));
        _profileButton.onClick.RemoveListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Profile));
        _leadersButton.onClick.RemoveListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Leaders));
        _infoButton.onClick.RemoveListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Info));
    }

    public override void SetScreen()
    {
        SetText();
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
}
