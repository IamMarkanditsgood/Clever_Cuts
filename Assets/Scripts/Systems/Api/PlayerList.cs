using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerList
{
    public List<PlayerData> players;
}
[System.Serializable]
public class PlayerData
{
    public int id;
    public string name;
    public int score;
    public string imageURL;
}
