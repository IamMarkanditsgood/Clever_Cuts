using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private BasicScreen[] _screens;

    public static UIManager Instance { get; private set; }

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        ShowScreen(ScreenTypes.Home);
    }

    public void ShowScreen(ScreenTypes screenType)
    {
        CloseAllScreens();

        foreach (var screen in _screens)
        {
            if(screen.ScreenType == screenType)
            {
                screen.Show();
            }
        }
    }

    private void CloseAllScreens()
    {
        foreach (var screen in _screens)
        {
            screen.Hide();
        }
    }
}
