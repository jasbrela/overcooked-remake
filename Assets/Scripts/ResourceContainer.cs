using UnityEngine;

public class ResourceContainer : MonoBehaviour, ISurface
{
    [SerializeField] private Item resource;

    public bool HasItem => true;
    public bool Place(Item item)
    {
        if (item.Id != resource.Id) return false;
        
        Destroy(item.gameObject);
        
        return true;
    }

    public Item Pick()
    {
        return Instantiate(resource);
    }

    public bool Combine(Item item)
    {
        return false;
    }
}
