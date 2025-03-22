using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : BasicScreen
{
    [SerializeField] private GameCards[] _cardLevels;
    [SerializeField] private CardData[] _cardData;
    [SerializeField] private PoolPosData[] _poolPosData;

    private CardData[] _basicCardData;
    private PoolPosData[] _basicPoolPosData;

    [SerializeField] private Button _revealImageButton;
    [SerializeField] private Button _pauseTimerButton;
    [SerializeField] private Button _autoSolveButton;

    [SerializeField] private TMP_Text _revealImageText;
    [SerializeField] private TMP_Text _pauseTimerText;
    [SerializeField] private TMP_Text _autoSolveText;

    [SerializeField] private Image _timerFillImage;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private float _time;

    [SerializeField] private Button _backButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private PausePopup _pausePopup;
    [SerializeField] private BasicPopup _winPopup;
    [SerializeField] private BasicPopup _losePopup;

    [SerializeField] private Image _mainArt;
    private float timeRemaining = 60f; // Час, що залишився
    private bool isPaused = false;
    private bool canPause = true;
    private bool canUseSkill = true;

    private Coroutine _coroutine;


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
        public Sprite[] cardSprites;
        public Sprite mainArt;
    }

    public int _currentLevel;

    private void Start()
    {
        _basicCardData = _cardData;
        _basicPoolPosData = _poolPosData;
        Subscribe();
    }
    private void OnDestroy()
    {
        UnSubscribe();
        StopAllCoroutines();
    }
    private void Subscribe()
    {
        GameEvents.OnCurrentLevelChanged += UpdateCurrentLevel;

        _revealImageButton.onClick.AddListener(RevealImage);
        _pauseTimerButton.onClick.AddListener(PauseTimer);
        _autoSolveButton.onClick.AddListener(AutoSolve);

        _backButton.onClick.AddListener(Back);
        _pauseButton.onClick.AddListener(Pause);

        for (int i = 0; i < _cardData.Length; i++)
        {
            int index = i;
            _cardData[index].cardButtons.onClick.AddListener(() => OnCardClick(index));
        }
    }

    private void UnSubscribe()
    {
        GameEvents.OnCurrentLevelChanged -= UpdateCurrentLevel;

        _revealImageButton.onClick.RemoveListener(RevealImage);
        _pauseTimerButton.onClick.RemoveListener(PauseTimer);
        _autoSolveButton.onClick.RemoveListener(AutoSolve);

        _backButton.onClick.RemoveListener(Back);
        _pauseButton.onClick.RemoveListener(Pause);

        for (int i = 0; i < _cardData.Length; i++)
        {
            int index = i;
            _cardData[index].cardButtons.onClick.RemoveListener(() => OnCardClick(index));
        }
    }
    public override void ResetScreen()
    {
        _cardData = _basicCardData;
        _poolPosData = _basicPoolPosData;
        _poolPosData[_poolPosData.Length-1].isEmptyPos = true;
        timeRemaining = _time;
        
        isPaused = false;
        canUseSkill = true;
        canPause = true;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        for (int i = 0; i < _cardData.Length; i++)
        {
            _cardData[i].cardButtons.interactable = true;
        }
    }

    public override void SetScreen()
    {
        SetSkills();
        
        for (int i = 0; i < _cardData.Length; i++)
        {
            _cardData[i].cardImages.sprite = _cardLevels[_currentLevel].cardSprites[i];
            _cardData[i].startPos = _poolPosData[i].cardPos.anchoredPosition;
            _cardData[i].currentPos = _poolPosData[i].cardPos.anchoredPosition;
            _poolPosData[i].isEmptyPos = false;
        }
        _mainArt.sprite = _cardLevels[_currentLevel].mainArt;
        RandomizeCards();

        _coroutine = StartCoroutine(Timer());
    }
    public void SetSkills()
    {
        _revealImageText.text = SaveManager.PlayerPrefs.LoadInt(GameKeys.RevealImage).ToString();
        _pauseTimerText.text = SaveManager.PlayerPrefs.LoadInt(GameKeys.PauseTimer).ToString();
        _autoSolveText.text = SaveManager.PlayerPrefs.LoadInt(GameKeys.AutoSolve).ToString();
    }
    private void UpdateCurrentLevel(int newLevel)
    {
        _currentLevel = newLevel;
        Debug.Log(_currentLevel);
    }
    private void RandomizeCards()
    {
        List<int> randomPos = GenerateUniqueRandomNumbers(_cardData.Length, 0, _cardData.Length - 1);
        
        for (int i = 0; i < _cardData.Length; i++)
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
            for (int i = 0; i < _poolPosData.Length; i++)
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
        Win();
    }

    private IEnumerator Timer()
    {
        while (timeRemaining > 0)
        {
            // Чекаємо, поки таймер не в паузі
            while (isPaused)
            {
                yield return null; // Пропускаємо кадри, поки таймер на паузі
            }

            // Оновлюємо текст таймера
            _timerText.text = Mathf.CeilToInt(timeRemaining).ToString();

            // Оновлюємо заповнення Image
            _timerFillImage.fillAmount = timeRemaining / _time;

            yield return new WaitForSeconds(1f); // Затримка на 1 секунду
            timeRemaining -= 1f; // Зменшуємо залишок часу
        }

        _timerText.text = "0"; // Встановлюємо 0 після завершення
        _timerFillImage.fillAmount = 0; // Повністю очищаємо заповнення
        Lose();
    }

    private void Lose()
    {
        _losePopup.Show();
        UIManager.Instance.ShowScreen(ScreenTypes.Levels);
        AddTotalLevels();
    }
    private void Win()
    {
        ResourcesManager.Instance.ModifyResource(ResourceTypes.Coins, 500);
        ResourcesManager.Instance.ModifyResource(ResourceTypes.Points, 500);
        _winPopup.Show();
        UIManager.Instance.ShowScreen(ScreenTypes.Levels);
        AddTotalLevels();

        if (_currentLevel == SaveManager.PlayerPrefs.LoadInt(GameKeys.PassedLevels))
        {
            int passedLevel = SaveManager.PlayerPrefs.LoadInt(GameKeys.PassedLevels);
            passedLevel++;
            SaveManager.PlayerPrefs.SaveInt(GameKeys.PassedLevels, passedLevel);
        }
    }
    private void AddTotalLevels()
    {
        int levels = SaveManager.PlayerPrefs.LoadInt(GameKeys.LevelFinished);
        levels++;
        SaveManager.PlayerPrefs.SaveInt(GameKeys.LevelFinished, levels);
    }
    public void Pause()
    {
        if (canPause)
        {
            isPaused = true;
            _pausePopup.Init(this);
            _pausePopup.Show();
        }
    }
    public void PauseOff()
    {
        isPaused = false;
    }

    private void Back()
    {
        Hide();
        UIManager.Instance.ShowScreen(ScreenTypes.Shop);     
    }
    private async void RevealImage()
    {
        int revealImage = SaveManager.PlayerPrefs.LoadInt(GameKeys.RevealImage);
        if (revealImage > 0 && canUseSkill)
        {

            AddTotalHints();
            _mainArt.gameObject.SetActive(true);
            revealImage--;
            SaveManager.PlayerPrefs.SaveInt(GameKeys.RevealImage, revealImage);
            await UniversalTimer(3);
            _mainArt.gameObject.SetActive(false);


        }
    }

    private async void PauseTimer()
    {
        int pauseTimer = SaveManager.PlayerPrefs.LoadInt(GameKeys.PauseTimer);
        if (pauseTimer > 0 && canUseSkill)
        {
            
            AddTotalHints();
            isPaused = true; 
            pauseTimer--;
            SaveManager.PlayerPrefs.SaveInt(GameKeys.PauseTimer, pauseTimer);
            await UniversalTimer(5);
            isPaused = false;

        }
    }

    private async void AutoSolve()
    {
        int autoSolve = SaveManager.PlayerPrefs.LoadInt(GameKeys.AutoSolve);
        if (autoSolve > 0 && canUseSkill)
        {

            AddTotalHints();
            StopCoroutine(_coroutine);
            for (int i = 0; i < _cardData.Length; i++)
            {
                _cardData[i].cardImages.rectTransform.anchoredPosition = _cardData[i].startPos;
                _cardData[i].cardButtons.interactable = false;
            }
            autoSolve--;
            SaveManager.PlayerPrefs.SaveInt(GameKeys.AutoSolve, autoSolve);
            await UniversalTimer(3);
            
            Win();
            
        }
    }
    private void AddTotalHints()
    {
        int hints = SaveManager.PlayerPrefs.LoadInt(GameKeys.HintsUtilized);
        hints++;
        SaveManager.PlayerPrefs.SaveInt(GameKeys.HintsUtilized, hints);
    }
    private async Task UniversalTimer(int timer)
    {
        canPause = false;
        canUseSkill = false;
        SetSkills();
        await Task.Delay(timer * 1000);
        canPause = true;
        canUseSkill = true;
    }
}

