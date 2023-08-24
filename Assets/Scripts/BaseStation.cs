using Player;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseStation : BaseSurface
{
    [SerializeField] private bool waitToCook = true;
    [SerializeField] private ProgressBar progress;
    [SerializeField] private StationType stationType;

    protected ProgressBar ProgressBar => progress;
    private bool WaitToCook => waitToCook;

    private void Start()
    {
        if (WaitToCook) progress.OnFinish += OnFinish;
    }

    private void OnFinish() => Cook();
    
    /// <summary>
    /// If the player must hold to cook, waits for it before cooking the item.
    /// Otherwise, cooks immediately.
    /// </summary>
    protected void Use()
    {
        if (CurrentItem == null) return;
        if (CurrentItem.GetCraftedItem(stationType) == null) return;

        if (WaitToCook)
        {
            progress.StartProgress();
        } else Cook();
    }

    protected override void OnItemPicked() => progress.ResetProgress();
    protected override void OnItemCombined() => progress.ResetProgress();

    /// <summary>
    /// Turn the current item into the final cooked item.
    /// </summary>
    private void Cook()
    {
        if (!HasItem) return;

        var item = CurrentItem.GetCraftedItem(stationType);
        if (item == null) return;
        
        Destroy(CurrentItem.gameObject);
        CurrentItem = Instantiate(item, snapPoint.position, snapPoint.rotation);
    }
}