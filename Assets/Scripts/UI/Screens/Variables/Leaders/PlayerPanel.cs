using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _posInListText;



    [SerializeField] private Image _backGround;
    [SerializeField] private Sprite _playerFirstPanel;
    [SerializeField] private Sprite _playerSecondPanel;
    [SerializeField] private Sprite _playerThirdPanel;


    public void Init(int Score, string name, int posInList, int playerPos)
    {
        _scoreText.text = Score.ToString();
        _nameText.text = name;
        _posInListText.text = posInList.ToString();
        if(posInList <=3)
        {
            SetTextColor(_scoreText, "#101432");
            SetTextColor(_nameText, "#101432");
            SetTextColor(_posInListText, "#101432");
        }
        switch (posInList)
        {
            case 1:
                _backGround.sprite = _playerFirstPanel;
                break;
            case 2:
             
                _backGround.sprite = _playerSecondPanel;
                break;
            case 3:
                
                _backGround.sprite = _playerThirdPanel;
                break;
        }
    }
    public void SetTextColor(TMP_Text text, string hexColor)
    {
        if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
        {
            text.color = color;
        }
        else
        {
            Debug.LogWarning($"Invalid HEX color code: {hexColor}");
        }
    }
}
