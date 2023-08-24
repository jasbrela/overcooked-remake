public interface ISurface
{
    public bool HasItem { get; }
    public bool Place(Item item);
    public Item Pick();
    public bool Combine(Item item);
}
