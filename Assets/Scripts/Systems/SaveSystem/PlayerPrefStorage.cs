using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPrefStorage
{
    public void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public int LoadInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    public void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public string LoadString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
    public void SaveAchievementList(string key, List<AchievementTypes> list)
    {
        string serializedList = string.Join(",", list.Select(a => a.ToString()).ToArray());
        PlayerPrefs.SetString(key, serializedList);
        PlayerPrefs.Save();
        Debug.Log(key);
    }

    public List<AchievementTypes> LoadAchievementList(string key)
    {
        string serializedList = PlayerPrefs.GetString(key, string.Empty);
        if (string.IsNullOrEmpty(serializedList))
        {
            return new List<AchievementTypes>();
        }

        return serializedList.Split(',')
                             .Select(s => (AchievementTypes)System.Enum.Parse(typeof(AchievementTypes), s))
                             .ToList();
    }
    public bool IsSaved(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return true;
        }
        return false;
    }

    public void ResetSaves()
    {
        PlayerPrefs.DeleteAll();
    }
}
