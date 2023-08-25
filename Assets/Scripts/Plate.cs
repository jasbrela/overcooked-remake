using System.Collections.Generic;
using UnityEngine;

public class Plate : Item
{
    [SerializeField] private int maxStack = 4;
    private readonly Stack<Plate> _plates = new();

    public int CurrentStack => _plates.Count;

    private void Start()
    {
        _plates.Push(this);
    }

    /// <summary>
    /// Pick a plate from the plate stack. Pick the plate if it's the only plate in the stack.
    /// </summary>
    /// <returns>Returns the Plate from the stack's top.</returns>
    public Plate Pick()
    {
        if (CurrentStack == 1) return this;
        
        var plate = _plates.Pop();
        
        return plate;
    }

    /// <summary>
    /// Add a plate to the stack
    /// </summary>
    /// <param name="plate">The plate you want to stack</param>
    /// <returns>True if the plate was successfully stacked.</returns>
    public bool Stack(Plate plate)
    {
        if (CurrentStack >= maxStack) return false;
        
        _plates.Push(plate);
        
        Debug.Log($"Added a plate to the stack. Current length: {CurrentStack}");

        var plateTransform = plate.transform;
        var t = transform;
        
        plateTransform.parent = t;
        plateTransform.position = t.position + (Vector3.up * 0.1f);
        plateTransform.rotation = t.rotation;
        
        return true;
    }
}
