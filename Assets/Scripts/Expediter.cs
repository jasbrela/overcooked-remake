using UnityEngine;

public class Expediter : MonoBehaviour, ISurface
{
    [SerializeField] private ActiveOrders active;
    
    public bool HasItem => false;
    public bool Place(Item item)
    {
        if (item is Food food)
        {
            Destroy(item.gameObject);
            return active.Remove(food);
        }

        return false;
    }

    public Item Pick() => null;
    public bool Combine(Item item) => false;
}
