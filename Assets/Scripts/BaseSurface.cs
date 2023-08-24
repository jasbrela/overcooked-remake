using UnityEngine;

public class BaseSurface : MonoBehaviour, ISurface
{
    [SerializeField] private Item startingItem;
    [SerializeField] protected Transform snapPoint;
    
    protected Item CurrentItem;
    public Item Item => CurrentItem;
    
    public bool HasItem => CurrentItem != null;

    void Start()
    {
        if (startingItem)
        {
            Place(Instantiate(startingItem, snapPoint.position, snapPoint.rotation));
        }
    }
    
    public bool Place(Item item)
    {
        if (HasItem) return false;
        
        CurrentItem = item;
        
        Transform t = item.gameObject.transform;
        t.position = snapPoint.position;
        t.rotation = snapPoint.rotation;

        t.parent = snapPoint;
        
        OnItemPlaced();
        return true;
    }

    protected virtual void OnItemPlaced() { }
    protected virtual void OnItemPicked() { }
    
    public Item Pick()
    {
        Item temp = CurrentItem;
        
        CurrentItem = null;
        
        OnItemPicked();
        
        return temp;
    }

    public bool Combine(Item item)
    {
        if (!HasItem) return false;
        
        var combined = item.CheckCombination(CurrentItem);
                    
        combined ??= CurrentItem.CheckCombination(item);

        if (combined == null) return false;
        
        Destroy(item.gameObject);
        Destroy(CurrentItem.gameObject);
        
        CurrentItem = Instantiate(combined, snapPoint.position, snapPoint.rotation);
        
        return true;
    }
}
