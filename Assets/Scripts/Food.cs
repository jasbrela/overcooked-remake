using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    [SerializeField] private bool isPlateable = true;
    
    [SerializeField] private Food handsResult;
    [SerializeField] private Food cutResult;
    [SerializeField] private Food stoveResult;
    
    [SerializeField] private List<Combination> combinations = new();

    public bool IsPlateable => isPlateable;
    public bool IsPlated => _isPlated;
    
    private bool _isPlated;
    
    private void Start()
    {
        foreach (var combination in combinations)
        {
            if (combination.result == null) combinations.Remove(combination);
        }
    }

    public Food GetCraftedItem(StationType type)
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

    public Food CheckCombination(Food food)
    {
        foreach (var combination in combinations)
        {
            if (combination.combineWith.Id == food.Id) return combination.result;
        }

        return null;
    }

    public bool Plate(Plate plate)
    {
        if (!isPlateable || _isPlated) return false;
        _isPlated = true;
        
        var plateTransform = plate.transform;
        var t = transform;

        plateTransform.parent = t;
        plateTransform.position = t.position;
        plateTransform.rotation = t.rotation;

        return true;
    }
}
