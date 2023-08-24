public class ManualStation : BaseStation, IInteractable
{
    public void OnInteract() => Use();
    public void CancelInteraction() => ProgressBar.PauseProgress();
}
