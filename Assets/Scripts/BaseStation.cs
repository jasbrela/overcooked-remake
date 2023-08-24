using Player;
using UnityEngine;

public class BaseStation : MonoBehaviour, IInteractable, ISurface
{
    [SerializeField] private Transform snapPoint;
    [SerializeField] private Item startingItem;
    [SerializeField] private bool hold;
    [SerializeField] private ProgressBar progress;
    [SerializeField] private StationType type;

    private Item _currentItem;

    public bool HasItem => _currentItem != null;

    public bool MustHold => hold;

    private void Start()
    {
        if (startingItem) Place(startingItem);
        if (MustHold) progress.OnFinish += OnFinish;
    }

    private void OnFinish()
    {
        Craft();
    }

    public void OnInteract()
    {
        if (_currentItem == null) return;
        
        if (MustHold)
        {
            progress.StartProgress();
            return;
        }
        
        Craft();
    }

    private void Craft()
    {
        if (_currentItem == null) return;

        var item = _currentItem.GetCraftedItem(type);
        if (item == null) return;
        
        Destroy(_currentItem.gameObject);
        _currentItem = Instantiate(item, snapPoint.position, snapPoint.rotation);
    }

    public void CancelInteraction()
    {
        progress.PauseProgress();
    }
    
    public void Place(Item item)
    {
        _currentItem = item;
        
        Transform t = item.gameObject.transform;
        t.position = snapPoint.position;
        t.rotation = snapPoint.rotation;
    }

    public Item Pick()
    {
        Item temp = _currentItem;
        
        _currentItem = null;
        
        return temp;
    }
}
