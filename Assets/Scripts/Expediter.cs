using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expediter : MonoBehaviour, ISurface
{
    [SerializeField] private ActiveOrders active;
    [SerializeField] private PlateSurface plateSurface;
    [SerializeField] private float returnAfterSeconds;

    private readonly List<Plate> _plates = new();
    
    public bool HasItem => false;
    public bool Place(Item item)
    {
        if (item is Food food)
        {
            active.Remove(food);
            
            if (food.IsPlated)
                StorePlate(food.RemovePlate());
            
            Destroy(food.gameObject);
            return true;
        }
        
        if (item is Plate plate)
            StorePlate(plate);

        return true;
    }

    private void StorePlate(Plate plate)
    {
        _plates.Add(plate);
        _plates[^1].Hide();

        StartCoroutine(ReturnPlate());
    }

    IEnumerator ReturnPlate()
    {
        yield return new WaitForSeconds(returnAfterSeconds);

        plateSurface.Place(_plates[0]);
        _plates.RemoveAt(0);
    }

    public Item Pick() => null;
    public bool Combine(Item item) => false;
}
