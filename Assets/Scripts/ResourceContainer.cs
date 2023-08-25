using UnityEngine;

public class ResourceContainer : MonoBehaviour, ISurface
{
    [SerializeField] private Food resource;

    public bool HasItem => true;
    
    /// <summary>
    /// Puts the resource back if it's the same as this container's resource.
    /// </summary>
    /// <param name="item">The resource you want to put back.</param>
    /// <returns>Whether the operation was successful.</returns>
    public bool Place(Item item)
    {
        if (item.Id != resource.Id) return false;
        
        Destroy(item.gameObject);
        
        return true;
    }

    /// <summary>
    /// Pick the item this container stores.
    /// </summary>
    /// <returns>an Item equal to this container's resource</returns>
    public Item Pick()
    {
        return Instantiate(resource);
    }
    
    /// <summary>
    /// A resource container can't combine items.
    /// </summary>
    /// <returns>false</returns>
    public bool Combine(Item item)
    {
        return false;
    }
}
