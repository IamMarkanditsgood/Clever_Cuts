using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{

    public static ResourcesManager Instance { get; private set; }

    private Dictionary<ResourceTypes, int> _resources = new Dictionary<ResourceTypes, int>();

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        InitResourceDictionary();
        
    }

    public int GetResource(ResourceTypes resource)
    {
        return _resources[resource];
    }

    public void ModifyResource(ResourceTypes resource, int updateAmount)
    {
        _resources[resource] += updateAmount;

        if(resource == ResourceTypes.Points)
        {

            int totalScore = SaveManager.PlayerPrefs.LoadInt(GameKeys.TotalScore);
            totalScore += updateAmount;
            SaveManager.PlayerPrefs.SaveInt(GameKeys.TotalScore, totalScore);
        }
        if (resource == ResourceTypes.Coins)
        {
            int totalCoins = SaveManager.PlayerPrefs.LoadInt(GameKeys.TotalCoins);
            totalCoins += updateAmount;
            SaveManager.PlayerPrefs.SaveInt(GameKeys.TotalCoins, totalCoins);
        }
        SaveManager.Resources.SaveResource(resource, _resources[resource]);
        ResourceEvents.ResourceModified(resource, _resources[resource]);
    }

    public bool IsEnoughResource(ResourceTypes resource, int price)
    {
        if (_resources[resource] < price)
        {
            return false;
        }

        return true;
    }

    private void InitResourceDictionary()
    {

        _resources[ResourceTypes.Coins] = SaveManager.Resources.LoadResource(ResourceTypes.Coins);
        _resources[ResourceTypes.Points] = SaveManager.Resources.LoadResource(ResourceTypes.Points);
    }
}