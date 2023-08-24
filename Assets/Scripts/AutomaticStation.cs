public class AutomaticStation : BaseStation
{
    protected override void OnItemPlaced() => Use();
}