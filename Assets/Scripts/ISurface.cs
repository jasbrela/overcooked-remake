public interface ISurface
{
    public bool HasItem { get; }
    public void Place(Item item);
    public Item Pick();
}
