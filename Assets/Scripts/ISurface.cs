public interface ISurface
{
    public bool HasItem { get; }
    public Item Item { get; }
    public bool Place(Item item);
    public Item Pick();
    public bool Combine(Item item);
}
