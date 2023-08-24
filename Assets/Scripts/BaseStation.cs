using Player;
using UnityEngine;

public abstract class BaseStation : BaseSurface
{
    [SerializeField] private bool hold = true;
    [SerializeField] private ProgressBar progress;
    [SerializeField] private StationType stationType;

    protected ProgressBar ProgressBar => progress;
    public bool MustHold => hold;

    private void Start()
    {
        if (MustHold) progress.OnFinish += OnFinish;
    }

    private void OnFinish()
    {
        Cook();
    }
    
    protected void Use()
    {
        if (CurrentItem == null) return;
        if (CurrentItem.GetCraftedItem(stationType) == null) return;

        if (MustHold)
        {
            progress.StartProgress();
        } else Cook();
    }

    protected override void OnItemPicked() => progress.ResetProgress();

    private void Cook()
    {
        if (!HasItem) return;

        var item = CurrentItem.GetCraftedItem(stationType);
        if (item == null) return;
        
        Destroy(CurrentItem.gameObject);
        CurrentItem = Instantiate(item, snapPoint.position, snapPoint.rotation);
    }
}