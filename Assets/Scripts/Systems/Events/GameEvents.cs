using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event Action<int> OnCurrentLevelChanged;

    public static void SentCurrentLevel(int newLevel)
    {
        OnCurrentLevelChanged?.Invoke(newLevel);
    }
}
