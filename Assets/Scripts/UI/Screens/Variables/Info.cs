using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : BasicScreen
{
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
    }

    public override void SetScreen()
    {     
    }
}
