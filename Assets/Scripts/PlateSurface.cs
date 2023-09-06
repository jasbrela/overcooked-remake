using System;
using UnityEngine;

public class PlateSurface : MonoBehaviour, ISurface
{
    [SerializeField] protected Transform snapPoint;
    
    private Plate _plate;

    public bool HasItem => _plate != null;
    public bool Place(Item item) => false; // from player

    public bool Place(Plate plate)
    {
        if (_plate == null)
        {
            _plate = plate;
            
            _plate.Snap(snapPoint);
        }
        else
        {
            _plate.Stack(plate, true);
        }
        
        plate.Show();

        return true;
    }
    
    public Item Pick()
    {
        if (_plate.CurrentStack > 1) return _plate.Pick();
        
        var plate = _plate;   
        _plate = null;
        return plate;
    }

    public bool Combine(Item item) => false;
}
