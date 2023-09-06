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
        
        CurrentItem.Snap(snapPoint);
        
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
    /// <returns>Whether the combination was successful</returns>
    public bool Combine(Item item)
    {
        if (!HasItem) return false;

        if (CurrentItem is Food currentFood)
        {
            if (item is Food food)
                return CombineFood(currentFood, food);

            if (item is Plate plate && currentFood.IsPlateable)
                return currentFood.Plate(plate.Pick());
            
        }
        
        if (CurrentItem is Plate currentPlate)
        {
            if (currentPlate.CurrentStack > 1)
            {
                if (item is Food { IsPlateable: true } food2)
                {
                    food2.Plate(currentPlate.Pick());
                    return false;
                }
            }
            
            if (item is Food { IsPlateable: true } food)
            {
                if (food.Plate(currentPlate.Pick()))
                {
                    CurrentItem = null;
                    return Place(food);
                }
            }
            
            if (item is Plate plate)
                return currentPlate.Stack(plate);
            
        }

        return false;
    }

    private bool CombineFood(Food current, Food other)
    {
        var combined = other.CheckCombination(current);

        combined ??= current.CheckCombination(other);

        if (combined == null) return false;

        OnItemCombined();
        Plate p = null;
        
        if (other.IsPlated) p = other.RemovePlate();
        if (current.IsPlated) p = current.RemovePlate();
        
        Destroy(other.gameObject);
        Destroy(current.gameObject);

        CurrentItem = Instantiate(combined, snapPoint.position, snapPoint.rotation);
        if (p != null) ((Food)CurrentItem).Plate(p);
                
        return true;
    }
}
