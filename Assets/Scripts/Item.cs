using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Item handsResult;
    [SerializeField] private Item cutResult;
    [SerializeField] private Item stoveResult;

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
}
