using Player;
using UnityEngine;

public class BaseStation : BaseSurface, IInteractable
{
    [SerializeField] private bool hold;
    [SerializeField] private ProgressBar progress;
    [SerializeField] private StationType type;
    
    public bool MustHold => hold;

    private void Start()
    {
        if (MustHold) progress.OnFinish += OnFinish;
    }

    private void OnFinish()
    {
        Craft();
    }

    public void OnInteract()
    {
        if (CurrentItem == null) return;
        if (CurrentItem.GetCraftedItem(type) == null) return;
        
        if (MustHold)
        {
            progress.StartProgress();
            return;
        }
        
        Craft();
    }

    private void Craft()
    {
        if (CurrentItem == null) return;

        var item = CurrentItem.GetCraftedItem(type);
        if (item == null) return;
        
        Destroy(CurrentItem.gameObject);
        CurrentItem = Instantiate(item, snapPoint.position, snapPoint.rotation);
    }

    public void CancelInteraction()
    {
        progress.PauseProgress();
    }
}
