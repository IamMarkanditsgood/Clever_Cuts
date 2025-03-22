using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }
    public void Achieve(AchievementTypes newAchieve)
    {
        List<AchievementTypes> achievements = SaveManager.PlayerPrefs.LoadAchievementList(GameKeys.Achievements);
        for (int i = 0; i < achievements.Count; i++)
        {
            if (achievements[i] == newAchieve)
            {
                return;
            }
        }
        achievements.Add(newAchieve);
        SaveManager.PlayerPrefs.SaveAchievementList(GameKeys.Achievements, achievements);
    }

    public List<AchievementTypes> GetReceivedAchievements()
    {
        List<AchievementTypes> achievements = SaveManager.PlayerPrefs.LoadAchievementList(GameKeys.Achievements);
        return achievements;
    }
}
