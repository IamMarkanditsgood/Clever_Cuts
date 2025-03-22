using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : BasicPopup
{
    [SerializeField] private Button _okButton;

    private void Start()
    {
        _okButton.onClick.AddListener(Close);
    }
    private void OnDestroy()
    {
        _okButton.onClick.RemoveListener(Close);
    }
    private void Close()
    {
        
        Hide();
    }
    public override void ResetPopup()
    {

    }

    public override void SetPopup()
    {
     
    }
}
