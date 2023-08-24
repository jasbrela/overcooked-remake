using UnityEngine;

public class BaseSurface : MonoBehaviour, ISurface
{
    [SerializeField] private Item startingItem;
    [SerializeField] protected Transform snapPoint;
    
    protected Item CurrentItem;
    public bool HasItem => CurrentItem != null;

    void Start()
    {
        if (startingItem) Place(startingItem);
    }


    public void Place(Item item)
    {
        CurrentItem = item;
        
        Transform t = item.gameObject.transform;
        t.position = snapPoint.position;
        t.rotation = snapPoint.rotation;
    }

    public Item Pick()
    {
        Item temp = CurrentItem;
        
        CurrentItem = null;
        
        return temp;
    }
}
