using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : BasicPopup
{
    [SerializeField] private Button _pauseOff;
    private GameScreen _gameScreen;

    private void Start()
    {
        _pauseOff.onClick.AddListener(PauseOff);
    }
    private void OnDestroy()
    {
        _pauseOff.onClick.RemoveListener(PauseOff);
    }
    public void Init(GameScreen gameScreen)
    {
        _gameScreen = gameScreen;
    }
    public override void ResetPopup()
    {
        
    }

    public override void SetPopup()
    {
        
    }
    private void PauseOff()
    {
        Hide();
        _gameScreen.PauseOff();
    }
}
