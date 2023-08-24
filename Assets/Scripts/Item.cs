using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private List<Combination> combinations = new();

    [SerializeField] private int id;
    [SerializeField] private Item handsResult;
    [SerializeField] private Item cutResult;
    [SerializeField] private Item stoveResult;

    private void Start()
    {
        foreach (var combination in combinations)
        {
            if (combination.result == null) combinations.Remove(combination);
        }
    }

    public Item GetCraftedItem(StationType type)
    {
        switch (type)
        {
            case StationType.Hands: return handsResult;
            case StationType.Cut: return cutResult;
            case StationType.Stove: return stoveResult;
            
            default:
                return null;
        }
    }

    public Item CheckCombination(Item item)
    {
        foreach (var combination in combinations)
        {
            if (combination.combineWith.id == item.id) return combination.result;
        }

        return null;
    }
}
