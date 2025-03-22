using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillBuyPopup : BasicPopup
{
    [SerializeField] private int _price;

    [SerializeField] private Image _skillName;

    [SerializeField] private SkillTypes[] _skillTypes;
    [SerializeField] private Sprite[] _skillNames;

    [SerializeField] private Button _backButton;
    [SerializeField] private Button _buyButton;

    private SkillTypes _skillType;

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
        _backButton.onClick.AddListener(ClosePopup);
        _buyButton.onClick.AddListener(Buy);
    }
    private void UnSubscribe()
    {
        _backButton.onClick.RemoveListener(ClosePopup);
        _buyButton.onClick.RemoveListener(Buy);
    }
    public void Init(SkillTypes skill)
    {
        _skillType = skill;
    }

    public override void ResetPopup()
    {
        
    }

    public override void SetPopup()
    {
        for(int i = 0; i < _skillTypes.Length; i++)
        {
            if(_skillType == _skillTypes[i])
            {
                _skillName.sprite = _skillNames[i];
            }
        }
    }
    private void ClosePopup()
    {
        Hide();
        UIManager.Instance.ShowScreen(ScreenTypes.Shop);
    }
    private void Buy()
    {
        if(ResourcesManager.Instance.IsEnoughResource(ResourceTypes.Coins, _price))
        {
            ResourcesManager.Instance.ModifyResource(ResourceTypes.Coins, -_price);
            string key = "";
            switch (_skillType)
            {
                case SkillTypes.RevealImage:
                    key = GameKeys.RevealImage;
                    break;
                case SkillTypes.PauseTimer:
                    key = GameKeys.PauseTimer;
                    break;
                case SkillTypes.AutoSolve:
                    key = GameKeys.AutoSolve;
                    break;
            }
            GiveSkill(key);
        }
    }
    private void GiveSkill(string skillKey)
    {
        int skillAmount = SaveManager.PlayerPrefs.LoadInt(skillKey);
        skillAmount++;
        SaveManager.PlayerPrefs.SaveInt(skillKey, skillAmount);
    }
}
