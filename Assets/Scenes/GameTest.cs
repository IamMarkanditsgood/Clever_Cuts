using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GameTest : MonoBehaviour
{
    [SerializeField] private GameCards[] _cardLevels;
    [SerializeField] private CardData[] _cardData;
    [SerializeField] private PoolPosData[] _poolPosData;

    [Serializable]
    public class CardData
    {
        public int row;
        public int col;
        public Image cardImages;
        public Button cardButtons;
        public Vector2 startPos;
        public Vector2 currentPos;
    }
    [Serializable]
    public class PoolPosData
    {
        public int row;
        public int col;
        public RectTransform cardPos;
        public bool isEmptyPos = true;
    }

    [Serializable]
    public class GameCards
    {
        public Sprite[] _cardSprites;
    }

    public int _currentLevel;

    private void Start()
    {
        SetGameScreen();
        for (int i = 0; i < _cardData.Length; i++)
        {
            int index = i;
            _cardData[index].cardButtons.onClick.AddListener(() => OnCardClick(index));
        }
    }
    private void OnDestroy()
    {
        for (int i = 0; i < _cardData.Length; i++)
        {
            int index = i;
            _cardData[index].cardButtons.onClick.RemoveListener(() => OnCardClick(index));
        }
    }

    private void SetGameScreen()
    {
        
        for (int i = 0; i < _cardData.Length; i++)
        {
            _cardData[i].cardImages.sprite = _cardLevels[_currentLevel]._cardSprites[i];
            _cardData[i].startPos = _poolPosData[i].cardPos.anchoredPosition;
            _cardData[i].currentPos = _poolPosData[i].cardPos.anchoredPosition;
            _poolPosData[i].isEmptyPos = false;
        }
       
        RandomizeCards();
    }

    private void RandomizeCards()
    {
        List<int> randomPos = GenerateUniqueRandomNumbers(_cardData.Length, 0, _cardData.Length-1);
        for(int i = 0; i < _cardData.Length; i++)
        {
            _cardData[i].cardImages.rectTransform.anchoredPosition = _poolPosData[randomPos[i]].cardPos.anchoredPosition;
            _cardData[i].currentPos = _poolPosData[randomPos[i]].cardPos.anchoredPosition;
            _cardData[i].row = _poolPosData[randomPos[i]].row;
            _cardData[i].col = _poolPosData[randomPos[i]].col;
        }
    }
    public List<int> GenerateUniqueRandomNumbers(int count, int min, int max)
    {
        if (count > (max - min + 1))
        {
            Debug.LogError("Неможливо згенерувати " + count + " унікальних чисел в діапазоні від " + min + " до " + max);
            return null;
        }

        List<int> numbers = new List<int>();
        for (int i = min; i <= max; i++)
        {
            numbers.Add(i);
        }

        List<int> randomNumbers = new List<int>();
        System.Random rng = new System.Random();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = rng.Next(numbers.Count);
            randomNumbers.Add(numbers[randomIndex]);
            numbers.RemoveAt(randomIndex);
        }

        return randomNumbers;
    }
    private void OnCardClick(int cardIndex)
    {
        bool cardMoved = true; // Прапор, що вказує, чи була переміщена картка

        cardMoved = CheckUp(cardIndex);
        if (cardMoved)
        {
            cardMoved = CheckDown(cardIndex);
        }
        
        if (cardMoved)
        {
            cardMoved = CheckLeft(cardIndex);
        }
    
        if (cardMoved)
        {
            cardMoved = CheckRight(cardIndex);
        }

        CheckWin();
    }
    private bool CheckUp(int cardIndex)
    {
        if (_cardData[cardIndex].row > 0)
        {
            for(int i =0; i < _poolPosData.Length; i++)
            {
                if (_poolPosData[i].row == _cardData[cardIndex].row - 1 && _poolPosData[i].col == _cardData[cardIndex].col && _poolPosData[i].isEmptyPos)
                {
                    Debug.Log("uP");
                    MoveCard(cardIndex, i);
                    return false;
                }
            }
        }
        return true;
    }
    private bool CheckDown(int cardIndex)
    {

        if (_cardData[cardIndex].row < 3)
        {
            for (int i = 0; i < _poolPosData.Length; i++)
            {
                if (_poolPosData[i].row == _cardData[cardIndex].row + 1 && _poolPosData[i].col == _cardData[cardIndex].col && _poolPosData[i].isEmptyPos)
                {
                    Debug.Log("dOWN");
                    MoveCard(cardIndex, i);
                    return false;
                }
            }
        }
        return true;
    }
    private bool CheckLeft(int cardIndex)
    {
        if (_cardData[cardIndex].col > 0)
        {
            for (int i = 0; i < _poolPosData.Length; i++)
            {
                if (_poolPosData[i].col == _cardData[cardIndex].col - 1 && _poolPosData[i].row == _cardData[cardIndex].row && _poolPosData[i].isEmptyPos)
                {
                    Debug.Log("lEFT");
                    MoveCard(cardIndex, i);
                    return false;
                }
            }
        }
        return true;
    }
    private bool CheckRight(int cardIndex)
    {
        if (_cardData[cardIndex].col < 3)
        {
            for (int i = 0; i < _poolPosData.Length; i++)
            {
                if (_poolPosData[i].col == _cardData[cardIndex].col + 1 && _poolPosData[i].row == _cardData[cardIndex].row && _poolPosData[i].isEmptyPos)
                {
                    Debug.Log("rIGHT");
                    MoveCard(cardIndex, i);
                    return false;
                }
            }
        }
        return true;
    }

    private void MoveCard(int cardIndex, int i)
    {
        for (int j = 0; j < _poolPosData.Length; j++)
        {
            if (_cardData[cardIndex].currentPos == _poolPosData[j].cardPos.anchoredPosition)
            {
                _poolPosData[j].isEmptyPos = true;
            }
        }
        _cardData[cardIndex].cardImages.rectTransform.anchoredPosition = _poolPosData[i].cardPos.anchoredPosition;
        _cardData[cardIndex].currentPos = _poolPosData[i].cardPos.anchoredPosition;
        _cardData[cardIndex].row = _poolPosData[i].row;
        _cardData[cardIndex].col = _poolPosData[i].col;
        _poolPosData[i].isEmptyPos = false;
    }
    private void CheckWin()
    {
        for (int i = 0; i < _cardData.Length; i++)
        {
            if (_cardData[i].currentPos != _cardData[i].startPos)
            {
                return;
            }
        }
        Debug.Log("You Win!");
    }
}
