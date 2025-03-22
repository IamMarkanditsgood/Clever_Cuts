using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class LeaderBoardAcembler
{
    [Header("AnotherPlayers")]
    [SerializeField] private List<PlayerPanel> _leaders;
    [Header("PlayersInDB")]
    [SerializeField] private PlayerList _playerList;

    [SerializeField] private ApiManager _apiManager;
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _playerPosPref;

    [Header("MainPlayer")]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_Text _pos;

    private int playerPos;

   
    public async void SetLeaderBoard()
    {
        _playerList = await _apiManager.GetPlayerList();
        SortPlayersByScore(_playerList);
        GetPlayerPos();
        SetPlayers();

    }

    private void GetPlayerPos()
    {
        for (int i = 0; i < _playerList.players.Count; i++)
        {
            if (_playerList.players[i].id == SaveManager.PlayerPrefs.LoadInt(GameKeys.PlayerID))
            {
                playerPos = i;
            }
        }
    }

    private void SortPlayersByScore(PlayerList playerList)
    {
        if (playerList != null && playerList.players != null)
        {
            playerList.players.Sort((x, y) => y.score.CompareTo(x.score));
        }
    }
    private void SetPlayers()
    {
        for (int i = 0; i < _playerList.players.Count; i++)
        {
            PlayerPanel player;
            int score = _playerList.players[i].score;
            int pos = i + 1;
            string name = _playerList.players[i].name;
            GameObject obj = UnityEngine.Object.Instantiate(_playerPosPref, _content);
            player = obj.GetComponent<PlayerPanel>();
            _leaders.Add(player);
            player.Init(score, name, pos, playerPos + 1);

            if(pos == playerPos+1)
            {
                SetMainPlayer(i);
            }
        }
    }
    public void CleanLeaderBoard()
    {
        for (int i = 0; i < _leaders.Count; i++)
        {
            GameObject obj = _leaders[i].gameObject;
            UnityEngine.Object.Destroy(obj);
        }
        _leaders.Clear();
    }
    private void SetMainPlayer(int i)
    {
        _score.text = _playerList.players[i].score.ToString();
        _pos.text = (i + 1).ToString();
        _name.text = _playerList.players[i].name;


    }
}
