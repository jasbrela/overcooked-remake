public interface IInteractable
{
    public bool MustHold { get; }
    
    public void OnInteract();
    public void CancelInteraction();
}
