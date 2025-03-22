using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaders : BasicScreen
{
    [SerializeField] private LeaderBoardAcembler leaderBoardAcembler;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private Button _backButton;

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
    }

    private void UnSubscribe()
    {
        _backButton.onClick.RemoveListener(() => UIManager.Instance.ShowScreen(ScreenTypes.Home));
    }

    public override void ResetScreen()
    {
        leaderBoardAcembler.CleanLeaderBoard();
    }

    public override void SetScreen()
    {
        SetText();
        leaderBoardAcembler.SetLeaderBoard();
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
