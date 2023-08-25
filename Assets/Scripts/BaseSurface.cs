using UnityEngine;

public class BaseSurface : MonoBehaviour, ISurface
{
    [SerializeField] private Item startingItem;
    [SerializeField] protected Transform snapPoint;
    
    protected Item CurrentItem;
    
    public bool HasItem => CurrentItem != null;

    void Start()
    {
        if (startingItem)
        {
            Place(Instantiate(startingItem, snapPoint.position, snapPoint.rotation));
        }
    }
    
    /// <summary>
    /// Place a item on the surface.
    /// </summary>
    /// <param name="item">The item you want to place</param>
    /// <returns>If the operation was successful.</returns>
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
    protected virtual void OnItemCombined() { }
    
    /// <summary>
    /// Picks the item from the surface.
    /// </summary>
    /// <returns>the surface current's item</returns>
    public Item Pick()
    {
        if (!HasItem) return null;
        
        Item item = CurrentItem;

        if (CurrentItem is Plate plate)
        {
            if (plate.CurrentStack == 1)
            {
                CurrentItem = null;
            }

            item = plate.Pick();

        }
        else CurrentItem = null;
        
        OnItemPicked();
        
        return item;
    }

    /// <summary>
    /// Try to make a combination out of the current's item and the provided item.
    /// If one of them is a plate and other is a food, then plates the food.
    /// If both of them are foods, try to combine them into a new food.
    /// If both of them are plates, stack the plates.
    /// </summary>
    /// <param name="item">The food you provide for the combination.</param>
    /// <returns>If the combination was a success</returns>
    public bool Combine(Item item)
    {
        if (!HasItem) return false;

        if (CurrentItem is Food currentFood)
        {
            if (item is Food food)
            {
                var combined = food.CheckCombination(currentFood);

                combined ??= currentFood.CheckCombination(food);

                if (combined == null) return false;

                OnItemCombined();

                Destroy(food.gameObject);
                Destroy(currentFood.gameObject);

                CurrentItem = Instantiate(combined, snapPoint.position, snapPoint.rotation);
                
                return true;
            }
            
            if (item is Plate plate)
            {
                if (!currentFood.IsPlateable) return false;
                return currentFood.Plate(plate.Pick());
            }
        }
        
        if (CurrentItem is Plate currentPlate)
        {
            if (item is Food food)
            {
                if (!food.IsPlateable) return false;

                CurrentItem = food;
                return food.Plate(currentPlate.Pick());
            }
            
            if (item is Plate plate)
            {
                return currentPlate.Stack(plate);
            }
        }

        return false;
    }
}
